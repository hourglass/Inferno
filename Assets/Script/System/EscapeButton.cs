using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscapeButton : MonoBehaviour
{
    private void Awake()
    {
        ResumeButton.onClick.AddListener(delegate { ResumeDel(); });
        TitleButton.onClick.AddListener(delegate { BackToMenu(); });
    }


    private void BackToMenu()
    {
        ResumeDel();
        SceneManager.LoadScene("0.Menu");
    }


    // Delegate //
    // 게임 재개 델리게이트
    public delegate void ResumeDelegate();
    public static ResumeDelegate ResumeDel;

    //Member Variable//
    [SerializeField]
    private Button ResumeButton;
    
    [SerializeField]
    private Button TitleButton;
}
