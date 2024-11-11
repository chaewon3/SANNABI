using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashFollow : MonoBehaviour
{
    public GameObject target;

    void Start()
    {
        this.transform.position = target.transform.position + Vector3.up * 0.58f + Vector3.right * 1.15f;
    }

    private void OnEnable()
    {
        StartCoroutine(DashCooltime());
        StartCoroutine(enable());
    }

    IEnumerator DashCooltime()
    {
        float time = 0;
        while(time < 2.5f)
        {
            if (target == null)
            {
                Destroy(this.gameObject, 0f);
            }
            Vector3 pos = Vector3.Lerp(transform.position, target.transform.position + Vector3.up * 0.58f + Vector3.right * 1.15f, 0.1f);
            transform.position = pos;
            time += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator enable()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<Animator>().SetTrigger("Off");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
