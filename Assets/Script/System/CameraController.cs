using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;

    float ScreenX = 92f;
    float ScreenY = 52f;

    float currentX = 0;
    float currentY = 0;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        Vector3 playerPos = player.transform.position;

        if (playerPos.x > -ScreenX && playerPos.x < ScreenX)
        {
            currentX = playerPos.x;
        }

        if (playerPos.y > -ScreenY && playerPos.y < ScreenY)
        {
            currentY = playerPos.y;
        }

        transform.position = new Vector3(currentX, currentY, transform.position.z);
    }
}
