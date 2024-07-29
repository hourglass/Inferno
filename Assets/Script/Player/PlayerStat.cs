using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    private void Awake()
    {
        InitVariable();
        UISetting();
    }


    private void OnDestroy()
    {
        EnemyStat.ExpPointDel -= GainExp;
    }


    // 변수 초기화 함수
    private void InitVariable()
    {
        // 경험치를 받아올 델리게이트
        EnemyStat.ExpPointDel = GainExp;

        // 멤버 변수 초기화
        maxHealth = 100;
        currentHealth = maxHealth;
        maxExp = 100;
        currentexp = 0;
        maxLevel = 4;
        currentlevel = 1;
    }


    private void UISetting()
    {
        healthBar.maxValue = maxHealth;
        expBar.maxValue = maxExp;
        LevelText.text = currentlevel.ToString();
    }

    
    private void LevelUp()
    {
        //경험치 초기화 & 최대 경험치 증가
        currentexp = 0;
        maxExp += 50;
        expBar.maxValue = maxExp;

        //레벨 증가
        currentlevel += 1;
        LevelText.text = currentlevel.ToString();
        if (currentlevel >= maxLevel)
        {
            currentexp = maxExp;
            expBar.value = maxExp;
        }

        //체력 회복
        currentHealth += maxHealth * 0.25f;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthBar.value = currentHealth;

        //선택지 호출
        ShowChoiceDel();
    }


    private void Die()
    {
        GameOverDel();
    }


    public void TakeDamage(float damage)
    {
        //현재 체력 감소
        currentHealth -= damage;

        //UI 적용
        healthBar.value = currentHealth;

        //사망
        if (currentHealth <= 0)
        {
            Die();
        }
    }


    public void GainExp(float expPoint)
    {
        //경험치 증가
        currentexp += expPoint;

        //레벨 업
        if (currentlevel < maxLevel && currentexp >= maxExp)
        {
            LevelUp();
        }

        //UI 적용
        expBar.value = currentexp;
    }


    // Member Variable //
    // 게임 오버 함수
    public delegate void SceneGameOverDel();
    public static SceneGameOverDel GameOverDel;

    // 선택지 함수
    public delegate void ChoiceManagerShowChoiceDel();
    public static ChoiceManagerShowChoiceDel ShowChoiceDel;

    // UI 변수
    [SerializeField]
    private Slider healthBar = null;

    [SerializeField]
    private Slider expBar = null;

    [SerializeField]
    private Text LevelText = null;

    // 능력치 변수 //
    [SerializeField]
    private float maxHealth;

    private float currentHealth;
    private float maxExp;
    private float currentexp;
    private float maxLevel;
    private float currentlevel;
}
