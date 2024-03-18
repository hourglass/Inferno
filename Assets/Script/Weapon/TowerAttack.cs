using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    //공격 오브젝트
    [SerializeField] GameObject missle = null;

    //공격 주기
    protected float attackDelay = 1f;

    void Start()
    {
        InvokeRepeating("AttackRoutine", 0, attackDelay);
    }

    void AttackRoutine()
    {
        Instantiate(missle, transform.position, Quaternion.identity);
    }
}
