using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private void Awake()
    {
        InitVariable();
        Destroy(gameObject, lifeTime);
    }


    // 변수 초기화 함수
    void InitVariable()
    {
        range = 2500f;
        rotSpeed = 2500f;
    }


    void Update()
    {
        SearchTarget();
        LookAtTarget();
    }


    private void OnDestroy()
    {
        BuildDel(id);
    }


    // 적군 탐지 함수
    void SearchTarget()
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
    void LookAtTarget()
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


    //Delegate//
    public delegate void TowerManagerBuildDel(int id);
    public static TowerManagerBuildDel BuildDel;

    //Member Variable//
    //타워 id
    [SerializeField] int id;

    //지속 시간
    [SerializeField] float lifeTime;

    // 레이어 마스크
    [SerializeField] LayerMask enemyMask;

    // 타겟 변수
    Transform target;
    float direction;
    float range;
    float rotSpeed;
}
