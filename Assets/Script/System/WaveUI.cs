using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    private void Awake()
    {
        EnemySpawner.StartTextDel = StarTextOn;
        EnemySpawner.WaveNumberDel = SetWaveNumber;

        StartText.SetActive(false);
    }

    private void OnDestroy()
    {
        EnemySpawner.StartTextDel -= StarTextOn;
        EnemySpawner.WaveNumberDel -= SetWaveNumber;
    }


    private void StarTextOn()
    {
        StartText.SetActive(true);
        StartCoroutine(TextOffDelay());
    }


    private void StartTextOff()
    {
        StartText.SetActive(false);
    }


    private IEnumerator TextOffDelay()
    {
        WaitForSeconds delay = new WaitForSeconds(1);
        yield return delay;

        StartTextOff();
    }


    private void SetWaveNumber(int number)
    {
        StartNumber.text = number.ToString();
        WaveNumber.text = number.ToString();
    }


    // Member Variable //
    [SerializeField]
    private GameObject StartText;

    [SerializeField]
    private Text StartNumber;

    [SerializeField]
    private Text WaveNumber;
}
