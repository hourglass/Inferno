using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour
{
    [SerializeField] Toggle[] selectToggles;

    [SerializeField] Button JoinBtn = null;

    int group_index = 0;

    void Start()
    {
        PlayerManager.GetIndexDel = getGroupIndex;

        for (int i = 0; i < selectToggles.Length; i++)
        {
            selectToggles[i].onValueChanged.AddListener(delegate
            {
                ToggleCheck();
            });
        }

        JoinBtn.onClick.AddListener(JoinToGame);
    }

    void JoinToGame()
    {
        ChoiceManager.gameIsPaused = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene("3.Game");
    }

    void BackToMenu()
    {
        SceneManager.LoadScene("0.Menu");
    }

    void ToggleCheck()
    {
        for (int i = 0; i < selectToggles.Length; i++)
        {
            if (selectToggles[i].isOn)
            {
                group_index = i;
            }
        }
    }

    int getGroupIndex()
    {
        return group_index;
    }
}
