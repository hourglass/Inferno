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

    //Wave UI 현재 웨이브 숫자 함수
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

        // 적군을 생성할 위치정보를 위한 스크린 정보
        cam = Camera.main;
        screen = cam.gameObject.GetComponent<CameraController>();

        InitArray();
        Invoke("WaveStart", waveDelay);
    }


    // 웨이브 당 생성할 적군 수를 저장하는 함수
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


    // 적군 관리 리스트에서 적군을 제거하는 함수
    void RemoveEnemy(Transform tm)
    {
        enemyList.Remove(tm);
    }


    void Spwan()
    {
        //모든적을 소환했는지 확인
        if (enemyCount[waveNumber] > 0)
        {
            // 소환 지점 가져오기
            Vector2 point = SpwanPoint();
            
            // 적군 생성
            GameObject enemyObj = Instantiate(enemy, point, Quaternion.identity);

            // 적군 관리 리스트에 추가
            enemyList.Add(enemyObj.transform);
            --enemyCount[waveNumber];

            // 체력바 생성 델리게이트 수행
            CreateHPDel(enemyObj.transform);
        }
        else
        {
            //모든 적을 처치 했는지 확인
            if (enemyList.Count <= 0)
            {
                // 웨이브 숫자 증가
                waveNumber++;

                // 적군 생성 중지
                CancelInvoke("Spwan");
                Invoke("WaveStart", waveDelay);
            }
        }
    }

    Vector2 SpwanPoint()
    {
        // 카메라의 위치 가져오기
        Vector2 camPos = cam.transform.position;

        // 화면 가운데부터 상하 한쪽 방향의 길이
        float camHeight = cam.orthographicSize;

        // cam.aspect(종횡비) = 너비 / 높이 
        // cam.aspect * 높이 = 너비
        float camWidth = camHeight * cam.aspect; 

        // 화면의 추가 여유 공간 크기
        float margin = 10f;

        // 적군 생성 시 x,y 거리
        float spwanX = 0;
        float spwanY = 0;

        // -1 or 1 랜덤 생성
        float sign = (Random.Range(0, 2) * 2) - 1;

        // area 변수가 0과 1를 번갈아 가며 카메라 바깥 상하좌우로 적군 생성
        switch (area)
        {
            case 0:          
                spwanX = Random.Range(sign * camWidth, sign * (camWidth + margin));
                spwanY = Random.Range(-camHeight, camHeight) + (-Mathf.Sign(camPos.y) * margin);
                area = 1;
                break;
            case 1:
                spwanX = Random.Range(-camWidth, camWidth) + (-Mathf.Sign(camPos.x) * margin);
                spwanY = Random.Range(sign * camHeight, sign * (camHeight + margin));
                area = 0;
                break;
        }
        return new Vector2(camPos.x + spwanX, camPos.y + spwanY);
    }
}
