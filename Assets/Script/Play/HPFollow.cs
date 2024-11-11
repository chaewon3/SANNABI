using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPFollow : MonoBehaviour
{
    public GameObject target;

    void Start()
    {
        this.transform.position = target.transform.position + Vector3.up * 0.7f + Vector3.left * 1f;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Destroy(this.gameObject, 0f);
        }
        Vector3 pos = Vector3.Lerp(transform.position, target.transform.position + Vector3.up * 0.7f + Vector3.left * 1f, 0.1f);
        transform.position = pos;
    }
}
