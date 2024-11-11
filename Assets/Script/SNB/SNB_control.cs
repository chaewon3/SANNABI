using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SNB_control : MonoBehaviour
{
    #region public 변수
    public float moveSpeed = 6.0f;
    public float Maxchain = 10.0f;
    public GameObject Aim;
    public AudioClip[] arrAudio;
    public AudioSource audioManager;
    public GameObject dashHUD;
    #endregion

    #region private 변수
    SpriteRenderer[] sprite;
    Animator[] animator;
    RayCast linecheck;
    ChainGrab chaingrab;
    GameObject grabbed;
    DistanceJoint2D joint;
    Shadow shadow;

    bool isGround;
    bool isWall;
    bool isCeiling = false;
    bool isHang;
    int isRight;
    bool isClick;
    bool isRiding = true;
    bool cooltime = true;
    Vector3 hitPoint;
    Rigidbody2D rigid;
    GameObject Aimming;
    IEnumerator coroutine;
    #endregion

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponentsInChildren<SpriteRenderer>();
        animator = gameObject.GetComponentsInChildren<Animator>();
        linecheck = gameObject.GetComponent<RayCast>();
        chaingrab = FindObjectOfType<ChainGrab>();
        shadow = FindObjectOfType<Shadow>();
        joint = GetComponent<DistanceJoint2D>();
    }
    
    void Update()
    {
        if (isRiding)
        {
            MoveSNB();
        }
        foreach (Animator animation in animator)
        {
            animation.SetFloat("Speed", rigid.velocity.magnitude);
        }
    }

    void FixedUpdate()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position + Vector3.down * 0.65f, Vector3.down, 0.2f, LayerMask.GetMask("Ground"));

        if (groundHit.collider != null)
        {
            foreach (Animator animation in animator)
            {
                animation.SetBool("IsGround", true);
                if(!isGround)
                {
                    animation.SetTrigger("Ground");
                }
            }
            isGround = true;
        }
        else
        {
            foreach (Animator animation in animator)
            {
                animation.SetBool("IsGround", false);
            }
            isGround = false;
        }

        RaycastHit2D wallhit = Physics2D.Raycast(transform.position - Vector3.right * 0.5f, Vector2.right, 1f, LayerMask.GetMask("Ground"));

        if (wallhit.collider != null)
        {
            if(!isWall)
            {
                sprite[1].transform.localPosition = new Vector2(0.172f, -0.096f);
            }
            if (!isWall && !isCeiling)
            {
                foreach (Animator animation in animator)
                {
                    animation.ResetTrigger("Jump");
                    animation.SetTrigger("IsWall");
                }
            }
            isWall = true;
            if (wallhit.point.x < transform.position.x)
            {
                isRight = 1;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                isRight = -1;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            if (isWall)
            {
                sprite[1].transform.localPosition = new Vector2(-0.203f, -0.204f);

                rigid.simulated = true;
                foreach (Animator animation in animator)
                {
                    animation.SetTrigger("Fall");
                }
            }
            isWall = false;
        }

        RaycastHit2D ceilingHit = Physics2D.Raycast(transform.position + Vector3.up * 0.4f, Vector3.up, 0.4f, LayerMask.GetMask("Ground"));
        if (ceilingHit.collider != null)
        {
            isCeiling = true;
            foreach (Animator animation in animator)
            {
                animation.SetBool("Falling", false);
            }
        }
        else
        {
            isCeiling = false;
            foreach (Animator animation in animator)
            {
                animation.SetBool("Falling", true);
            }
        }
    }

    void MoveSNB()
    {
        Vector2 moveDirection = rigid.velocity.normalized;

        if (Input.GetKeyDown(KeyCode.D) && !isWall)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.A) && !isWall)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetButton("Horizontal"))
        {
            float move = Input.GetAxisRaw("Horizontal");
            move = move * moveSpeed * Time.deltaTime;
            rigid.transform.Translate(Vector2.right * move);

            foreach (Animator animation in animator)
            {
                animation.SetBool("Run", true);
            }
        }

        if (Input.GetButtonUp("Horizontal"))
        {
            foreach (Animator animation in animator)
            {
                animation.SetBool("Run", false);
            }
        }

        if(Input.GetKey(KeyCode.W) && isWall)
        {
            float move = 4 * Time.deltaTime;
            rigid.transform.Translate(Vector2.up * move);
            foreach (Animator animation in animator)
            {
                animation.SetBool("ClimbUp", true);
            }
        }

        if (Input.GetKey(KeyCode.S) && isWall)
        {
            float move = 8 * Time.deltaTime;
            rigid.transform.Translate(Vector2.down * move);
            foreach (Animator animation in animator)
            {
                animation.SetBool("ClimbDown", true);
            }
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            foreach (Animator animation in animator)
            {
                animation.SetBool("ClimbUp", false);
            }
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            foreach (Animator animation in animator)
            {
                animation.SetBool("ClimbDown", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.velocity = Vector2.zero;
            if (isGround == true)
            {
                audioManager.PlayOneShot(arrAudio[1], 0.2f);
                rigid.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
                foreach (Animator animation in animator)
                {
                    animation.SetTrigger("Jump");
                }
            }

            if (isWall == true)
            {
                rigid.simulated = true;
                foreach (Animator animation in animator)
                {
                    animation.SetTrigger("Jump");
                }
                rigid.velocity = new Vector2(isRight * 3f, 15f);
            }
            
            if(isClick)
            {
                audioManager.PlayOneShot(arrAudio[2], 0.2f);
                isHang = true;
                float starttime = Time.time;
                shadow.ShadowOn();
                StartCoroutine(ToHitpoint(starttime));
            }
        }

        if(Input.GetMouseButtonDown(0) && linecheck.LineCheck)
        {
            isHang = false;
            sprite[1].flipX = true;

            sprite[1].transform.localPosition = new Vector2(-0.203f, -0.005f);
            animator[1].SetTrigger("Grab");
            Grab();

            isClick = true;            
        }

        if(Input.GetMouseButtonUp(0) && isClick)
        {
            foreach (Animator animation in animator)
            {
                animation.SetTrigger("MouseUp");
            }
            if (!isHang)
            {
                chain();
            }
            isClick = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isClick && cooltime)
        {
            dashHUD.SetActive(true);
            cooltime = false;
            audioManager.PlayOneShot(arrAudio[3], 0.2f);
            Vector2 force = new Vector2(15f, 25f);
            rigid.AddForce(force* moveDirection, ForceMode2D.Impulse);
            shadow.ShadowOn();
            Invoke("Cooltime", 2.5f);
        }       
    }
    void Cooltime()
    {
        cooltime = true;
    }

    void chain()
    {
        isClick = false;
        sprite[1].flipX = false;
        Destroy(grabbed, 0f);
        transform.GetChild(1).rotation = Quaternion.Euler(0f, 0f, 0f);
        sprite[1].transform.localPosition = new Vector2(-0.203f, -0.204f);
        joint.enabled = false;
    }

    void Grab()
    {
        audioManager.PlayOneShot(arrAudio[0], 0.3f);
        hitPoint = linecheck.hitPoint;
        Vector3 direction = (hitPoint - transform.GetChild(1).position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.GetChild(1).rotation = Quaternion.Euler(0f, 0f, angle);

        if (!isClick)
        {
            chaingrab.chainShoot(transform.GetChild(1).position, hitPoint);
        }

        if(linecheck.hit.collider.tag == "Ground")
        {
            float dis = direction.magnitude;
            if (dis > 7)
            {
                GetComponent<DistanceJoint2D>().distance = 7;
            }
            else
            {
                GetComponent<DistanceJoint2D>().distance = dis;
            }
            joint.enabled = true;
            joint.connectedAnchor = hitPoint;
            
            foreach (Animator animation in animator)
            {
                animation.ResetTrigger("Fall");
                animation.SetTrigger("Hang");
            }
        }
        else if (linecheck.hit.collider.tag == "SkyLine")
        {
            isHang = true;
            chain();
            float starttime = Time.time;
            coroutine = CarRide(starttime, linecheck.hit.collider);
            StartCoroutine(coroutine);
        }

    }

    IEnumerator ToHitpoint(float starttime)
    {
        chain();
        Vector3 startposition = transform.position;
        Vector3 destination = hitPoint;
        if (startposition.y < hitPoint.y)
        {
            destination += Vector3.down * 0.5f;
        }
        else
        {
            destination += Vector3.up * 0.5f;
        }

        if (startposition.x < hitPoint.x)
        {
            destination += Vector3.left * 0.3f;
        }
        else
        {
            destination += Vector3.right * 0.3f;
        }

        while (transform.position != destination)
        {
            float t = (Time.time - starttime) / 0.2f;
            transform.position = Vector3.Lerp(startposition, destination, t);
            yield return null;
        }

        rigid.simulated = false;

        if (isCeiling)
        {
            yield return new WaitForSeconds(0.2f);
            foreach (Animator animation in animator)
            {
                animation.SetBool("IsCeiling", true);
            }

            sprite[1].transform.localPosition = new Vector2(-0.203f, 0.296f);

            while (Input.GetMouseButton(0))
            {
                if (!isCeiling)
                {
                    break;
                }
                yield return null;
            }

            rigid.simulated = true;

            sprite[1].transform.localPosition = new Vector2(-0.203f, -0.204f);
            foreach (Animator animation in animator)
            {
                animation.SetBool("IsCeiling", false);
            }
        }

    }

    IEnumerator CarRide(float starttime, Collider2D collider)
    {
        Destroy(chaingrab.Grabbed, 0f);
        chaingrab.lineRenderer.enabled = false;
        isRiding = false;
        transform.localScale = new Vector3(1, 1, 1);
        Vector2 startposition = rigid.position;
        Vector2 destination = hitPoint;
        foreach (Animator animation in animator)
        {
            animation.SetTrigger("RideStart");
        }

        while (rigid.position != destination)
        {
            float t = (Time.time - starttime) / 0.2f;
            rigid.position = Vector3.Lerp(startposition, destination, t);
            yield return null;
        }
        rigid.velocity = Vector3.zero;
        gameObject.transform.SetParent(collider.transform);
        this.transform.localPosition = new Vector3(0.26699999f, -1.44000006f, 0);
        sprite[1].transform.localPosition = new Vector3(-0.20300293f, 0.342000008f, 0);

        rigid.isKinematic = true;
        Aimming = Instantiate(Aim, transform.position, transform.rotation);
        Aimming.transform.parent = this.transform;

        while (!Input.GetMouseButtonUp(0))
        {
            yield return null;
        }
        foreach (Animator animation in animator)
        {
            animation.SetTrigger("Dash");
        }
        yield return new WaitForSeconds(0.12f);
        sprite[1].transform.localPosition = new Vector3(-0.20300293f, -0.204000235f, 0);
        gameObject.transform.SetParent(null);
        Destroy(Aimming, 0f);
        rigid.isKinematic = false;
        if(linecheck.worldPosition.x <= rigid.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        StartCoroutine(SlowTime());
        starttime = 0;
        while (starttime <= 0.1)
        {
            starttime += Time.deltaTime;
            rigid.transform.Translate(linecheck.direction * 50f * Time.deltaTime);
            yield return null;
        }
        isRiding = true;
        isClick = false;
    }

    IEnumerator SlowTime()
    {
        Time.timeScale = 0.05f;
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(0.3f);
        while(Time.timeScale != 1)
        {
            Time.timeScale += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        }
    }

    public void EndSkyline()
    {
        StopCoroutine(coroutine);
        sprite[1].transform.localPosition = new Vector3(-0.20300293f, -0.204000235f, 0);
        gameObject.transform.SetParent(null);
        Destroy(Aimming, 0f);
        rigid.isKinematic = false;
        isRiding = true;
        isClick = false;

    }
}


