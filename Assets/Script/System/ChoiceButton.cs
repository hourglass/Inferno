using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    private void Awake()
    {
        InitVariable();
    }


    private void OnDestroy()
    {
        ChoiceManager.SetTextDel -= SetText;
    }


    // 변수 초기화 함수
    private void InitVariable()
    {
        // 선택지 설명 적용 함수
        ChoiceManager.SetTextDel = SetText;

        // 버튼을 누르면 Id를 넘겨주는 함수 실행
        ChoiceBtn[0].onClick.AddListener(delegate { Selected(selectId[0]); });
        ChoiceBtn[1].onClick.AddListener(delegate { Selected(selectId[1]); });
        ChoiceBtn[2].onClick.AddListener(delegate { Selected(selectId[2]); });
    }


    private void SetText()
    {
        // 랜덤으로 뽑힌 Id 저장
        for (int i = 0; i < selectId.Length; i++)
        {
            selectId[i] = GetSelectedIdDel();
        }

        // Id를 통해 선택지 텍스트 가져오기
        for (int i = 0; i < explainText.Length; i++)
        {
            explainText[i].text = GetExplainTextDel(selectId[i]);
        }
    }


    private void Selected(int id)
    {
        // 고른 선택지의 Id 넘겨주기
        ApplyChoiceDel(id);

        // 일시 정지 해제
        ResumeDel();
    }


    // Delegate //
    // 선택지를 고른 후 일시 정지 해제 함수
    public delegate void ChoiceManagerResumeDel();
    public static ChoiceManagerResumeDel ResumeDel;

    // 고른 선택지의 id를 가져오는 함수
    public delegate int WeaponManagerGetSelectedIdDel();
    public static WeaponManagerGetSelectedIdDel GetSelectedIdDel;

    // 선택지들의 설명 텍스트를 가져오는 함수
    public delegate string WeaponManagerGetExplainTextDel(int id);
    public static WeaponManagerGetExplainTextDel GetExplainTextDel;

    // 고른 선택지의 id를 넘겨주는 함수
    public delegate void WeaponManagerApplyChoiceDel(int id);
    public static WeaponManagerApplyChoiceDel ApplyChoiceDel;


    // Member Variable //
    // UI 오브젝트
    [SerializeField]
    private Button[] ChoiceBtn = new Button[3];
    
    [SerializeField]
    private Text[] explainText = new Text[3];

    // Id를 저장할 배열
    private int[] selectId = new int[3];
}
