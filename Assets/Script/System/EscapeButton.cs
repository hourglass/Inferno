using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscapeButton : MonoBehaviour
{
    public delegate void ResumeDelegate();
    public static ResumeDelegate ResumeDel;

    [SerializeField] Button ResumeButton = null;
    [SerializeField] Button TitleButton = null;

    void Start()
    {
        ResumeButton.onClick.AddListener(delegate { ResumeDel(); });
        TitleButton.onClick.AddListener(delegate { BackToMenu(); });
    }

    void BackToMenu()
    {
        ResumeDel();
        SceneManager.LoadScene("0.Menu");
    }
}
