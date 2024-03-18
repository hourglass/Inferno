using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public delegate void TowerManagerBuildDel(int id);
    public static TowerManagerBuildDel BuildDel;

    //타워 id
    [SerializeField] int id = 0;

    //지속 시간
    [SerializeField] float lifeTime = 0;

    //사정 거리
    float range = 5000f;

    //타겟 변수
    [SerializeField] LayerMask enemyMask = 0;
    Transform target = null;
    float dir = 0f;
    float rotSpeed = 5000f;


    void Start()
    {
        Invoke("SelfDestroy", lifeTime);
    }

    void SelfDestroy()
    {
        BuildDel(id);
        Destroy(gameObject);
    }

    void Update()
    {
        SearchTarget();
        LookAtTarget();
    }

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

    void LookAtTarget()
    {
        if (target != null)
        {
            float dir_x = target.position.x - transform.position.x;
            float dir_y = target.position.y - transform.position.y;
            dir = (Mathf.Atan2(dir_y, dir_x) * Mathf.Rad2Deg);
        }

        //오브젝트 회전
        Quaternion targetRotation = Quaternion.AngleAxis(dir, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }
}
