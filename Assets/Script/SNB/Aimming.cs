using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimming : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0;

        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);

        worldPosition = Camera.main.ViewportToWorldPoint(viewportPosition);
        worldPosition.z = 0;

        Vector2 direction = (worldPosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle-90);
    }
}
