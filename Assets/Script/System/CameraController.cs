using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Awake()
    {
        InitVariable();
    }


    // 변수 초기화 함수
    private void InitVariable()
    {
        player = GameObject.Find("Player");

        ScreenX = 92f;
        ScreenY = 52f;
        currentX = 0;
        currentY = 0;
    }


    private void Update()
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


    // Member Variable //
    private GameObject player;

    private float ScreenX;
    private float ScreenY;
    private float currentX;
    private float currentY;
}
