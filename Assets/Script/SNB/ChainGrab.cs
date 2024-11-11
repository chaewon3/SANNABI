using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainGrab : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject flySpr;
    public GameObject Arm;
    public GameObject grabSpr;

    float animaDuration = 0.1f;
    GameObject Flying;
    [HideInInspector]
    public GameObject Grabbed;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }
    
    public void chainShoot(Vector3 from, Vector3 to)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
        StartCoroutine(Shoot(from, to));
    }


    IEnumerator Shoot(Vector3 from, Vector3 to)
    {
        float startTime = Time.time;
        Vector3 pos = from;

        Flying = Instantiate(flySpr, transform.position, transform.rotation);

        //이미지 방향 정하고 고정시킴
        Vector3 direction = (to - from);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Flying.transform.rotation = Quaternion.Euler(0f, 0f, angle-90);

        Grabbed = Instantiate(grabSpr, to, Quaternion.Euler(0f, 0f, angle - 90));
        while (pos != to)
        {
            float t = (Time.time - startTime) / animaDuration;
            pos = Vector3.Lerp(from, to, t);
            lineRenderer.SetPosition(1, pos);
            Flying.transform.position = pos;

            yield return null;
        }

        Destroy(Flying, 0f);

        while (true)
        {
            lineRenderer.SetPosition(0, Arm.transform.position);

            if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
            {
                break;
            }
            yield return null;
        }

        Destroy(Grabbed, 0f);
        lineRenderer.enabled = false;
    }



}
