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

        // 기본 공격 & 스킬 키 등록
        currentAttackKey = "Arrow_Attack";
        currentSkillKey = "Arrow_Skill";
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
    private void DefaultAttack()
    {
        PlaySound();
        Create(currentAttackKey, spawnPoint);
    }


    // 기본 스킬 함수
    private void DefaultSkill()
    {
        PlaySound();

        for (int i = 0; i < 12; i++)
        {
            Create(currentSkillKey, spawnPoint, 30f * i);
        }
    }


    // 3연속 공격 함수
    private void TripleAttack()
    {
        StartCoroutine(TripleRountine());
    }

    private IEnumerator TripleRountine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);

        for (int i = 0; i < 2; i++)
        {
            yield return delay;
            DefaultAttack();
        }
    }


    // 공격 프리팹 변경 함수
    private void SetAttackPierceable()
    {
        currentAttackKey = "Arrow_AttackPierceable";
    }


    // 스킬 프리팹 변경 함수
    private void SetSkillPierceable()
    {
        currentSkillKey = "Arrow_SkillPierceable";
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
        Create("Passive_Bomb", spawnPoint);
    }


    // 오디오 재생 함수
    private void PlaySound()
    {
        int index = Random.Range(0, arrowSound.Length);
        arrowAudio.clip = arrowSound[index];
        arrowAudio.Play();
    }


    // Member Variable //
    // 공격 & 스킬 생성위치
    [SerializeField] 
    private Transform spawnPoint;

    // 오디오 변수 //  
    [SerializeField] AudioSource arrowAudio;
    [SerializeField] AudioClip[] arrowSound;
}
