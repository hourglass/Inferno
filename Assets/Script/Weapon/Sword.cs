using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private void Awake()
    {
        InitVariable();
    }


    // 변수 초기화 함수
    private void InitVariable()
    {
        WeaponSword.MotionDel = ToggleSwitch;

        toggleCount = 1;
        isSwing = false;
    }


    private void Update()
    {
        if (!ChoiceManager.gameIsPaused)
        {
            if (isSwing)
            {
                Swing();
            }
        }
    }


    private void Swing()
    {
        Quaternion targetRotation;

        if (toggleCount == 0)
        {
            targetRotation = Quaternion.Euler(0, 0, 105f);

            if (transform.localRotation.eulerAngles.z >= 100f)
            {
                targetRotation = Quaternion.Euler(0, 0, 210);
            }
        }
        else
        {
            targetRotation = Quaternion.Euler(0, 0, 95f);

            if (transform.localRotation.eulerAngles.z <= 100f)
            {
                targetRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, 0.5f);
    }


    private void ToggleSwitch()
    {
        isSwing = true;

        if (toggleCount == 0)
        {
            toggleCount = 1;
        }
        else
        {
            toggleCount = 0;
        }

        ChangeSpriteDel(toggleCount);
    }


    // Member Variable //
    // 스프라이트 이미지 변경할 델리게이트
    public delegate void SwordSpriteChangeSpriteDel(int index);
    public static SwordSpriteChangeSpriteDel ChangeSpriteDel;

    private int toggleCount;
    private bool isSwing;
}
