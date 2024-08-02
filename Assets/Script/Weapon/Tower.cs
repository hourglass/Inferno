using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum TowerType
    { 
        Bullet,
        Laser,
    }

    private void OnEnable()
    {
        InitVariable();
    }


    private void OnDisable()
    {
        ChargeCountDel(type);
    }


    // 변수 초기화 함수
    private void InitVariable()
    {
        range = 2500f;
        rotSpeed = 2500f;
    }


    private void FixedUpdate()
    {
        SearchTarget();
        LookAtTarget();
    }

    // 적군 탐지 함수
    private void SearchTarget()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);
        Transform closeTarget = null;

        if (cols.Length > 0)
        {
            float targetDistance = range;
            foreach (Collider2D colTarget in cols)
            {
                float distance = Vector3.SqrMagnitude(transform.position - colTarget.transform.position);
                if (targetDistance > distance)
                {
                    targetDistance = distance;
                    closeTarget = colTarget.transform;
                }
            }
            target = closeTarget;
        }
    }


    // 방향 전환 함수
    private void LookAtTarget()
    {
        if (target != null)
        {
            float dir_x = target.position.x - transform.position.x;
            float dir_y = target.position.y - transform.position.y;
            direction = (Mathf.Atan2(dir_y, dir_x) * Mathf.Rad2Deg);
        }

        //오브젝트 회전
        Quaternion targetRotation = Quaternion.AngleAxis(direction, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }


    // Delegate //
    public delegate void WeaponTowerBuildDel(TowerType type);
    public static WeaponTowerBuildDel ChargeCountDel;

    // Member Variable //
    // 레이어 마스크
    [SerializeField]
    private LayerMask enemyMask;

    // 타워 id
    [SerializeField]
    private TowerType type;

    // 타겟 변수
    private Transform target;
    private float direction;
    private float range;
    private float rotSpeed;
}
