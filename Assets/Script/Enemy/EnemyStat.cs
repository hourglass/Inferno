using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    //리스트 제거 함수
    public delegate void EnemyStatRemoveDel(Transform tm);
    public static EnemyStatRemoveDel RemoveDel;

    //경험치 증가 함수
    public delegate void PlayerStatExpPointDel(float expPoint);
    public static PlayerStatExpPointDel ExpPointDel;

    //UI 체력 감소 함수
    public delegate void EnemyUIHpDel(Transform enemyTm, float health);
    public static EnemyUIHpDel HpDel;

    //처치 애니메이션
    [SerializeField] GameObject dieEffect = null;

    //능력치 변수
    [SerializeField] float maxHealth = 0;
    [SerializeField] float bodyDamage = 0;
    [SerializeField] float expPoint = 0;

    private float currentHealth;

    void Awake()
    {
        EnemyUI.MaxHeathDel = getMaxHealth;

        currentHealth = maxHealth;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어에 충돌 시 피해를 주는 함수 실행
        if (other.tag == "Player")
        {
            GiveToDamage(other.gameObject);
        }

        // 무기 타입에 충돌 시 피해를 받는 함수 실행
        if (other.tag == "Weapon")
        {
            // 대미지 큐에 있는 모든 피해를 받는 함수 실행
            TakeAllDamage();
        }
    }

    void GiveToDamage(GameObject obj)
    {
        obj.GetComponent<PlayerStat>().TakeDamage(getBodyDamage());
    }

    void TakeAllDamage()
    {
        if (WeaponStat.damageQueue.Count > 0)
        {
            for (int i = 0; i < WeaponStat.damageQueue.Count; i++)
            {
                if (currentHealth <= 0)
                {
                    WeaponStat.damageQueue.Clear();
                    break;
                }
                TakeDamage(WeaponStat.damageQueue.Dequeue());
            }
        }
    }

    void TakeDamage(float damage)
    {
        //체력 감소
        currentHealth -= damage;
        Mathf.Clamp(currentHealth, 0f, maxHealth);

        //UI 적용
        HpDel(transform, currentHealth);

        if (currentHealth <= 0)
        {
            //사망
            Die();
        }
    }

    void Die()
    {
        Instantiate(dieEffect, transform.position, Quaternion.identity);

        ExpPointDel(expPoint);
        RemoveDel(transform);
        Destroy(gameObject);
    }

    float getMaxHealth()
    {
        return maxHealth;
    }

    float getBodyDamage()
    {
        return bodyDamage;
    }
}
