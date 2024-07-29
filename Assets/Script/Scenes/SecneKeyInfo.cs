using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SecneKeyInfo : MonoBehaviour
{
    private void Start()
    {
        BackButton.onClick.AddListener(delegate { BackToMenu(); });
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene("0.Menu");
    }


    // Member Variable //
    [SerializeField]
    private Button BackButton = null;
}
