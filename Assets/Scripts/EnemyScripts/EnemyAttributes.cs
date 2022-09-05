using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * author : Bui Minh-Quân, Reino Anthony
 * last update : 05/02/2020
 * Scripts gérant les attributs des ennemis, il faudra penser à l'implémenter sur chaque prefabs et modifier les valeurs de départ via l'inspecteur Unity
 */
public class EnemyAttributes : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;

    public GameObject deathEffect;

    private float currentHealth = 0f;

    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    double scoreAmount;

    [SerializeField]
    private int incomeReward;

    [SerializeField]
    private Image healthBorder;

    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private Image healthBarBG;

    private GameObject gameManager;

    [SerializeField]
    private GameObject hitPoint;
    //private Canvas CanvasObject;



    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        currentHealth = maxHealth;
        healthBorder.GetComponent<Image>().enabled = false;
        healthBar.GetComponent<Image>().enabled = false;
        healthBarBG.GetComponent<Image>().enabled = false;
    }

    public void setHealth(float h)
    {
        healthBar.fillAmount = h;
    }
    public float getSpeed()
    {
        return speed;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        //Besoin de calculer la vie de cette façon car l'intervalle est 0-1)
        float calculateHealth = currentHealth / maxHealth;
        setHealth(calculateHealth);
    }

    void Update()
    {

        if (currentHealth < maxHealth)
        {
            healthBar.GetComponent<Image>().enabled = true;
            healthBarBG.GetComponent<Image>().enabled = true;
            healthBorder.GetComponent<Image>().enabled = true;
        }

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        gameManager.GetComponent<PlayerManager>().receiveIncome(incomeReward);
        gameManager.GetComponent<PlayerManager>().receiveScore(scoreAmount);
        gameManager.GetComponent<PlayerManager>().EnemyDestroyed();
        gameManager.GetComponent<PlayerManager>().TotalReceiveIncome(incomeReward);
        GameObject df = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(df, 2f);
        Destroy(gameObject);
    }

    public GameObject getHitPoint()
    {
        return hitPoint;
    }

    public void UpgradeHealth(int waveIndex, float multiplier)
    {
        if (waveIndex == 15)
        {
            maxHealth = maxHealth * 2;
        }
        else if(waveIndex < 70)
        {
            maxHealth = maxHealth + ((maxHealth * (0.6f) - (maxHealth * (0.05f) * multiplier)));
        }
        else
        {
            maxHealth = maxHealth + (maxHealth * (0.1f));
        }
    }
}
