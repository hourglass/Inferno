using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSword : Weapon
{
    //칼 모션을 받아올 함수
    public delegate void SwordMotionDel();
    public static SwordMotionDel MotionDel;

    // 공격 & 스킬 생성위치
    [SerializeField] private Transform spawnPoint = null;

    //공격 & 스킬의 프리팹
    [SerializeField] GameObject slashObj;
    [SerializeField] GameObject skillObj;
    [SerializeField] GameObject waveObj;

    [SerializeField] GameObject homingObj;
    [SerializeField] GameObject bombObj;

    //오디오
    [SerializeField] AudioClip[] swordSound;
    [SerializeField] AudioSource swordAudio;
    int audioIndex = 0;


    void Awake()
    {
        swordAudio = GetComponent<AudioSource>();
    }

    protected override void InitWeapon()
    {
        // 기본 공격 & 스킬 함수 등록
        attackDel += delegate { DefaultAttack(); };
        skillDel += delegate { DefaultSkill(); };

        Debug.Log("Called");
    }

    protected override void InitChoiceDatas()
    {
        // 선택지 정보 입력
        InputChoiceInfos("공격 시 검기 추가");
        InputChoiceDatas(ActionType.Attack,
                         ExecutionType.AddChain,
                         delegate { WaveSlash(); });

        InputChoiceInfos("공격 시 유도화살 2개 발사");
        InputChoiceDatas(ActionType.Attack,
                         ExecutionType.AddChain,
                         delegate { HomingArrowAttack(); });

        InputChoiceInfos("스킬 연속 3회 시전");
        InputChoiceDatas(ActionType.Skill,
                         ExecutionType.AddChain,
                         delegate { BurstSkill(); });

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
        MotionDel();
        PlaySound();

        Create(slashObj, spawnPoint, 0);
    }


    // 기본 스킬 함수
    void DefaultSkill()
    {
        swordAudio.clip = swordSound[0];
        swordAudio.Play();

        MotionDel();

        int degree = -45;
        for (int i = 0; i < 3; i++)
        {
            Create(skillObj, spawnPoint, degree + (45 * i));
        }
    }


    // 스킬 3연속 수행 함수
    void BurstSkill()
    {
        StartCoroutine(TripleBurst());
    }

    IEnumerator TripleBurst()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);

        for (int i = 0; i < 2; i++)
        {
            yield return delay;
            DefaultSkill();
        }
    }


    // 검기 생성 함수
    void WaveSlash()
    {
        Create(waveObj, spawnPoint, 0);
    }

    // 유도 화살 생성 함수
    void HomingArrowAttack()
    {
        Create(homingObj, transform, 90);
        Create(homingObj, transform, -90);
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
        swordAudio.clip = swordSound[audioIndex];
        audioIndex++;
        if (audioIndex > swordSound.Length - 1)
        {
            audioIndex = 0;
        }
        swordAudio.Play();
    }
}
