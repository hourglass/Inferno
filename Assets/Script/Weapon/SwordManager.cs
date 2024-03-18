using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : WeaponManager
{
    //칼 모션을 받아올 함수
    public delegate void SwordMotionDel();
    public static SwordMotionDel MotionDel;

    //공격 & 스킬의 프리팹
    [SerializeField] GameObject slashObj = null;
    [SerializeField] GameObject skillObj = null;
    [SerializeField] GameObject waveObj = null;

    [SerializeField] GameObject homingObj = null;
    [SerializeField] GameObject bombObj = null;

    //오디오
    [SerializeField] AudioClip[] swordSound = null;
    [SerializeField] AudioSource swordAudio = null;
    int audioIndex = 0;

    void Start()
    {
        swordAudio = GetComponent<AudioSource>();
    }

    protected override void InitWeapon()
    {
        attackDel += delegate { DefaultAttack(); };
        skillDel += delegate { DefaultSkill(); };
    }

    protected override void InitChoiceDatas()
    {
        InputChoiceInfos("공격 시 검기 추가");
        InputChoiceDatas(ActionType.Attack,
                         ExecutionType.AddChain,
                         delegate { this.WaveSlash(); });

        InputChoiceInfos("공격 시 유도화살 2개 발사");
        InputChoiceDatas(ActionType.Attack,
                         ExecutionType.AddChain,
                         delegate { this.HomingArrowAttack(); });

        InputChoiceInfos("스킬 연속 3회 시전");
        InputChoiceDatas(ActionType.Skill,
                         ExecutionType.AddChain,
                         delegate { this.BurstSkill(); });

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
        MotionDel();
        PlaySound();

        Create(slashObj, spawnPoint, 0);
    }

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

    void WaveSlash()
    {
        Create(waveObj, spawnPoint, 0);
    }

    void HomingArrowAttack()
    {
        Create(homingObj, transform, 90);
        Create(homingObj, transform, -90);
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
        swordAudio.clip = swordSound[audioIndex];
        audioIndex++;
        if (audioIndex > swordSound.Length - 1)
        {
            audioIndex = 0;
        }
        swordAudio.Play();
    }
}
