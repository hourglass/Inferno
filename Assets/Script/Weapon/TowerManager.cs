using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : WeaponManager
{
    [SerializeField] GameObject attackObj = null;
    [SerializeField] GameObject skillObj = null;

    [SerializeField] GameObject homingObj = null;
    [SerializeField] GameObject bombObj = null;
    [SerializeField] GameObject arrowObj = null;

    [SerializeField] AudioSource towerAudio = null;
    [SerializeField] AudioClip buildSound = null;

    uint maxAttackCount = 3;
    uint currAttackCount = 0;
    uint maxSkillCount = 1;
    uint currSkillCount = 0;


    void Start()
    {
        TowerController.BuildDel += ChargeBuild;
        towerAudio.clip = buildSound;
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
                         delegate { this.increaseAttackCount(); });

        InputChoiceInfos("스킬 구조물 1개 추가");
        InputChoiceDatas(ActionType.Skill,
                         ExecutionType.Immediate,
                         delegate { this.increaseSkillCount(); });

        InputChoiceInfos("주기마다 유도화살 4개 발사");
        InputChoiceDatas(ActionType.Passive,
                         ExecutionType.AddChain,
                         delegate { this.HomingArrowPassive(); }, 2f);

        InputChoiceInfos("주기마다 자폭병 소환");
        InputChoiceDatas(ActionType.Passive,
                         ExecutionType.AddChain,
                         delegate { this.SummonBomb(); }, 2f);

        InputChoiceInfos("주기마다 여러 방향의 화살 발사");
        InputChoiceDatas(ActionType.Passive,
                         ExecutionType.AddChain,
                         delegate { this.ManyDirectionsArrow(); }, 4f);
    }

    void DefaultAttack()
    {
        Vector2 summonPoint = GameObject.Find("point").transform.position;

        if (currAttackCount < maxAttackCount)
        {
            towerAudio.Play();
            Instantiate(attackObj, summonPoint, Quaternion.identity);
            currAttackCount++;
        }
    }

    void DefaultSkill()
    {
        Vector2 summonPoint = GameObject.Find("point").transform.position;

        if (currSkillCount < maxSkillCount)
        {
            Instantiate(skillObj, summonPoint, Quaternion.identity);
            currSkillCount++;
        }
    }

    void increaseAttackCount()
    {
        uint value = 2;
        maxAttackCount = increaseBuildCount(maxAttackCount, value);
    }

    void increaseSkillCount()
    {
        uint value = 1;
        maxSkillCount = increaseBuildCount(maxSkillCount, value);
    }

    uint increaseBuildCount(uint count, uint value)
    {
        count += value;
        return count;
    }

    void HomingArrowPassive()
    {
        Create(homingObj, transform, 90);
        Create(homingObj, transform, -90);
        Create(homingObj, transform, 135);
        Create(homingObj, transform, -135);
    }

    void SummonBomb()
    {
        Create(bombObj, transform, 0);
    }

    void ManyDirectionsArrow()
    {
        int direction = 16;
        float degree = 360f / direction;

        for (int i = 0; i < direction; i++)
        {
            Create(arrowObj, transform, i * degree);
        }
    }

    void ChargeBuild(int id)
    {
        switch (id)
        {
            case 0: currAttackCount--; break;
            case 1: currSkillCount--; break;
        }
    }
}
