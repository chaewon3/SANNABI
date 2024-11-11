using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    #region public º¯¼ö
    public GameObject target;
    public float moveSpeed = 0.1f;

    public Vector3 adjustCamPos;
    public Vector2 minCamLimit;
    public Vector2 maxCamLimit;
    #endregion


    void Start()
    {
        this.transform.position = target.transform.position - Vector3.down*3;
    }
    
    void LateUpdate()
    {
        cameraMove();
    }
    void cameraMove()
    {
        Vector3 pos = Vector3.Lerp(transform.position, target.transform.position - Vector3.down * 3, moveSpeed);

        transform.position = new Vector3
            (
               pos.x+adjustCamPos.x,
               pos.y+adjustCamPos.y,
               -10f + adjustCamPos.z
            );
    }
}
