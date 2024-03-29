﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneMenu : MonoBehaviour
{
    [SerializeField] Button StartBtn = null;
    [SerializeField] Button KeyInfoBtn = null;
    [SerializeField] Button EndBtn = null;

    // Start is called before the first frame update
    void Start()
    {
        StartBtn.onClick.AddListener(delegate { StartGame(); });
        KeyInfoBtn.onClick.AddListener(delegate { GoToKeyInfo(); });
        EndBtn.onClick.AddListener(delegate { EndGame(); });
    }
    void StartGame()
    {
        SceneManager.LoadScene("2.Select");
    }

    void GoToKeyInfo()
    {
        SceneManager.LoadScene("1.KeyInfo");
    }

    void EndGame()
    {
        Application.Quit();
    }
}
