using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SecneKeyInfo : MonoBehaviour
{
    [SerializeField] Button BackButton = null;

    // Start is called before the first frame update
    void Start()
    {
        BackButton.onClick.AddListener(delegate { BackToMenu(); });
    }

    void BackToMenu()
    {
        SceneManager.LoadScene("0.Menu");
    }
}
