using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePoint : MonoBehaviour
{
    LineRenderer lineRenderer;
    public GameObject point;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        point.SetActive(false);
    }

    public void Play(Vector3 from, Vector3 to)
    {
        lineRenderer.enabled = true;
        point.SetActive(true);

        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
        point.transform.position = to;
    }

    public void Stop()
    {
        lineRenderer.enabled = false;
        point.SetActive(false);
    }

}
