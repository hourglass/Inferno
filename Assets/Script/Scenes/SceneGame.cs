using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneGame : MonoBehaviour
{
    private void Start()
    {
        PlayerStat.GameOverDel = GameOver;
        EnemyManager.GameClearDel = GameClear;
    }

    private void GameOver()
    {
        SceneGameOver.setEndText("Game Over!");
        SceneManager.LoadScene("4.GameOver");
    }

    private void GameClear()
    {
        SceneGameOver.setEndText("Clear!");
        SceneManager.LoadScene("4.GameOver");
    }
}
