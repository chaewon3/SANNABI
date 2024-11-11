using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public GameObject SNB;
    public GameObject menu;
    public GameObject[] UI;
    public AudioSource audioManager;

    bool MenuOpen = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!MenuOpen)
            {
                audioManager.volume = 0.15f;
                Time.timeScale = 0;
                menu.SetActive(true);
                SNB.GetComponent<RayCast>().enabled = false;
                GetComponent<SetResolution>().enabled = false;
                Cursor.visible = true;
                MenuOpen = true;
            }
            else
            {
                audioManager.volume = 0.5f;
                MenuOpen = false;
                Time.timeScale = 1;
                menu.SetActive(false);
                SNB.GetComponent<RayCast>().enabled = true;
                Cursor.visible = false;
                GetComponent<SetResolution>().enabled = true;

            }
        }
    }

    public void CheckPoint()
    {
        UI[0].SetActive(false);
        UI[2].SetActive(true);
    }

    public void GotoMAin()
    {
        UI[0].SetActive(false);
        UI[3].SetActive(true);
    }

    public void GameExit()
    {
        UI[0].SetActive(false);
        UI[4].SetActive(true);
    }

    public void Yescheck()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Chapter2");
    }

    public void YesMain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }
    public void YesExit()
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
        UI[2].SetActive(false);
        UI[3].SetActive(false);
        UI[4].SetActive(false);
    }
}
