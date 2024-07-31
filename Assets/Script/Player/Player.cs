using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private void Awake()
    {
        InitVariable();
    }


    // 변수 초기화 함수
    private void InitVariable()
    {
        // 멤버 변수 초기화
        rb = GetComponent<Rigidbody2D>();
        afterImage.SetActive(false);

        moveSpeed = 25f;        // 이동 속도
        stopDistance = 0.5f;    // 정지 거리
        dashSpeed = 40f;        // 대쉬 속도
        dashTime = 0.25f;       // 대쉬 시간
        dashDelay = 0.25f;      // 대쉬 쿨타임
        dashState = true;       // 대쉬 가능 여부
    }


    private void Update()
    {
        if (!ChoiceManager.gameIsPaused)
        {
            // 마우스 좌클릭 시 공격 함수 실행
            if (Input.GetMouseButtonDown(0))
            {
                AttackDel();
            }

            // 마우스 휠클릭 시 스킬 함수 실행
            if (Input.GetMouseButtonDown(2))
            {
                SkillDel();
            }

            //  마우스 우클릭 시 대쉬 함수 실행
            if (Input.GetMouseButtonDown(1))
            {
                if (dashState)
                {
                    StartCoroutine(DashRoutine());
                }
            }
        }
    }


    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        // 마우스 커서의 위치를 받아오기
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;

        // 마우스 방향으로 향하는 벡터 구하기
        forward = mousePos - playerPos;
        forward.Normalize();

        if (Vector2.Distance(mousePos, playerPos) > stopDistance)
        {
            // 마우스 방향으로 이동
            rb.velocity = forward * (moveSpeed);
        }
        else
        {
            // 마우스가 플레이어와 일정 거리만큼 가까워지면 정지
            rb.velocity = Vector2.zero;
        }
    }


    private IEnumerator DashRoutine()
    {
        // 대쉬 코루틴
        // 일정 시간동안 이동 속도를 대쉬 속도만큼 증가
        moveSpeed += dashSpeed;
        afterImage.SetActive(true);

        // 대쉬 지속 시간
        yield return new WaitForSeconds(dashTime);

        // 지속 시간 이후 다시 원래 속도로 복구
        moveSpeed -= dashSpeed;
        dashState = false;

        // 대쉬 쿨타임
        yield return new WaitForSeconds(dashDelay);

        // 대쉬 쿨타임 이후 다시 대쉬가 사용 가능하도록 설정
        dashState = true;
        afterImage.SetActive(false);
    }


    // Delegate //
    // 공격 & 스킬 함수 델리게이트
    public delegate void WeaponManagerInputKeyDel();
    public static WeaponManagerInputKeyDel AttackDel;
    public static WeaponManagerInputKeyDel SkillDel;

    // Member Variable //
    // 잔상 파티클 오브젝트
    [SerializeField]
    private GameObject afterImage;

    private Rigidbody2D rb;
    private Vector2 forward;

    private float moveSpeed;    // 이동 속도
    private float stopDistance; // 정지 거리
    private float dashSpeed;    // 대쉬 속도
    private float dashTime;     // 대쉬 시간
    private float dashDelay;    // 대쉬 쿨타임
    private bool dashState;     // 대쉬 가능 여부
}