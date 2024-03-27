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

    // 현재 공격 & 스킬을 저장할 변수
    GameObject currentAttackObj = null;
    GameObject currenSkillObj = null;


    protected override void InitWeapon()
    {
        // 기본 공격 & 스킬 프리팹 등록
        currentAttackObj = attackObj;
        currenSkillObj = skillObj;

        // 기본 공격 & 스킬 함수 등록
        attackDel += delegate { DefaultAttack(); };
        skillDel += delegate { DefaultSkill(); };
    }

    protected override void InitChoiceDatas()
    {
        // 선택지 정보 입력
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


    // 기본 공격 함수
    void DefaultAttack()
    {
        PlaySound();
        Create(currentAttackObj, spawnPoint, 0);
    }

    // 기본 스킬 함수
    void DefaultSkill()
    {
        PlaySound();

        for (int i = 0; i < 12; i++)
        {
            Create(currenSkillObj, spawnPoint, 30 * i);
        }
    }


    // 공격 3연속 수행 함수
    void TripleAttack()
    {
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

    // 기본 공격 & 스킬 프리팹 변경 함수
    void SetAttackPierceable()
    {
        currentAttackObj = pierceableAttackObj;
    }

    void SetSkillPierceable()
    {
        currenSkillObj = pierceableSkillObj;
    }

    // 패시브 오브젝트 생성 함수
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

    // 소리 재생 함수
    void PlaySound()
    {
        int index = Random.Range(0, arrowSound.Length);
        arrowAudio.clip = arrowSound[index];
        arrowAudio.Play();
    }
}
