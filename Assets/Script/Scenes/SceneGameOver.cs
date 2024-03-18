using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneGameOver : MonoBehaviour
{
    [SerializeField] Button BackButton = null;
    [SerializeField] Text EndMessage = null;

    static string endText = "Game Over";

    void Start()
    {
        EndMessage.text = endText;

        BackButton.onClick.AddListener(delegate { BackToMenu(); });
    }

    public static void setEndText(string _endText)
    {
        endText = _endText;
    }

    void BackToMenu()
    {
        SceneManager.LoadScene("0.Menu");
    }
}
