using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneMenu : MonoBehaviour
{
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


    // Member Variable //
    [SerializeField] 
    private Button StartBtn;

    [SerializeField] 
    private Button KeyInfoBtn;
    
    [SerializeField] 
    private Button EndBtn;
}
