using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    // 잔상 파티클 오브젝트
    [SerializeField] GameObject afterImageEffect = null;

    // 추적할 플레이어 변수
    static GameObject player = null;

    Rigidbody2D rb;     

    float rotSpeed = 500f; // 회전 속도
    float moveSpeed = 10f; // 이동 속도
    float dashSpeed = 40f; // 대시 속도

    float attackDuration = 2f; // 공격 지속시간
    float attackDelay = 2f;    // 공격 딜레이

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        afterImageEffect.SetActive(false);

        // static 변수이므로 1번만 플레이어를 탐색하도록 설정
        if (player == null)
        {
            // 플레이어를 탐색해 저장
            player = GameObject.FindWithTag("Player");
        }
    }

    void Start()
    {
        InvokeRepeating("ChasePlayer", 0f, 0.2f); // 플레이어 추적 함수 실행
        StartCoroutine(Attack());                 // 플레이어 공격 함수 실행
    }

    // 플레이어 추적 함수
    void ChasePlayer()
    {
        if (player != null)
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
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            // 플레이어가 없어지면 추적 중지
            CancelInvoke("ChasePlayer");
        }
    }

    // 공격 주기 코루틴
    IEnumerator Attack()
    {
        WaitForSeconds duration = new WaitForSeconds(attackDuration);
        WaitForSeconds delay = new WaitForSeconds(attackDelay);

        while (true)
        {
            yield return delay;

            afterImageEffect.SetActive(true);
            moveSpeed += dashSpeed;

            yield return duration;

            afterImageEffect.SetActive(false);
            moveSpeed -= dashSpeed;
        }
    }
}
