using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class WeaponTower : Weapon
{
    void Awake()
    {
        // 변수 초기화 함수
        InitVariable();

        // 마우스 오브젝트 캐싱
        point = GameObject.Find("point");
    }


    // 변수 초기화 함수
    void InitVariable()
    {
        // 델리게이트 등록
        Tower.BuildDel += ChargeBuild;
        towerAudio.clip = buildSound;

        maxAttackCount = 3;
        currAttackCount = 0;
        maxSkillCount = 1;
        currSkillCount = 0;
    }


    protected override void InitWeapon()
    {
        attackDel += delegate { DefaultAttack(); };
        skillDel += delegate { DefaultSkill(); };
    }


    protected override void InitChoiceDatas()
    {
        InputChoiceInfos("공격 구조물 2개 추가");
        InputChoiceDatas(ActionType.Attack,
                         ExecutionType.Immediate,
                         delegate { increaseAttackCount(); });

        InputChoiceInfos("스킬 구조물 1개 추가");
        InputChoiceDatas(ActionType.Skill,
                         ExecutionType.Immediate,
                         delegate { increaseSkillCount(); });

        InputChoiceInfos("주기마다 유도화살 4개 발사");
        InputChoiceDatas(ActionType.Passive,
                         ExecutionType.AddChain,
                         delegate { HomingArrowPassive(); }, 2f);

        InputChoiceInfos("주기마다 자폭병 소환");
        InputChoiceDatas(ActionType.Passive,
                         ExecutionType.AddChain,
                         delegate { SummonBomb(); }, 2f);

        InputChoiceInfos("주기마다 여러 방향의 화살 발사");
        InputChoiceDatas(ActionType.Passive,
                         ExecutionType.AddChain,
                         delegate { ManyDirectionsArrow(); }, 4f);
    }


    // 기본 공격 함수
    void DefaultAttack()
    {
        if (currAttackCount < maxAttackCount)
        {
            towerAudio.Play();
            Instantiate(attackObj, point.transform.position, Quaternion.identity);
            currAttackCount++;
        }
    }


    // 기본 스킬 함수
    void DefaultSkill()
    {
        if (currSkillCount < maxSkillCount)
        {
            Instantiate(skillObj, point.transform.position, Quaternion.identity);
            currSkillCount++;
        }
    }


    // 공격 설치 횟수 증가 함수
    void increaseAttackCount()
    {
        int value = 2;
        maxAttackCount = increaseBuildCount(maxAttackCount, value);
    }


    // 스킬 설치 횟수 증가 함수
    void increaseSkillCount()
    {
        int value = 1;
        maxSkillCount = increaseBuildCount(maxSkillCount, value);
    }


    // 건축물 설치 횟수 증가 함수
    int increaseBuildCount(int count, int value)
    {
        count += value;
        return count;
    }


    // 유도 화살 생성 함수
    void HomingArrowPassive()
    {
        Create(homingObj, transform, 90);
        Create(homingObj, transform, -90);
        Create(homingObj, transform, 135);
        Create(homingObj, transform, -135);
    }


    // 자폭병 생성 함수
    void SummonBomb()
    {
        Create(bombObj, transform, 0);
    }


    // 16 방향 화살 생성 함수
    void ManyDirectionsArrow()
    {
        int direction = 16;
        float degree = 360f / direction;

        for (int i = 0; i < direction; i++)
        {
            Create(arrowObj, transform, i * degree);
        }
    }


    // 건축물 설치 횟수 충전 함수
    void ChargeBuild(int id)
    {
        switch (id)
        {
            case 0: currAttackCount--; break;
            case 1: currSkillCount--; break;
        }
    }


    //Member Variable//
    [SerializeField] GameObject attackObj;
    [SerializeField] GameObject skillObj;

    [SerializeField] GameObject homingObj;
    [SerializeField] GameObject bombObj;
    [SerializeField] GameObject arrowObj;

    [SerializeField] AudioSource towerAudio;
    [SerializeField] AudioClip buildSound;

    GameObject point;

    int maxAttackCount;
    int currAttackCount;
    int maxSkillCount;
    int currSkillCount;
}
