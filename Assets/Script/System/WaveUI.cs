using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    void Awake()
    {
        EnemyManager.StartTextDel = StarTextOn;
        EnemyManager.WaveNumberDel = SetWaveNumber;

        StartText.SetActive(false);
    }


    void StarTextOn()
    {
        StartText.SetActive(true);
        StartCoroutine(TextOffDelay());
    }


    void StartTextOff()
    {
        StartText.SetActive(false);
    }


    IEnumerator TextOffDelay()
    {
        WaitForSeconds delay = new WaitForSeconds(1);
        yield return delay;

        StartTextOff();
    }


    void SetWaveNumber(int number)
    {
        StartNumber.text = number.ToString();
        WaveNumber.text = number.ToString();
    }


    //Member Variable//
    [SerializeField] GameObject StartText;
    [SerializeField] Text StartNumber;
    [SerializeField] Text WaveNumber;
}
