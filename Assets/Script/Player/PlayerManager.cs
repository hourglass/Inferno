using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // 방향 전환이 가능한 상태인지 받아올 델리게이트
    public delegate bool WeaponManagerCanLookAtDel();
    public static WeaponManagerCanLookAtDel CanLookAtDel;

    // 무기 선택 창에서 선택된 인덱스를 받아올 델리게이트
    public delegate int SceneSelectGetIndexDel();
    public static SceneSelectGetIndexDel GetIndexDel;

    //무기 프리팹
    [SerializeField] List<GameObject> WeaponList = new List<GameObject>();
    [SerializeField] int weaponIndex = 0;

    //타겟 변수
    [SerializeField] LayerMask enemyMask = 0;
    Transform target = null;
    float direction = 0f;
    float rotSpeed = 100f;

    // 탐색 범위
    float range = 5000f;

    private void Start()
    {
        // 현재 플레이어의 방향을 넘겨주는 델리게이트
        Weapon.GetDirectionDel = getDirection;

        // 어떤 교단의 인덱스가 선택됐는지 가져오는 함수
        weaponIndex = GetIndexDel();

        // 선택된 인덱스에 해당하는 무기 생성
        GameObject weapon = Instantiate(WeaponList[weaponIndex], transform.position, Quaternion.identity);
        weapon.transform.parent = gameObject.transform;
    }

    void Update()
    {
        if (!ChoiceManager.gameIsPaused)
        {
            // 가장 가까운 적을 탐색하는 함수
            SearchTarget();

            // 방향 전환이 가능한 상태인지 확인
            if (CanLookAtDel())
            {
                // 적을 바라보는 함수
                LookAtTarget();
            }
        }
    }

    float getDirection()
    {
        return direction;
    }

    void SearchTarget()
    {
        // 원형으로 충돌체크
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);
        Transform closeTarget = null;

        if (cols.Length > 0)
        {
            // 감지 범위를 최대 거리로 설정
            float targetDistance = range;
            
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

    void LookAtTarget()
    {
        Vector2 playerPos = transform.parent.position;

        if (target != null)
        {
            // 타겟이 있을 경우 타겟을 바라봄
            float dir_x = target.position.x - playerPos.x;
            float dir_y = target.position.y - playerPos.y;
            direction = (Mathf.Atan2(dir_y, dir_x) * Mathf.Rad2Deg);
        }
        else
        {
            // 타겟이 없을 경우 마우스를 바라봄
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float dir_x = mousePos.x - playerPos.x;
            float dir_y = mousePos.y - playerPos.y;
            direction = (Mathf.Atan2(dir_y, dir_x) * Mathf.Rad2Deg);
        }

        //오브젝트 회전
        Quaternion targetRotation = Quaternion.AngleAxis(direction, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }
}
