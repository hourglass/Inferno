using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneGameOver : MonoBehaviour
{
    private void Start()
    {
        EndMessage.text = endText;
        BackButton.onClick.AddListener(delegate { BackToMenu(); });
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene("0.Menu");
    }

    public static void setEndText(string _endText)
    {
        endText = _endText;
    }


    // Member Variable //
    [SerializeField] 
    private Button BackButton = null;
    
    [SerializeField] 
    private Text EndMessage = null;

    static string endText = "Game Over";
}
