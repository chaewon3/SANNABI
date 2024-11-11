using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    public LayerMask GroundMask;
    public LayerMask CarMask;

    public LineRenderer lineRenderer;
    public GameObject point;

    public Vector2 direction;
    public Vector3 worldPosition;

    [SerializeField]
    private Transform owner;
    [HideInInspector]
    public bool LineCheck;
    public Vector3 hitPoint;
    public RaycastHit2D hit;
    Vector3 ownercenter;
    bool isClick = false;

    void Awake()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        point.SetActive(false);
    }

    void Update()
    {
        ownercenter = owner.transform.position;

        Vector3 mousePosition = Input.mousePosition;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0;

        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);

        worldPosition = Camera.main.ViewportToWorldPoint(viewportPosition);
        worldPosition.z = 0;

        direction = (worldPosition - ownercenter).normalized;
        
        hit = Physics2D.Raycast(ownercenter, direction, 15, GroundMask | CarMask);

        if(Input.GetMouseButtonDown(0))
        {
            isClick = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            isClick = false;
        }
        
        
        if (hit.collider != null && !isClick)
        {
            if (hit.collider.tag == "Hang")
            {
                Stop();
                LineCheck = false;
            }
            if (hit.collider.tag == "Ground")
            {
                Play(ownercenter, hit.point);
                hitPoint = hit.point;
                LineCheck = true;
            }
            else if (hit.collider.tag == "SkyLine")
            {
                Vector3 ObjectPoint = hit.collider.transform.position + Vector3.down * 1.3f;
                Play(ownercenter, ObjectPoint);
                hitPoint = ObjectPoint;
                LineCheck = true;
            }

        }
        else
        {
            Stop();
            LineCheck = false;
        }
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
