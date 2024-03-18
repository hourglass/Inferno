using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    //게임오버 함수
    public delegate void SceneGameOverDel();
    public static SceneGameOverDel GameOverDel;

    //선택지 함수
    public delegate void ChoiceManagerShowChoiceDel();
    public static ChoiceManagerShowChoiceDel ShowChoiceDel;

    //UI 변수
    [SerializeField] Slider healthBar = null;
    [SerializeField] Slider expBar = null;
    [SerializeField] Text LevelText = null;

    //능력치 변수
    [SerializeField] float maxHealth = 0;
    float currentHealth = 0;
    float maxExp = 100;
    float exp = 0;
    float maxLevel = 4;
    float level = 1;

    void Awake()
    {
        EnemyStat.ExpPointDel = GainExp;

        currentHealth = maxHealth;
        UISetting();
    }

    void UISetting()
    {
        healthBar.maxValue = maxHealth;
        expBar.maxValue = maxExp;
        LevelText.text = level.ToString();
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
        exp += expPoint;

        //레벨 업
        if (level < maxLevel && exp >= maxExp)
        {
            LevelUp();
        }

        //UI 적용
        expBar.value = exp;
    }

    void LevelUp()
    {
        //경험치 초기화 & 최대 경험치 증가
        exp = 0;
        maxExp += 50;
        expBar.maxValue = maxExp;

        //레벨 증가
        level += 1;
        LevelText.text = level.ToString();
        if (level >= maxLevel)
        {
            exp = maxExp;
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

    void Die()
    {
        GameOverDel();
    }
}
