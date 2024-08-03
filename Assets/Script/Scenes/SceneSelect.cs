using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour
{
    private void Awake()
    {
        WeaponSpawner.GetweaponIdDel = getGroupIndex;

        for (int i = 0; i < selectToggles.Length; i++)
        {
            selectToggles[i].onValueChanged.AddListener(delegate
            {
                ToggleCheck();
            });
        }

        JoinBtn.onClick.AddListener(JoinToGame);
    }

    private void JoinToGame()
    {
        ChoiceManager.gameIsPaused = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene("3.Game");
    }

    private void ToggleCheck()
    {
        for (int i = 0; i < selectToggles.Length; i++)
        {
            if (selectToggles[i].isOn)
            {
                group_index = i;
            }
        }
    }

    private int getGroupIndex()
    {
        return group_index;
    }


    // Member Variable //
    [SerializeField]
    private Toggle[] selectToggles;
    [SerializeField]
    private Button JoinBtn = null;

    int group_index = 0;
}
