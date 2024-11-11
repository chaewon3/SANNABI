using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class DeathEvent : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public GameObject background;
    public GameObject BGM;
    public AudioClip Death;
    public AudioSource audioManager;
    public GameObject SNB;


    private void OnEnable()
    {
        StartCoroutine(Movecamera());
        background.SetActive(true);
        BGM.SetActive(false);
        audioManager.PlayOneShot(Death, 2.8f);
        Invoke("restart", 3.2f);     
    }

    IEnumerator Movecamera()
    {
        virtualCamera.Follow = SNB.transform;
        float time = 0;
        float starttime = Time.time;
        float startSize = virtualCamera.m_Lens.OrthographicSize;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y = 0;
        while (time <= 0.5f)
        {
            time += Time.deltaTime;
            float t = (Time.time - starttime) / 0.5f;
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, 3.5f, t);
            yield return null;
        }
    }

    void restart()
    {
        SceneManager.LoadScene("Chapter2");
    }
}
