using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SNB_collision : MonoBehaviour
{
    public Animator HP;
    public GameObject HPHUD;
    public GameObject deathEvent;
    public GameObject Camera;
    public AudioSource audiomanager;
    public AudioClip[] arrAudio;

    Rigidbody2D rigid;
    Animator[] animator;

    int Hp = 4;
    bool DashCooltime = false;
    Vector2 movedirection = Vector2.zero;
    Coroutine slow;
    float angle;

    void Awake()
    {
        Vector3 checkpointPosition = GameManager.Instance.LoadCheckPoint();
        transform.position = checkpointPosition;
        rigid = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentsInChildren<Animator>();
    }

    void Update()
    {
        DashMove();
        if (DashCooltime && Input.GetKeyDown(KeyCode.Space))
        {
            audiomanager.PlayOneShot(arrAudio[1], 0.5f);
            if (movedirection == Vector2.up)
            {
                rigid.transform.rotation = Quaternion.Euler(0f, 0f, 90);
            }
            foreach (Animator animation in animator)
            {
                animation.SetTrigger("Dash");
            }
            Time.timeScale = 1;
            StopCoroutine(slow);
            rigid.AddForce(movedirection * 16f, ForceMode2D.Impulse);
            DashCooltime = false;
            Invoke("Rotation", 0.4f);
        }
        if (Hp == 0)
        {
            Camera.GetComponent<CameraFollowing>().enabled = false;
            Camera.GetComponent<DeathEvent>().enabled = true;
            Destroy(HPHUD, 0);
            deathEvent.transform.position = rigid.position;
            deathEvent.SetActive(true);
            Time.timeScale = 1;
            Destroy(this.gameObject, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DeathTile" && rigid.isKinematic == false)
        {
            audiomanager.PlayOneShot(arrAudio[0], 0.5f);
            Vector2 direciotn = rigid.velocity.normalized;
            rigid.velocity = Vector2.zero;
            rigid.AddForce(-direciotn * 16f, ForceMode2D.Impulse);
            Hp--;
            foreach (Animator animation in animator)
            {
                animation.SetTrigger("Damaged");
            }
            HP.SetTrigger("Damaged");
            CancelInvoke("restore");
            Invoke("restore", 4f);
            DashCooltime = true;
            Time.timeScale = 1;
            slow = StartCoroutine(SlowTime());
        }
        if (collision.gameObject.tag == "Border" )
        {
            Destroy(deathEvent, 0f);
            audiomanager.PlayOneShot(arrAudio[0], 0.2f);
            Camera.GetComponent<DeathEvent>().enabled = true;
            Destroy(this.gameObject, 0f);
        }
        if (collision.gameObject.tag == "Finish")
        {
            foreach (Animator animation in animator)
            {
                animation.SetBool("Run", false);
            }
        }
    }

    private void DashMove()
    {
        movedirection = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movedirection += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movedirection += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movedirection += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movedirection += Vector2.right;
        }
        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            movedirection += Vector2.up +Vector2.left;
        }
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
        {
            movedirection += Vector2.left + Vector2.down;
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            movedirection += Vector2.down + Vector2.right;
        }
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
        {
            movedirection += Vector2.right + Vector2.up;
        }


        movedirection = movedirection.normalized;
        angle = Mathf.Atan2(movedirection.y, movedirection.x) * Mathf.Rad2Deg;
    }

    IEnumerator SlowTime()
    {
        Time.timeScale = 0.05f;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(0.3f);
        while (Time.timeScale != 1)
        {
            Time.timeScale += Time.unscaledDeltaTime/2;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        }
        yield return new WaitForSecondsRealtime(0.35f);
        DashCooltime = false;
    }

    void Rotation()
    {
        rigid.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
    void restore()
    {
        HP.SetTrigger("Restore");
        Hp = 4;
    }
    

}
