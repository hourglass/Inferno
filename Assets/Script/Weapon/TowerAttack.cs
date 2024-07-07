using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    void Awake()
    {
        InitVariable();
        StartCoroutine(AttackRoutine());
    }


    // 변수 초기화 함수
    void InitVariable()
    {
        attackDelay = 1f;
    }


    IEnumerator AttackRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(attackDelay);

        while (true) 
        {
            yield return delay;

            Instantiate(missle, transform.position, Quaternion.identity);
        }      
    }


    //Member Variable//
    //공격 오브젝트
    [SerializeField] GameObject missle;

    //공격 주기
    protected float attackDelay;
}
