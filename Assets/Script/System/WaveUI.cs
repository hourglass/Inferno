using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField] GameObject StartText = null;
    [SerializeField] Text StartNumber = null;
    [SerializeField] Text WaveNumber = null;

    void Start()
    {
        EnemyManager.StartTextDel = StarTextOn;
        EnemyManager.WaveNumberDel = SetWaveNumber;

        StartText.SetActive(false);
    }

    void StarTextOn()
    {
        StartText.SetActive(true);
        Invoke("StarTextOff", 1f);
    }

    void StarTextOff()
    {
        StartText.SetActive(false);
    }

    void SetWaveNumber(int number)
    {
        StartNumber.text = number.ToString();
        WaveNumber.text = number.ToString();
    }
}
