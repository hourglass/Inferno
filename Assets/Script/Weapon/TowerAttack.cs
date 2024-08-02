using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    private void OnEnable()
    {
        InitVariable();
        StartCoroutine(AttackRoutine());
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }


    // 변수 초기화 함수
    private void InitVariable()
    {
        attackDelay = 1f;
    }


    private IEnumerator AttackRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(attackDelay);

        while (true) 
        {
            yield return delay;

            ObjectPoolManager.instance.Spawn("Tower_Bullet");
        }      
    }


    // Member Variable //
    // 공격 주기
    private float attackDelay;
}
