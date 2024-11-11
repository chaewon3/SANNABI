using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTitle : MonoBehaviour
{
    [SerializeField]
    Sprite lightOff;
    [SerializeField]
    GameObject light;

    void Start()
    {
        StartCoroutine(StartScenes());
        Cursor.visible = false;
    }

    IEnumerator StartScenes()
    {
        Vector3 startposition = transform.position;
        float starttime = Time.time;
        while (transform.position != Vector3.zero)
        {
            float t = (Time.time - starttime) / 6f;
            transform.position = Vector3.Lerp(startposition, Vector3.zero, t);
            yield return null;
        }
        GetComponent<SpriteRenderer>().sprite = lightOff;
        light.SetActive(true);
        yield return null;
    }
}
