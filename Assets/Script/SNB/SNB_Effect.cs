using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SNB_Effect : MonoBehaviour
{
    public GameObject[] Effects;

    Collider2D SNBCol;
    Animator[] animation;
    bool isGround;

    void Start()
    {
        SNBCol = GetComponent<Collider2D>();
        animation = gameObject.GetComponentsInChildren<Animator>();

    }

    void Update()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position + Vector3.down * 0.65f, Vector3.down, 0.2f, LayerMask.GetMask("Ground"));
        if(groundHit.collider != null)
        {
            if(!isGround)
            {
                GameObject LandingEff = Instantiate(Effects[1], transform.position + Vector3.down * 0.6f, transform.rotation);
                Destroy(LandingEff, 0.3f);
            }
            isGround = true;
        }
        else
        {
            isGround = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            GameObject JumpEff = Instantiate(Effects[0], transform.position+Vector3.up*0.1f, transform.rotation);
            Destroy(JumpEff, 0.65f);
        }

        
    }

}
