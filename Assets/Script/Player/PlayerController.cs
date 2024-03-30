using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // 잔상 파티클 오브젝트
    [SerializeField] GameObject particle = null;

    // 공격 & 스킬 함수 델리게이트
    public delegate void WeaponManagerInputKeyDel();
    public static WeaponManagerInputKeyDel AttackDel;
    public static WeaponManagerInputKeyDel SkillDel;

    Rigidbody2D rb;
    Vector2 forward;

    float moveSpeed = 25f;
    float stopDistance = 0.5f;

    bool dashState = true;
    float dashSpeed = 40f;
    float dashTime = 0.25f;
    float dashDelay = 0.25f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        particle.SetActive(false);
    }

    void Update()
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

    void FixedUpdate()
    {
        Move();
    }

    void Move()
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

    IEnumerator DashRoutine()
    {
        // 대시 코루틴
        // 일정 시간동안 이동 속도를 대시 속도만큼 증가
        moveSpeed += dashSpeed;
        particle.SetActive(true);

        // 대시 지속 시간
        yield return new WaitForSeconds(dashTime);

        // 지속 시간 이후 다시 원래 속도로 복구
        moveSpeed -= dashSpeed;
        dashState = false;

        // 대시 쿨타임
        yield return new WaitForSeconds(dashDelay);

        // 대시 쿨타임 이후 다시 대시가 사용 가능하도록 설정
        dashState = true;
        particle.SetActive(false);
    }
}
