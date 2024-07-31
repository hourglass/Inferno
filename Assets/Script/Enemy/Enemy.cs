using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    private void Awake()
    {
        // static 변수이므로 1번만 플레이어를 탐색하도록 설정
        if (player == null)
        {
            // 플레이어를 탐색해 저장
            player = GameObject.FindWithTag("Player");
        }
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }


    private void OnEnable()
    {
        // 변수 초기화 함수 호출
        InitVariable();

        // 돌진 주기 코루틴 실행
        StartCoroutine(Rush());
    }


    // 변수 초기화 함수
    private void InitVariable()
    {
        // 멤버 변수 초기화
        rb = GetComponent<Rigidbody2D>();
        afterImageEffect.SetActive(false);
        rotSpeed = 500f;
        moveSpeed = 10f;
        rushSpeed = 35f;
        rushDelay = 2f;
        rushDuration = 2f;
    }


    private void FixedUpdate()
    {
        Chase();
    }


    private void Chase()
    {
        // 플레이어 null 체크 후 플레이어 위치 저장 
        var playerPos = player.transform.position;

        // 플레이어와의 거리가 0.2f보다 큰 지 확인
        if (Vector2.Distance(playerPos, rb.position) > 0.2f)
        {
            // 플레이어로 향하는 방향 벡터
            var direction = (Vector2)playerPos - rb.position;
            direction.Normalize();

            // direction = 플레이어로 향하는 방향 벡터
            // transform.up = 자신의 정면 벡터
            // 벡터의 외적 시 두 벡터와 직교하는 새로운 벡터를 구할 수 있다.
            // 2D 게임이므로 z축 밯향의 벡터를 회전축으로 삼는다.
            float rotAmount = Vector3.Cross(direction, transform.up).z;

            // angularVelocity는 회전 축의 방향에 대해서 오른손 법칙을 따른다.
            // 두 벡터의 외적의 크기는 두 벡터가 만드는 평행사변형의 넓이와 같다.
            // 회전 축 방향의 벡터 크기에 따라 회전 속도가 달라진다.
            rb.angularVelocity = -rotAmount * rotSpeed;

            // 회전 후 정면으로 이동
            rb.velocity = transform.up * moveSpeed;
        }
    }


    // 돌진 주기 코루틴
    private IEnumerator Rush()
    {
        WaitForSeconds delay = new WaitForSeconds(rushDelay);
        WaitForSeconds duration = new WaitForSeconds(rushDuration);

        while (true)
        {
            yield return delay;

            afterImageEffect.SetActive(true);
            moveSpeed += rushSpeed;

            yield return duration;

            afterImageEffect.SetActive(false);
            moveSpeed -= rushSpeed;
        }
    }


    // Member Variable //
    // 잔상 파티클 오브젝트
    [SerializeField]
    private GameObject afterImageEffect = null;

    // 추적할 플레이어 변수
    private static GameObject player = null;

    private Rigidbody2D rb;

    private float rotSpeed;  // 회전 속도
    private float moveSpeed; // 이동 속도
    private float rushSpeed; // 돌진 속도
    private float rushDelay;    // 돌진 딜레이
    private float rushDuration; // 돌진 지속시간
}

