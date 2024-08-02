using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private void Awake()
    {
        InitVariable();
        StartCoroutine(WaveStart());
    }


    private void OnDestroy()
    {
        EnemyStat.RemoveDel -= RemoveEnemy;
    }


    // 변수 초기화 함수
    private void InitVariable()
    {
        // 적 개체 제거 델리게이트
        EnemyStat.RemoveDel = RemoveEnemy;

        // 적군을 생성할 위치정보를 위한 스크린 정보
        cam = Camera.main;

        // 멤버 변수 초기화
        maxWave = 5;
        waveNumber = 0;
        killCount = 0;
        waveStartDelay = 1.5f;
        spwanDelay = 0.5f;

        // 웨이브 당 생성할 적군 개수 세팅
        for (int i = 0; i < maxWave; i++)
        {
            enemyCount[i] = (i + 15);
        }
    }


    // 웨이브 시작 함수
    private IEnumerator WaveStart()
    {
        WaitForSeconds delay = new WaitForSeconds(waveStartDelay);

        if (waveNumber >= maxWave)
        {
            GameClearDel();
        }
        else
        {
            yield return delay;

            // 웨이브 텍스트 적용
            StartTextDel();
            WaveNumberDel(waveNumber + 1);

            // 클리어 변수 초기화
            waveClear = false;

            //적군 생성 실행
            StartCoroutine(EnemySpwan());
        }
    }


    // 적군 생성 함수
    private IEnumerator EnemySpwan()
    {
        WaitForSeconds delay = new WaitForSeconds(spwanDelay);

        for (int i = 0; i < enemyCount[waveNumber]; ++i)
        {
            // 소환 지점 가져오기
            Vector2 point = SpwanPoint();

            // 적군 생성
            GameObject enemyObj = ObjectPoolManager.instance.Spawn("Enemy_Imp", point, Quaternion.identity);

            // 체력바 생성 델리게이트 수행
            CreateHPDel(enemyObj);

            yield return delay;
        }
    }


    // 적군 관리 리스트에서 적군을 제거하는 함수
    private void RemoveEnemy()
    {
        if (waveClear) { return; }

        killCount++;

        if (killCount >= enemyCount[waveNumber])
        {
            waveClear = true;

            // 처치 수 초기화
            killCount = 0;

            // 웨이브 숫자 증가
            waveNumber++;

            // 다음 웨이브 시작
            StartCoroutine(WaveStart());
        }
    }


    // 적군 생성 위치 설정 함수
    private Vector2 SpwanPoint()
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


    // Delegate //
    // 게임 클리어 함수
    public delegate void SceneGameClearDel();
    public static SceneGameClearDel GameClearDel;

    //HP바 생성 함수
    public delegate void EnemyUICreateHPDel(GameObject enemy);
    public static EnemyUICreateHPDel CreateHPDel;

    //Wave UI 텍스트 함수
    public delegate void WaveUIStartTextDel();
    public static WaveUIStartTextDel StartTextDel;

    //Wave UI 현재 웨이브 숫자 함수
    public delegate void WaveUIDelegateNumber(int number);
    public static WaveUIDelegateNumber WaveNumberDel;


    // Member Variable //
    // 카메라 변수
    private Camera cam = null;

    // 구역 구분 변수
    private int area = 0;

    // 웨이브 변수 //
    private int[] enemyCount = new int[10]; // 웨이브 당 적군 수
    private int killCount;  // 처치한 적군 수
    private int maxWave;    // 최대 웨이브
    private int waveNumber; // 현재 웨이브
    private bool waveClear; // 클리어 확인 변수 
    private float waveStartDelay;  // 웨이브 시작 시 딜레이
    private float spwanDelay;      // 적 생성마다의 딜레이
}
