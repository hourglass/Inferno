using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : WeaponManager
{
    // 공격 & 스킬의 프리팹
    [SerializeField] GameObject attackObj = null;
    [SerializeField] GameObject skillObj = null;

    // 관통 공격 & 스킬의 프리팹
    [SerializeField] GameObject pierceableAttackObj = null;
    [SerializeField] GameObject pierceableSkillObj = null;

    // 패시브 능력 프리팹
    [SerializeField] GameObject homingObj = null;
    [SerializeField] GameObject bombObj = null;

    // 오디오
    [SerializeField] AudioClip[] arrowSound = null;
    [SerializeField] AudioSource arrowAudio = null;

    GameObject defaultAttackObj = null;
    GameObject defaultSkillObj = null;

    protected override void InitWeapon()
    {
        defaultAttackObj = attackObj;
        defaultSkillObj = skillObj;
        attackDel += delegate { DefaultAttack(); };
        skillDel += delegate { DefaultSkill(); };
    }

    protected override void InitChoiceDatas()
    {
        InputChoiceInfos("공격 3점사");
        InputChoiceDatas(ActionType.Attack,
                         ExecutionType.AddChain,
                         delegate { this.TripleAttack(); });

        InputChoiceInfos("공격 화살 관통");
        InputChoiceDatas(ActionType.Attack,
                         ExecutionType.Immediate,
                         delegate { this.SetAttackPierceable(); });

        InputChoiceInfos("스킬 화살 관통");
        InputChoiceDatas(ActionType.Skill,
                         ExecutionType.Immediate,
                         delegate { this.SetSkillPierceable(); });

        InputChoiceInfos("주기마다 유도화살 4개 발사");
        InputChoiceDatas(ActionType.Passive,
                         ExecutionType.AddChain,
                         delegate { this.HomingArrowPassive(); }, 2f);

        InputChoiceInfos("주기마다 자폭병 소환");
        InputChoiceDatas(ActionType.Passive,
                         ExecutionType.AddChain,
                         delegate { this.SummonBomb(); }, 2f);
    }


    void DefaultAttack()
    {
        PlaySound();
        Create(defaultAttackObj, spawnPoint, 0);
    }

    void DefaultSkill()
    {
        PlaySound();

        for (int i = 0; i < 12; i++)
        {
            Create(defaultSkillObj, spawnPoint, 30 * i);
        }
    }

    void TripleAttack()
    {
        attackDelay = 0.5f;
        StartCoroutine(TripleBurst());
    }

    IEnumerator TripleBurst()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);

        for (int i = 0; i < 2; i++)
        {
            yield return delay;
            DefaultAttack();
        }
    }

    void SetAttackPierceable()
    {
        defaultAttackObj = pierceableAttackObj;
    }

    void SetSkillPierceable()
    {
        defaultSkillObj = pierceableSkillObj;
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

    void PlaySound()
    {
        int index = Random.Range(0, arrowSound.Length);
        arrowAudio.clip = arrowSound[index];
        arrowAudio.Play();
    }
}
