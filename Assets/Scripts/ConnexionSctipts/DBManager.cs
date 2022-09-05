using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DBManager
{//classe qui check si la connexion est déjà existante

    public static string pseudo;
    public static int score;
    public static int IdCompte;
    public static string mail;
    public static string statut;
    public static string mdp;
    public static string bConnecte;
    public static string tpsJeu;
    public static double meilleurScore;
    public static int IdJoueur;
    public static int nbMonstreTues;
    public static int compteur = 0;


    public static bool LoggedIn
    {
        get { return pseudo != null; }
    }
    public static void LogOut()
    {
        pseudo = null;
    }
    public static string getUsername()
    {
        return pseudo;
    }
    //ajouter affichage du meilleur score du joueur avec une requete
    // ajouter l'envoi des scores
    public static string getpseudo()
    {
        return pseudo;
    }



    public static string getprenom()
    {
        return pseudo;
    }


}
