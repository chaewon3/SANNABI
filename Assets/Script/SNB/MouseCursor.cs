using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    #region public º¯¼ö
    public RectTransform transform_cursor;
    #endregion

    void Start()
    {
        init_Cursor();   
    }
    
    void Update()
    {
        NewCursor();
    }

    void init_Cursor()
    {
        Cursor.visible = false;
    }

    void NewCursor()
    {
        Vector2 mousePos = Input.mousePosition;
        transform_cursor.position = mousePos;
    }

}
