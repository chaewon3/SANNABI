using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public GameObject SNB;
    public GameObject WalkSNB;
    public GameObject black;
    public AudioSource BGM;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            SNB.GetComponent<SNB_control>().enabled = false;
            WalkSNB.transform.position = SNB.transform.position;
            StartCoroutine(walk());
            StartCoroutine(VolumeDown());
        }
    }

    IEnumerator walk()
    {
        float starttime = 0;
        Destroy(SNB, 0);
        WalkSNB.SetActive(true);
        while (starttime <= 8) 
        {
            WalkSNB.transform.Translate(Vector2.right * 2 * Time.deltaTime);
            starttime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Main");
    }

    IEnumerator VolumeDown()
    {
        float starttime = Time.time;
        float startVolume = BGM.volume;
        float time = 0;
        while(time < 5)
        {
            Debug.Log("사운드 내려가는중");
            float t = (Time.time - starttime) / 5;
            BGM.volume = Mathf.Lerp(startVolume, 0, t);
            time += Time.deltaTime;
            yield return null;
        }
        BGM.volume = 0f;
        black.SetActive(true);
    }
}
