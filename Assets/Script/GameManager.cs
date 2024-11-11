using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Texture2D cursor;

    public Vector3 checkpoint = new Vector3(-1, -3, 0);
    public bool[] check = new bool[5];

    public string scenename;

    void Awake()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveCheckPoint(Vector3 position, int point)
    {
        checkpoint = position+Vector3.up*2.5f;
        check[point] = true;
    }    

    public Vector3 LoadCheckPoint()
    {
        return checkpoint;
    }
    public void LoadScene(string name)
    {
        scenename = name;
        SceneManager.LoadScene("Loading");
    }
}
