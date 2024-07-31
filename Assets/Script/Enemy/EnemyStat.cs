using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    private void Awake()
    {
        // UI의 체력바 설정 델리게이트
        EnemyUI.MaxHeathDel = getMaxHealth;
    }


    private void OnDestroy()
    {
        EnemyUI.MaxHeathDel -= getMaxHealth;
    }


    private void OnEnable()
    {
        InitVariable();
    }


    // 변수 초기화 함수
    private void InitVariable()
    {
        // 멤버 변수 초기화   
        currentHealth = maxHealth;
        isDead = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어에 충돌 시 피해를 주는 함수 실행
        if (other.tag == "Player")
        {
            GiveDamage(other.gameObject);
        }
    }


    // 대미지 전달 함수
    private void GiveDamage(GameObject obj)
    {
        var playerStat = obj.GetComponent<PlayerStat>();
        if (playerStat != null) 
        {
            playerStat.TakeDamage(getBodyDamage());
        }
    }


    private float getMaxHealth()
    {
        return maxHealth;
    }


    private float getBodyDamage()
    {
        return bodyDamage;
    }


    // 적군 사망 함수
    private void Die()
    {
        Instantiate(dieEffect, transform.position, Quaternion.identity);
        ExpPointDel(expPoint);
        RemoveDel();

        isDead = true;
        ObjectPoolManager.instance.Despawn(gameObject);
    }


    // 대미지 처리 함수
    public void TakeDamage(float damage)
    {
        if(isDead) { return; }

        //체력 감소
        currentHealth -= damage;
        Mathf.Clamp(currentHealth, 0f, maxHealth);

        //UI 적용
        DecreaseHpDel(gameObject, currentHealth);

        if (currentHealth <= 0)
        {
            //사망
            Die();
        }
    }


    // Delegate //
    // 리스트 제거 함수
    public delegate void EnemyStatRemoveDel();
    public static EnemyStatRemoveDel RemoveDel;

    // 경험치 증가 함수
    public delegate void PlayerStatExpPointDel(float expPoint);
    public static PlayerStatExpPointDel ExpPointDel;

    // UI 체력 감소 함수
    public delegate void EnemyUIHpDel(GameObject enemy, float health);
    public static EnemyUIHpDel DecreaseHpDel;


    // Member Variable //
    // 처치 애니메이션
    [SerializeField]
    private GameObject dieEffect;

    // 능력치 변수 //
    [SerializeField]
    private float maxHealth;
    
    [SerializeField]
    private float bodyDamage;
    
    [SerializeField]
    private float expPoint;
    private float currentHealth;
    private bool isDead;
}
