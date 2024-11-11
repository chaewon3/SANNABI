using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyLineCar : MonoBehaviour
{
    public Material[] mat;
    public Transform[] paths;

    public AudioSource audioSource;
    public AudioClip Destory;

    [SerializeField]
    bool EndPoint;
    [SerializeField]
    SNB_control SNB;
    [SerializeField]
    float moveSpeed = 10f;

    Vector3 SpawnPoint;
    Animator animation;

    Coroutine move;
    bool isclick;
    bool isride;

    void Awake()
    {
        SpawnPoint = transform.position;
        animation = GetComponent<Animator>();
        SNB = FindObjectOfType<SNB_control>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isclick = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            isclick = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && isclick)
        {
            isride = true;
            animation.SetBool("Ride", true);
            move = StartCoroutine(MovePath());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Cursor")
        {
            gameObject.GetComponent<SpriteRenderer>().material = mat[1];
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isride)
        {
            isride = false;
            audioSource.PlayOneShot(Destory, 0.3f);
            animation.SetBool("Ride", false);
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(respawn());
            StopCoroutine(move);
        }
        if (collision.gameObject.tag == "Cursor")
        {
            gameObject.GetComponent<SpriteRenderer>().material = mat[0];
        }

    }

    IEnumerator respawn()
    {
        yield return new WaitForSeconds(0.95f);
        gameObject.GetComponent<Renderer>().enabled = false;
        transform.position = SpawnPoint;
        yield return new WaitForSeconds(3f); 
        animation.SetTrigger("Respawn");
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(1f);
        GetComponent<Collider2D>().enabled = true;
    }

    IEnumerator MovePath()
    {
        int pathnum = 0;
        while (true)
        {
            transform.position = Vector2.MoveTowards
                (transform.position, paths[pathnum].transform.position, moveSpeed * Time.deltaTime);
            if(transform.position == paths[pathnum].transform.position)
            {
                pathnum++;
            }
            if(pathnum == paths.Length && !EndPoint)
            {
                pathnum = 0;
            }
            if(pathnum == paths.Length && EndPoint)
            {
                SNB.EndSkyline();
                animation.SetBool("Ride", false);
                GetComponent<Collider2D>().enabled = false;
                StartCoroutine(respawn());
                StopCoroutine(move);
            }
            yield return null;
        }
    }
}
