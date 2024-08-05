using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private void Awake()
    {
        InitVariable();
    }


    private void OnDestroy()
    {
        Weapon.GetDirectionDel -= GetDirection;
    }


    // 변수 초기화 함수
    private void InitVariable()
    {
        // 현재 플레이어의 방향을 넘겨주는 델리게이트
        Weapon.GetDirectionDel = GetDirection;

        // 멤버 변수 초기화
        rb = GetComponent<Rigidbody2D>();
        afterImage.SetActive(false);

        moveSpeed = 25f;        // 이동 속도
        stopDistance = 5f;    // 정지 거리
        dashSpeed = 40f;        // 대쉬 속도
        dashTime = 0.25f;       // 대쉬 시간
        dashDelay = 0.25f;      // 대쉬 쿨타임

        dashEnabled = true;     // 대쉬 가능 여부
        attackEnabled = true;   // 공격 가능 여부
        skillEnabled = true;    // 스킬 가능 여부

        direction = 0f;
        rotSpeed = 100f;
        searchRange = 5000f;
    }


    private void Update()
    {
        if (!ChoiceManager.gameIsPaused)
        {
            // 마우스 좌클릭 시 공격 함수 실행
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }

            //  마우스 우클릭 시 대쉬 함수 실행
            if (Input.GetMouseButtonDown(1))
            {
                Dash();
            }

            // 마우스 휠클릭 시 스킬 함수 실행
            if (Input.GetMouseButtonDown(2))
            {
                Skill();
            }
        }
    }


    private void FixedUpdate()
    {
        Move();
        SearchTarget();
        LookAtTarget();
    }


    private void Dash()
    {
        if (!dashEnabled) { return; }
        StartCoroutine(DashRoutine());
    }


    private void Attack()
    {
        if(!attackEnabled) { return; }

        if(currentWeapon == null) { return; }

        StartCoroutine(AttackRoutine());
    }


    private void Skill()
    { 
        if(!skillEnabled) { return; }

        if (currentWeapon == null) { return; }

        StartCoroutine(SkillRoutine());
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
        dashEnabled = false;

        // 대쉬 쿨타임
        yield return new WaitForSeconds(dashDelay);

        // 대쉬 쿨타임 이후 다시 대쉬가 사용 가능하도록 설정
        dashEnabled = true;
        afterImage.SetActive(false);
    }


    // 공격 코루틴
    private IEnumerator AttackRoutine()
    {
        attackEnabled = false;
        AttackDel();   
        yield return new WaitForSeconds(currentWeapon.attackDelay);
        attackEnabled = true;
    }


    // 스킬 코루틴
    private IEnumerator SkillRoutine()
    {
        skillEnabled = false;
        SkillDel();    
        yield return new WaitForSeconds(currentWeapon.attackDelay);
        skillEnabled = true;
    }


    private void Move()
    {
        // 마우스 커서의 위치를 받아오기
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = rb.position;

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


    // 적군 탐색 함수
    private void SearchTarget()
    {
        // 원형으로 충돌체크
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, searchRange, enemyMask);
        Transform closeTarget = null;

        if (cols.Length > 0)
        {
            // 감지 범위를 최대 거리로 설정
            float targetDistance = searchRange;

            // 배열을 순회하며 가장 가까운 적을 탐색
            foreach (Collider2D colTarget in cols)
            {
                float distance = Vector3.SqrMagnitude(transform.position - colTarget.transform.position);
                if (targetDistance > distance)
                {
                    targetDistance = distance;
                    closeTarget = colTarget.transform;
                }
            }

            // 탐색한 적을 저장
            target = closeTarget;
        }
    }


    // 방향 전환 함수
    private void LookAtTarget()
    {
        if (!attackEnabled) { return; }

        Transform LookAtTm = LookAtObject.transform;

        if (target != null)
        {
            // 타겟이 있을 경우 타겟을 바라봄
            float dir_x = target.position.x - LookAtTm.position.x;
            float dir_y = target.position.y - LookAtTm.position.y;
            direction = (Mathf.Atan2(dir_y, dir_x) * Mathf.Rad2Deg);
        }
        else
        {
            // 타겟이 없을 경우 마우스를 바라봄
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float dir_x = mousePos.x - LookAtTm.position.x;
            float dir_y = mousePos.y - LookAtTm.position.y;
            direction = (Mathf.Atan2(dir_y, dir_x) * Mathf.Rad2Deg);
        }

        //오브젝트 회전
        Quaternion targetRotation = Quaternion.AngleAxis(direction, Vector3.forward);
        LookAtTm.rotation = Quaternion.Slerp(LookAtTm.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }

    public void SetCurrentWeapon(Weapon weapon)
    { 
        currentWeapon = weapon;
        currentWeapon.transform.SetParent(LookAtObject.transform);
    }

    public float GetDirection()
    {
        return direction;
    }

    // Delegate //
    // 공격 & 스킬 함수 델리게이트
    public delegate void WeaponManagerInputKeyDel();
    public static WeaponManagerInputKeyDel AttackDel;
    public static WeaponManagerInputKeyDel SkillDel;


    // Member Variable //
    // 플레이어 컨트롤러
    [SerializeField]
    private GameObject LookAtObject;

    // 잔상 파티클 오브젝트
    [SerializeField]
    private GameObject afterImage;

    private Rigidbody2D rb;
    private Vector2 forward;

    // 이동 관련 변수 //
    private float moveSpeed;
    private float stopDistance;
    private float dashSpeed;
    private float dashTime;
    private float dashDelay;

    // 동작 가능 여부 //
    private bool dashEnabled;
    private bool attackEnabled;
    private bool skillEnabled;

    // 적군 탐색 변수 //
    [SerializeField]
    private LayerMask enemyMask;

    private Transform target;
    private float direction;
    private float rotSpeed;
    private float searchRange;

    // 무기 클래스
    private Weapon currentWeapon;
}