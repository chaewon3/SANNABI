using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public GameObject ghost;

    SpriteRenderer Sprite;

    bool isShadow = false;

    void Awake()
    {
        Sprite = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        if(isShadow)
        {
            InvokeRepeating("MakeShadow", 0, 0.03f);
            Invoke("CancleShadow", 0.3f);

            isShadow = false;
        }
    }

    public void ShadowOn()
    {
        isShadow = true;
    }

    void MakeShadow()
    {
        GameObject currentGhost = Instantiate(ghost, transform.position, this.transform.rotation);
        Sprite currentSprite = Sprite.sprite;
        currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
        Destroy(currentGhost, 0.3f);
    }

    void CancleShadow()
    {
        CancelInvoke("MakeShadow");
    }
}
