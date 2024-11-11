using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Loading : MonoBehaviour
{
    string scenename;

    private void Awake()
    {
        scenename = GameManager.Instance.scenename;
        StartCoroutine(LoadScene(scenename));
    }

    IEnumerator LoadScene(string name)
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(name);
        op.allowSceneActivation = false;
        yield return new WaitForSecondsRealtime(3f);
        op.allowSceneActivation = true;
    }
}
