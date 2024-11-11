using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    Animator animation;
    public int point;
    public AudioClip Save;
    public AudioSource audioSource;

    void Awake()
    {
        animation = GetComponent<Animator>();
        if(GameManager.Instance.check[point])
        {
            animation.SetBool("Saved", true);
        }
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animation.SetTrigger("Save");
        }

        GameManager.Instance.SaveCheckPoint(this.transform.position, point);

        if (!GameManager.Instance.check[point])
        {
            audioSource.PlayOneShot(Save, 0.3f);
        }

    }

}
