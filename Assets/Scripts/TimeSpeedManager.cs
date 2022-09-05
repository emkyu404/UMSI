using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSpeedManager : MonoBehaviour
{

   public Image backgroundButton;
   private Color currentColor;
   public Color speedUpColor;
    public void Awake()
    {
        currentColor = backgroundButton.color;
    }
    public void speedUp()
    {
        if (Time.timeScale == 1)
        {
            backgroundButton.color = speedUpColor;
            Time.timeScale = 4;
        }
        else
        {
            backgroundButton.color = currentColor;
            Time.timeScale = 1;
        }
    }
}
