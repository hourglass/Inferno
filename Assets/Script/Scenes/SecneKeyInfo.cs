using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SecneKeyInfo : MonoBehaviour
{
    void Start()
    {
        BackButton.onClick.AddListener(delegate { BackToMenu(); });
    }

    void BackToMenu()
    {
        SceneManager.LoadScene("0.Menu");
    }


    //Member Variable//
    [SerializeField] Button BackButton = null;
}
