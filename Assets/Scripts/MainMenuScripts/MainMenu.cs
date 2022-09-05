
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class MainMenu : MonoBehaviour
{
    public Text nomJoueur;
    public Text Score;
    public double score2;
    public bool boo = false;
    private void Start()
    {
        if (DBManager.LoggedIn)
        {

            StartCoroutine(ScorePlayer());
            nomJoueur.text = " " + DBManager.pseudo;
            Score.text = " " + DBManager.meilleurScore;

        }

    }
    void Update()
    {
        if (DBManager.compteur == 1)
        {
            if (boo == false)
            {
                StartCoroutine(ScorePlayer());
                boo = true;
            }
        }

        if (DBManager.LoggedIn)
        {
            nomJoueur.text = " " + DBManager.pseudo;
            Score.text = " " + score2;
        }

    }
    IEnumerator ScorePlayer()
    {

        Debug.Log(DBManager.pseudo);
        Debug.Log(DBManager.mdp);
        string urlrequest = "https://pjs4-umsi.000webhostapp.com/index.php?controleur=utilisateur&action=getlogingame&uname=" + DBManager.pseudo + "&psw=" + DBManager.mdp;
        UnityWebRequest www = UnityWebRequest.Get(urlrequest);
        Debug.Log("Requête en cours");
        yield return www.SendWebRequest(); // un espèce de wait qui attend que l'instruction au-dessu avant d'executer en bas
        if (www.isHttpError || www.isNetworkError)// vérifie également que la connexion a réussie et que le texte retour n'est pas sup a 30
        {
            Debug.Log("connexion failed" + www.downloadHandler.text);

        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            string[] text = www.downloadHandler.text.Split(' ');

            DBManager.meilleurScore = double.Parse(text[13]);
            score2 = double.Parse(text[13]);

            DBManager.compteur = 0;
        }
    }
    public void GoConnexionMenu()
    {
        SceneManager.LoadScene("GameOver");
    }
}
