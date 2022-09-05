using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * 
 */
public class PlayerManager : MonoBehaviour
{

    [Header("Game Related")]
    private bool gameOver = false;
    private int ArrivedAtTheEnd = 0; //Compte le nombre d'ennemi qui ont passé la ligne d'arrivée
    private double score = 0f;
    public int income; //income = monnaie du joueur pour construire/améliorer les tours
    private int turretBuilt = 0;
    private int turretUpgraded = 0;
    private int totalAmountReceive = 0;
    private int totalAmountSpent = 0;
    public AudioSource announcerIntruder;


    [Header("Time Related")]
    private int playTime = 0;
    private int seconds = 0;
    private int minutes = 0;
    private int hours = 0;

    [Header("Enemy Related")]
    private int enemyDestroyed = 0;

    [Header("DestructMode")]
    public Button destructModeButton;
    private Color originalColor;

    private bool destructMode = false;


    //[Header("Player profile Related")]
    //private Player player; <= profil du joueur entrain de faire une partie

    /** 
     * Patron utiliser pour choppé des informations sur le joueur, test sur un site dont le repertoire s'appelle umsi_test
     */

    void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(PlayerTimer());
        UpdateIncomeUI();
        totalAmountReceive += income;
        originalColor = destructModeButton.GetComponent<Image>().color;
    }

    //Gère le gameover
    private void Update()
    {
        if (this.ArrivedAtTheEnd == 5)
        {
            gameOver = true;
        }

        if (gameOver)
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy);
            }

            Debug.Log("Jeu termine");
            GameStats.setGameStatScore(this.score);
            GameStats.setGameTime(hours, minutes, seconds);
            GameStats.setTurretBuilt(turretBuilt);
            GameStats.setTurretUpgraded(turretUpgraded);
            GameStats.setIncomeReceived(totalAmountReceive);
            GameStats.setIncomeSpent(totalAmountSpent);
            GameStats.setEnemyDestroyed(enemyDestroyed);
            GameStats.setWaveIndex(GetComponent<WaveSpawner>().getWaveIndex() + 1);
            SceneManager.LoadScene("GameOver");
        }

    }

    /**
     * Coroutines qui va calculer le temps de jeu à partir du moment où on lance le jeu jusqu'au GameOver
     */
    private IEnumerator PlayerTimer()
    {
        while (!gameOver)
        {
            yield return new WaitForSecondsRealtime(1f);
            playTime += 1;
            seconds = (playTime % 60);
            minutes = (playTime / 60) % 60;
            hours = (playTime / 3600) % 24;
        }
        yield break;
    }

    /** gestion de l'income **/
    public bool spendIncome(int amount)
    {
        if (amount <= income)
        {
            income -= amount;
            UpdateIncomeUI();
            return true;
        }
        else
        {
            Debug.Log("Vous ne pouvez pas réalisé cette dépense");
            return false;
        }
    }

    public void receiveIncome(int amount)
    {
        income += amount;
        UpdateIncomeUI();
    }

    public int getIncome()
    {
        return income;
    }

    public void UpdateIncomeUI()
    {
        GameObject.Find("Income").GetComponent<UnityEngine.UI.Text>().text = income.ToString();
    }

    public void setEnnemyArrived()
    {
        gameObject.GetComponent<UIManager>().EnemyPassed(ArrivedAtTheEnd);
        if (!announcerIntruder.isPlaying)
        {
            EmergencyLight();
            if (Time.timeScale == 4)
            {
                announcerIntruder.pitch = 4;
            }
            else
            {
                announcerIntruder.pitch = 1;
            }
            announcerIntruder.Play();
        }
        ArrivedAtTheEnd++;
    }

    public void UpdateScoreUI()
    {
        GameObject.Find("Score").GetComponent<UnityEngine.UI.Text>().text = score.ToString();
    }




    public bool GameOver()
    {
        return gameOver;
    }

    public void setGameOver()
    {
        gameOver = true;
    }

    public int PlayerIncome()
    {
        return income;
    }

    public void receiveScore(double amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    public void removeScore(double amount)
    {
        score -= amount;
        UpdateScoreUI();
    }


    public void EnemyDestroyed()
    {
        enemyDestroyed++;

    }

    /* Used to send game info to data base - obsolete in standalone version */
    /*
    private IEnumerator SendScoreDatabase()
    {
        double sendingscore;
        if (score >= DBManager.meilleurScore)
        {
            sendingscore = score;
            Debug.Log("Nouveau record");
            DBManager.meilleurScore = score;
            GameStats.setIsNewHighScore(true);
        }
        else
        {
            Debug.Log("Pas de nouveau record");
            sendingscore = DBManager.meilleurScore;
            GameStats.setIsNewHighScore(false);
        }
        int time = int.Parse(DBManager.tpsJeu) + playTime;
        int nbmonster = DBManager.nbMonstreTues + enemyDestroyed;//ajouter les monstres tues de la sessions
        string urlrequest = "https://pjs4-umsi.000webhostapp.com/index.php?controleur=api_externe&action=enregistrerNouveauScore&idNiveau=1&dernierScore=" + score + "&nbMonstresTues=" + nbmonster + "&meilleurScore=" + sendingscore + "&tpsJeu=" + time  + "&idJoueur=" + DBManager.IdJoueur;
        UnityWebRequest www = UnityWebRequest.Get(urlrequest);
        Debug.Log("url request : " + urlrequest);
        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError || www.downloadHandler.text.Length > 30)// vérifie également que la connexion a réussie et que le texte retour n'est pas sup a 30
        {
            Debug.Log("connexion failed" + www.downloadHandler.text);
        }
        else
        {
            Debug.Log("connexion réussi");
        }
        DBManager.compteur = 1;
        SceneManager.LoadScene("GameOver");


    }
    */
    private void EmergencyLight()
    {
        GameObject[] everySceneLight = GameObject.FindGameObjectsWithTag("Light");

        foreach (GameObject light in everySceneLight)
        {
            light.GetComponent<SceneLightBehavior>().IntruderDetected();
        }
    }

    public void TurretUpgraded()
    {
        turretUpgraded++;
    }
    public void TurretBuilt()
    {
        turretBuilt++;
    }

    public void TurretDestroyed()
    {
        turretBuilt--;
    }

    public void TotalReceiveIncome(int amount)
    {
        totalAmountReceive += amount;
    }

    public void TotalSpendIncome(int amount)
    {
        totalAmountSpent += amount;
    }

    public void FastDestructMode()
    {
        destructMode = !destructMode;
        if (destructMode)
        {
            destructModeButton.GetComponent<Image>().color = new Color(255f/255f, 0/255f, 0/255f);
        }
        else
        {
            destructModeButton.GetComponent<Image>().color = originalColor;
        }
    }

    public bool IsDestructMode()
    {
        return destructMode;
    }

}
