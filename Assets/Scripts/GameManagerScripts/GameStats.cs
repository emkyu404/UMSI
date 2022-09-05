using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe qui sert de pont entre le jeu et l'écran de gameOver pour stocker les stats et les récupérer facilement
public class GameStats { 

    private static double score = 0;
    private static string hours = "00";
    private static string minutes = "00";
    private static string seconds = "00";
    private static int waveIndex = 0;
    private static int enemyDestroy = 0;
    private static int turretBuilt = 0;
    private static int turretUpgraded = 0;
    private static int incomeSpent = 0;
    private static int incomeReceived = 0;
    private static bool newHighScore = false;

    public static void setGameStatScore(double sc)
    {
        score = sc;
    }

    public static double getGameStatScore()
    {
        return score;
    }

    public static void setGameTime(int h, int m, int s)
    {
        if(h < 10)
        {
            hours = "0"+h;
        }
        else
        {
            hours = h.ToString();
        }
        
        if(m < 10)
        {
            minutes = "0" + m;
        }
        else
        {
            minutes = m.ToString();
        }
        if(s < 10)
        {
            seconds = "0" + s;
        }
        else
        {
            seconds = s.ToString();
        }
    }

    public static string getHours()
    {
        return hours;
    }

    public static string getMinutes()
    {
        return minutes;
    }

    public static string getSeconds()
    {
        return seconds;
    }

    public static void setWaveIndex(int wi)
    {
        waveIndex = wi;
    }

    public static int getWaveIndex()
    {
        return waveIndex;
    }

    public static void setEnemyDestroyed(int ed)
    {
        enemyDestroy = ed;
    }

    public static int getEnemyDestroyed()
    {
        return enemyDestroy;
    }

    public static void setTurretBuilt(int tb)
    {
        turretBuilt = tb;
    }

    public static int getTurretBuilt()
    {
        return turretBuilt;
    }

    public static void setTurretUpgraded(int tu)
    {
        turretUpgraded = tu;
    }

    public static int getTurretUpgraded()
    {
        return turretUpgraded;
    }

    public static void setIncomeSpent(int isp)
    {
        incomeSpent = isp;
    }

    public static int getIncomeSpent()
    {
        return incomeSpent;
    }

    public static void setIncomeReceived(int irc)
    {
        incomeReceived = irc;
    }

    public static int getIncomeReceived()
    {
        return incomeReceived;
    }

    public static bool IsNewHighScore()
    {
        return newHighScore;
    }

    public static void setIsNewHighScore(bool b)
    {
        newHighScore = b;
    }



}
