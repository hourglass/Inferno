using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneGame : MonoBehaviour
{
    void Start()
    {
        PlayerStat.GameOverDel = GameOver;
        EnemyManager.GameClearDel = GameClear;
    }

    void GameOver()
    {
        SceneGameOver.setEndText("Game Over!");
        SceneManager.LoadScene("4.GameOver");
    }

    void GameClear()
    {
        SceneGameOver.setEndText("Clear!");
        SceneManager.LoadScene("4.GameOver");
    }
}
