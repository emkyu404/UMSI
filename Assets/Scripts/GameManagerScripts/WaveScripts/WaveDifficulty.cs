using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDifficulty 
{
    private float a = 0.055f;
    private int b = 6;
    private float j = 0.06f;

    public WaveDifficulty() { }

    //Fonction qui calcul la difficulté d'une wave donnée
    public float GetDifficultyValue(float wavenumber)
    {
        return a * Mathf.Pow(wavenumber, 2) + 54 + b * (Mathf.Sin(wavenumber * Mathf.PI * 2 * j + Mathf.PI * 0.5f) * 0.5f + 0.5f);
    }
}

