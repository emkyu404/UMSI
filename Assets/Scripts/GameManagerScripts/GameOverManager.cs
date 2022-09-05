using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverManager : MonoBehaviour {

    public GameObject newHighScore;
    void Start()
    {
        GameObject.Find("Score").GetComponent<UnityEngine.UI.Text>().text = GameStats.getGameStatScore().ToString();
        GameObject.Find("Hours").GetComponent<UnityEngine.UI.Text>().text = GameStats.getHours();
        GameObject.Find("Minutes").GetComponent<UnityEngine.UI.Text>().text = GameStats.getMinutes();
        GameObject.Find("Seconds").GetComponent<UnityEngine.UI.Text>().text = GameStats.getSeconds();
        GameObject.Find("WaveIndex").GetComponent<UnityEngine.UI.Text>().text = GameStats.getWaveIndex().ToString();
        GameObject.Find("EnemyDestroyed").GetComponent<UnityEngine.UI.Text>().text = GameStats.getEnemyDestroyed().ToString();
        GameObject.Find("TurretBuilt").GetComponent<UnityEngine.UI.Text>().text = GameStats.getTurretBuilt().ToString();
        GameObject.Find("TurretUpgraded").GetComponent<UnityEngine.UI.Text>().text = GameStats.getTurretUpgraded().ToString();
        GameObject.Find("TotalReceive").GetComponent<UnityEngine.UI.Text>().text = GameStats.getIncomeReceived().ToString();
        GameObject.Find("TotalSpent").GetComponent<UnityEngine.UI.Text>().text = GameStats.getIncomeSpent().ToString();
        if (GameStats.IsNewHighScore())
        {
            newHighScore.SetActive(true);
        }
        else
        {
            newHighScore.SetActive(false);
        }
    }

    public void restart()
    {
        SceneManager.LoadScene("LevelInfinite");
    }

    public void leave()
    {
        SceneManager.LoadScene("Menu");
    }

}

