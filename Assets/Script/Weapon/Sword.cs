using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    //스프라이트 이미지 변경할 델리게이트
    public delegate void SwordSpriteChangeSpriteDel(int index);
    public static SwordSpriteChangeSpriteDel ChangeSpriteDel;

    int toggleIndex = 1;

    bool isSwing = false;

    void Start()
    {
        WeaponSword.MotionDel = ToggleSwitch;
    }

    void Update()
    {
        if (!ChoiceManager.gameIsPaused)
        {
            if (isSwing)
            {
                Swing();
            }
        }
    }

    void Swing()
    {
        Quaternion targetRotation;

        if (toggleIndex == 0)
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

    void ToggleSwitch()
    {
        isSwing = true;

        if (toggleIndex == 0)
        {
            toggleIndex = 1;
        }
        else
        {
            toggleIndex = 0;
        }

        ChangeSpriteDel(toggleIndex);
    }
}
