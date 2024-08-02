using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static Tower;

public class WeaponTower : Weapon
{
    private void Awake()
    {
        // 마우스 오브젝트 캐싱
        point = GameObject.Find("point");
    }


    private void OnEnable()
    {
        // 변수 초기화 함수
        InitVariable();
    }


    // 변수 초기화 함수
    private void InitVariable()
    {
        // 델리게이트 등록
        Tower.ChargeCountDel = ChargeCount;

        // 오디오 클립 설정
        towerAudio.clip = buildSound;

        // 타워 관련 변수 설정
        maxAttackCount = 3;
        attackCount = 0;
        maxSkillCount = 1;
        skillCount = 0;
    }


    protected override void InitWeapon()
    {
        attackDel += delegate { DefaultAttack(); };
        skillDel += delegate { DefaultSkill(); };

        // 기본 공격 & 스킬 키 등록
        currentAttackKey = "Tower_Attack";
        currentSkillKey = "Tower_Skill";
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
    private void DefaultAttack()
    {
        if (attackCount >= maxAttackCount) { return; }
        
        attackCount++;
        Create(currentAttackKey, point.transform);
        towerAudio.Play();
    }


    // 기본 스킬 함수
    private void DefaultSkill()
    {
        if (skillCount >= maxSkillCount) { return;  }

        skillCount++;
        Create(currentSkillKey, point.transform);
        towerAudio.Play();
    }


    // 공격 설치 횟수 증가 함수
    private void increaseAttackCount()
    {
        maxAttackCount += 2;
    }


    // 스킬 설치 횟수 증가 함수
    private void increaseSkillCount()
    {
        maxSkillCount += 1 ;
    }


    // 유도 화살 생성 함수
    private void HomingArrowPassive()
    {
        Create("Passive_Arrow", transform, 90f);
        Create("Passive_Arrow", transform, -90f);
        Create("Passive_Arrow", transform, 135f);
        Create("Passive_Arrow", transform, -135f);
    }


    // 자폭병 생성 함수
    private void SummonBomb()
    {
        Create("Passive_Bomb", transform);
    }


    // 16 방향 화살 생성 함수
    private void ManyDirectionsArrow()
    {
        int direction = 16;
        float degree = 360f / direction;

        for (int i = 0; i < direction; i++)
        {
            Create("Arrow_AttackPierceable", transform, i * degree);
        }
    }


    // 건축물 설치 횟수 충전 함수
    private void ChargeCount(Tower.TowerType type)
    {
        switch (type)
        {
            case TowerType.Bullet:
                if (attackCount <= 0) { return;  }
                attackCount--;
                break;
            case TowerType.Laser:
                if (skillCount <= 0) { return; }
                skillCount--;
                break;
        }
    }


    // Member Variable //
    // 마우스 오브젝트를 캐싱할 변수
    private GameObject point;

    // 오디오 변수 //
    [SerializeField]
    private AudioSource towerAudio;

    [SerializeField]
    private AudioClip buildSound;

    // 타워 관련 변수 //
    private int maxAttackCount;
    private int attackCount;
    private int maxSkillCount;
    private int skillCount;
}
