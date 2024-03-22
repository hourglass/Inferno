using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //게임 클리어 함수
    public delegate void SceneGameClearDel();
    public static SceneGameClearDel GameClearDel;

    //HP바 생성 함수
    public delegate void EnemyUICreateHPDel(Transform enemyTm);
    public static EnemyUICreateHPDel CreateHPDel;

    //Wave UI 텍스트 함수
    public delegate void WaveUIStartTextDel();
    public static WaveUIStartTextDel StartTextDel;

    public delegate void WaveUIDelegateNumber(int number);
    public static WaveUIDelegateNumber WaveNumberDel;

    //적군 프리팹
    [SerializeField] List<Transform> enemyList = new List<Transform>();
    [SerializeField] GameObject enemy = null;

    //카메라 변수
    Camera cam = null;
    CameraController screen = null;

    //웨이브 변수
    int[] enemyCount = new int[10]; //웨이브 당 적군 수
    int waveNumber = 0; // 현재 웨이브
    int maxWave = 5;    // 최대 웨이브
    float waveDelay = 1f;    //웨이브마다의 딜레이
    float spwanDelay = 0.25f; //적 생성마다의 딜레이

    int area = 0; // 구역을 구분할 변수

    void Start()
    {
        EnemyStat.RemoveDel = RemoveEnemy;

        cam = Camera.main;
        screen = cam.gameObject.GetComponent<CameraController>();

        InitArray();
        Invoke("WaveStart", waveDelay);
    }

    void InitArray()
    {
        for (int i = 0; i < 10; i++)
        {
            enemyCount[i] = (i + 15);
        }
    }

    void WaveStart()
    {
        if (waveNumber >= maxWave)
        {
            CancelInvoke("WaveStart");
            GameClearDel();
            return;
        }

        //UI 적용
        StartTextDel();
        WaveNumberDel(waveNumber + 1);

        //적군 생성 실행
        InvokeRepeating("Spwan", 0f, spwanDelay);
    }

    void Spwan()
    {
        //모든적을 소환했는지 확인
        if (enemyCount[waveNumber] > 0)
        {
            Vector2 point = SpwanPoint();
            GameObject enemyObj = Instantiate(enemy, point, Quaternion.identity);

            enemyList.Add(enemyObj.transform);
            --enemyCount[waveNumber];

            CreateHPDel(enemyObj.transform);
        }
        else
        {
            //모든 적을 처치 했는지 확인
            if (enemyList.Count <= 0)
            {
                waveNumber++;
                CancelInvoke("Spwan");
                Invoke("WaveStart", waveDelay);
            }
        }
    }

    void RemoveEnemy(Transform tm)
    {
        enemyList.Remove(tm);
    }


    Vector2 SpwanPoint()
    {
        Vector2 camPos = cam.transform.position;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        float margin = 10f;

        float screenX = screen.getScreenX();
        float screenY = screen.getScreenY();

        float sign = 0;
        float spwanX = 0;
        float spwanY = 0;

        switch (area)
        {
            case 0:
                sign = SpwanSign(camPos.x, (camWidth + margin), screenX);

                spwanX = Random.Range(sign * camWidth, sign * (camWidth + margin));
                spwanY = Random.Range(-camHeight, camHeight) + (-Mathf.Sign(camPos.y) * margin);
                area = 1;
                break;
            case 1:
                sign = SpwanSign(camPos.y, (camHeight + margin), screenY);

                spwanX = Random.Range(-camWidth, camWidth) + (-Mathf.Sign(camPos.x) * margin);
                spwanY = Random.Range(sign * camHeight, sign * (camHeight + margin));
                area = 0;
                break;
        }
        return new Vector2(camPos.x + spwanX, camPos.y + spwanY);
    }

    float SpwanSign(float camPos, float camLength, float screenLength)
    {
        float sign = 0;

        if (Mathf.Sign(camPos) * (camPos + camLength) >= Mathf.Sign(camPos) * screenLength)
        {
            sign = Mathf.Sign(camPos) * -1;
        }
        else
        {
            sign = (Random.Range(0, 2) * 2) - 1;
        }
        return sign;
    }
}
