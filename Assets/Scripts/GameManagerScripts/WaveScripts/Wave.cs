using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * author : Brackeys
 * last update : 03/02/2020
 * Classe utilisé par Brackeys pour gérer les créations de vague et notamment quelques caractéristiques => nombre d'ennemi, fréquence de spawn et numera de la vague
 */ 
public class Wave
{
    private int count;
    private float rate;
    private int waveNumber = 0;

    public Wave()
    {
        waveNumber = 0;
        rate = 1f;
        count = 0;
    }


    public float getRate()
    {
        return rate;
    }
}