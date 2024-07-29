using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    private void Awake()
    {
        InitVariable();
    }


    private void OnDestroy()
    {
        EscapeButton.ResumeDel -= EscapeResume;
        ChoiceButton.ResumeDel -= ChoiceResume;
        PlayerStat.ShowChoiceDel -= ChoicePause;
    }


    // 변수 초기화 함수
    private void InitVariable()
    {
        EscapeButton.ResumeDel = EscapeResume;  // 일시 정지 해제 함수
        ChoiceButton.ResumeDel = ChoiceResume;  // 선택지 설명 텍스트 적용 함수
        PlayerStat.ShowChoiceDel = ChoicePause; // 랜덤으로 뽑힌 선택지 Id를 가져오는 함수

        EscapeUI.SetActive(false);
        ChoiceUI.SetActive(false);

        gameIsPaused = false;
    }


    private void Update()
    {
        // ESC 키가 눌리면 일시정지 UI 생성
        // 한번 더 눌리면 게임 재개
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


    // ESC 일시 정지 해제
    private void EscapeResume()
    {
        EscapeUI.SetActive(false);

        Time.timeScale = 1f;
        gameIsPaused = false;
    }


    // 선택지 출력 일시정지 해제
    private void ChoiceResume()
    {
        ChoiceUI.SetActive(false);

        Time.timeScale = 1f;
        gameIsPaused = false;
    }


    // 선택지 출력 일시정지
    private void ChoicePause()
    {
        // 선택지 UI 활성화
        ChoiceUI.SetActive(true);

        // 선택지 정보 가져오기
        GetChoiceDel();

        // 선택지 정보 적용
        SetTextDel();

        Time.timeScale = 0f;
        gameIsPaused = true;
    }


    //Delegate//
    // 선택지 설명 텍스트 적용 함수
    public delegate void ChoiceButtonSetTextDel();
    public static ChoiceButtonSetTextDel SetTextDel;

    // 랜덤으로 뽑힌 선택지 Id를 가져오는 함수
    public delegate void WeaponManagerGetChoiceDel();
    public static WeaponManagerGetChoiceDel GetChoiceDel;


    //Member Variable//
    // 일시 정지 상태 값
    public static bool gameIsPaused;

    // UI 게임 오브젝트
    [SerializeField]
    private GameObject EscapeUI;
    
    [SerializeField]
    private GameObject ChoiceUI;
}
