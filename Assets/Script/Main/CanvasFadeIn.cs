using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFadeIn : MonoBehaviour
{
    public GameObject cursor;
    void Start()
    {
        StartCoroutine(FadeIn());
    }
    
   IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(7.5f);
        float starttime = 0;
        Cursor.visible = true;

        while (starttime < 3)
        {
            float t = (Time.time - starttime) / 3f;
            GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, starttime / 3);
            starttime += Time.deltaTime;
            yield return null;
        }
        GetComponent<CanvasGroup>().alpha = 1;
        cursor.SetActive(true);
    }
}
