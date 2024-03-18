using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public delegate int SceneSelectGetIndexDel();
    public static SceneSelectGetIndexDel GetIndexDel;

    public delegate bool WeaponManagerCanLookAtDel();
    public static WeaponManagerCanLookAtDel CanLookAtDel;

    //무기 프리팹
    [SerializeField] List<GameObject> WeaponList = new List<GameObject>();
    [SerializeField] int weaponIndex = 0;

    //타겟 변수
    [SerializeField] LayerMask enemyMask = 0;
    Transform target = null;
    float direction = 0f;
    float rotSpeed = 100f;

    //사정거리
    float range = 5000f;

    private void Start()
    {
        WeaponManager.GetDirectionDel = getDirection;

        weaponIndex = GetIndexDel();

        GameObject weapon = Instantiate(WeaponList[weaponIndex], transform.position, Quaternion.identity);
        weapon.transform.parent = gameObject.transform;
    }

    void Update()
    {
        if (!ChoiceManager.gameIsPaused)
        {
            SearchTarget();

            if (CanLookAtDel())
            {
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

    #region LookAtTarget
    void LookAtTarget()
    {
        Vector2 playerPos = transform.parent.position;

        if (target != null)
        {
            float dir_x = target.position.x - playerPos.x;
            float dir_y = target.position.y - playerPos.y;
            direction = (Mathf.Atan2(dir_y, dir_x) * Mathf.Rad2Deg);
        }
        else
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float dir_x = mousePos.x - playerPos.x;
            float dir_y = mousePos.y - playerPos.y;
            direction = (Mathf.Atan2(dir_y, dir_x) * Mathf.Rad2Deg);
        }
        //오브젝트 회전
        Quaternion targetRotation = Quaternion.AngleAxis(direction, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }
    #endregion
}
