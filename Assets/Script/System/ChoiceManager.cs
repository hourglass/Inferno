using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    public delegate void ChoiceButtonSetTextDel();
    public static ChoiceButtonSetTextDel SetTextDel;

    public delegate void WeaponManagerGetChoiceDel();
    public static WeaponManagerGetChoiceDel GetChoiceDel;

    public static bool gameIsPaused = false;

    [SerializeField] GameObject EscapeUI = null;
    [SerializeField] GameObject ChoiceUI = null;

    void Awake()
    {
        EscapeButton.ResumeDel = EscapeResume;
        ChoiceButton.ResumeDel = ChoiceResume;
        PlayerStat.ShowChoiceDel = ChoicePause;

        EscapeUI.SetActive(false);
        ChoiceUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameIsPaused)
            {
                EscapeUI.SetActive(true);

                gameIsPaused = true;
                Time.timeScale = 0f;
            }
            else if (gameIsPaused && ChoiceUI.activeSelf == false)
            {
                EscapeResume();
            }
        }
    }

    void EscapeResume()
    {
        EscapeUI.SetActive(false);

        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void ChoiceResume()
    {
        ChoiceUI.SetActive(false);

        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void ChoicePause()
    {
        // UI 활성화
        ChoiceUI.SetActive(true);

        // 선택지 정보 가져오기
        GetChoiceDel();

        // 선택지 정보 적용
        SetTextDel();

        Time.timeScale = 0f;
        gameIsPaused = true;
    }
}
