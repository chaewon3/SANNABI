using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainOption : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] arrAudio;

    public GameObject[] UI;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void GameStart()
    {
        GameManager.Instance.LoadScene("Chapter2");
    }

    public void GameExit()
    {
        UI[0].SetActive(false);
        UI[1].SetActive(true);
    }

    public void Yes()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void No()
    {
        UI[0].SetActive(true);
        UI[1].SetActive(false);
    }

    public void MouseHovering()
    {
        audioSource.PlayOneShot(arrAudio[0], 0.15f);
    }

    public void MouseClick()
    {
        audioSource.PlayOneShot(arrAudio[1], 0.15f);
    }
    public void Warning()
    {
        audioSource.PlayOneShot(arrAudio[2], 0.15f);
    }

}
