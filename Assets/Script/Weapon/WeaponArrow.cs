using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponArrow : Weapon
{
    protected override void InitWeapon()
    {
        // 기본 공격 & 스킬 함수 등록
        attackDel += delegate { DefaultAttack(); };
        skillDel += delegate { DefaultSkill(); };

        // 공격 & 스킬 프리팹 설정
        currentAttackObj = attackObj;
        currenSkillObj = skillObj;
    }


    protected override void InitChoiceDatas()
    {
        // 선택지 정보 입력
        InputChoiceInfos("공격 3점사");
        InputChoiceDatas(ActionType.Attack,
                         ExecutionType.AddChain,
                         delegate { TripleAttack(); });

        InputChoiceInfos("공격 화살 관통");
        InputChoiceDatas(ActionType.Attack,
                         ExecutionType.Immediate,
                         delegate { SetAttackPierceable(); });

        InputChoiceInfos("스킬 화살 관통");
        InputChoiceDatas(ActionType.Skill,
                         ExecutionType.Immediate,
                         delegate { SetSkillPierceable(); });

        InputChoiceInfos("주기마다 유도화살 4개 발사");
        InputChoiceDatas(ActionType.Passive,
                         ExecutionType.AddChain,
                         delegate { HomingArrowPassive(); }, 2f);

        InputChoiceInfos("주기마다 자폭병 소환");
        InputChoiceDatas(ActionType.Passive,
                         ExecutionType.AddChain,
                         delegate { SummonBomb(); }, 2f);
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


    // 3연속 공격 함수
    void TripleAttack()
    {
        StartCoroutine(TripleRountine());
    }

    IEnumerator TripleRountine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);

        for (int i = 0; i < 2; i++)
        {
            yield return delay;
            DefaultAttack();
        }
    }


    // 공격 프리팹 변경 함수
    void SetAttackPierceable()
    {
        currentAttackObj = pierceableAttackObj;
    }


    // 스킬 프리팹 변경 함수
    void SetSkillPierceable()
    {
        currenSkillObj = pierceableSkillObj;
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


    // 오디오 재생 함수
    void PlaySound()
    {
        int index = Random.Range(0, arrowSound.Length);
        arrowAudio.clip = arrowSound[index];
        arrowAudio.Play();
    }


    //Member Variable//
    // 공격 & 스킬 생성위치
    [SerializeField] private Transform spawnPoint = null;

    // 공격 & 스킬의 프리팹
    [SerializeField] GameObject attackObj;
    [SerializeField] GameObject skillObj;

    // 관통 공격 & 스킬의 프리팹
    [SerializeField] GameObject pierceableAttackObj;
    [SerializeField] GameObject pierceableSkillObj;

    // 패시브 능력 프리팹
    [SerializeField] GameObject homingObj;
    [SerializeField] GameObject bombObj;

    // 오디오
    [SerializeField] AudioClip[] arrowSound;
    [SerializeField] AudioSource arrowAudio;

    // 현재 공격 & 스킬을 저장할 변수
    GameObject currentAttackObj;
    GameObject currenSkillObj;
}
