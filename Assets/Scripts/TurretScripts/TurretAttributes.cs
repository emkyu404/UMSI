using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretAttributes : MonoBehaviour
{

    private readonly int MAX_LEVEL = 3;
    [SerializeField]
    private float range = 3f;
    [SerializeField]
    private float baseturretDamage = 500f;

    [SerializeField]
    private float currentTurretDamage;

    [SerializeField]
    private GameObject UI;

    private TileManager tileManager;

    private AudioSource audioSource;



    [SerializeField]
    private int buildPrice = 500;

    [SerializeField]
    private double scoreAmount;
    [SerializeField]
    private int upgradePrice;
    [SerializeField]
    private int maxTargets;

    [SerializeField]
    private float fireRate;

    [SerializeField]
    private float currentDmgMultiplier;

    [SerializeField]
    private float baseDmgMultiplier = 0.1f;

    [SerializeField]
    private bool upgradeAvailable;
    
    
    [SerializeField]
    private Button upgradeButton;

    [SerializeField]
    private Button destroyButton;

    public GameObject centerStar;
    public GameObject leftStar;
    public GameObject rightStar;
    public GameObject mediumLStar;
    public GameObject mediumRStar;

    public Text upgradePriceUI;
    public Text destroyPriceUI;


    private int TurretLevel = 1;
    

    public void Awake()
    {
        currentTurretDamage = baseturretDamage;
        upgradePrice = buildPrice * 3;
        currentDmgMultiplier = baseDmgMultiplier;
        UI.SetActive(false);
        destroyPriceUI.text = "+" + buildPrice;
        upgradePriceUI.text = "-" + upgradePrice;
        leftStar.SetActive(false);
        rightStar.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if(Time.timeScale != 0)
        {
            if (!upgradeAvailable)
            {
                upgradePriceUI.text = "MAX";
                upgradeButton.interactable = false;
            }
            else if(upgradePrice > GameObject.Find("GameManager").GetComponent<PlayerManager>().getIncome())
            {
                upgradeButton.interactable = false;
            }
            else
            {
                upgradeButton.interactable = true;
            }

            if (tileManager != null && !tileManager.waveHasEnded())
            {
                destroyButton.interactable = false;
            }
            else
            {
                destroyButton.interactable = true;
            }
        }
        else
        {
            destroyButton.interactable = false;
            upgradeButton.interactable = false;
        }
    }

    public void SetTileManager(TileManager tl)
    {
        tileManager = tl;
    }
    public float getRange()
    {
        return range;
    }


    public float getTurretDamage()
    {
        return currentTurretDamage;
    }

    public void updateTurretDamage()
    {
        currentTurretDamage = currentTurretDamage + currentTurretDamage * currentDmgMultiplier;
    }

    public void resetTurretDamage()
    {
        currentTurretDamage = baseturretDamage;
    }


    public int getBuildPrice()
    {
        return buildPrice;
    }

    public int getMaxTargets()
    {
        return maxTargets;
    }

    public float getFireRate()
    {
        return fireRate;
    }

    public void ActivateUI()
    {
        UI.SetActive(true);
    }

    public void DesactivateUI()
    {
        UI.SetActive(false);
    }

    public void DestroyTower()
    {
        tileManager.DestroyTower(buildPrice);
    }

    public void UpgradeClassicTower()
    {
        if (TurretLevel < MAX_LEVEL && tileManager.UpgradeTower(upgradePrice))
        {
            range++;
            fireRate = fireRate * 1.5f; //Augmentation de +150% de la vitesse de tir
            currentTurretDamage = currentTurretDamage * 2f; // Augmentation des dégats de 200%
            upgradePrice = upgradePrice * 2; // augmentation du prix pour 
            upgradePriceUI.text = "-" + upgradePrice;
            audioSource.Play();
            UpdateScoreAmount();
            TurretLevel++;

            if(TurretLevel == 2)
            {
                centerStar.SetActive(false);
                mediumLStar.SetActive(true);
                mediumRStar.SetActive(true);
            }

            if(TurretLevel >= 3)
            {
                centerStar.SetActive(true);
                leftStar.SetActive(true);
                rightStar.SetActive(true);
                mediumLStar.SetActive(false);
                mediumRStar.SetActive(false);
                upgradeAvailable = false;
            }
        }
    }

    public void UpgradeDeathLaserTower()
    {
        if (TurretLevel < MAX_LEVEL && tileManager.UpgradeTower(upgradePrice))
        {
            range++;
            upgradePrice = upgradePrice * 2; // augmentation du prix pour 
            baseturretDamage = baseturretDamage * 2;
            upgradePriceUI.text = "-" + upgradePrice;
            audioSource.Play();
            UpdateScoreAmount();
            TurretLevel++;

            if (TurretLevel == 2)
            {
                centerStar.SetActive(false);
                mediumLStar.SetActive(true);
                mediumRStar.SetActive(true);

            }

            if (TurretLevel >= 3)
            {
                centerStar.SetActive(true);
                leftStar.SetActive(true);
                rightStar.SetActive(true);
                mediumLStar.SetActive(false);
                mediumRStar.SetActive(false);
                upgradeAvailable = false;
            }
        }
    }

    public void UpgradeTeslaTower()
    {
        if (TurretLevel < MAX_LEVEL && tileManager.UpgradeTower(upgradePrice))
        {
            maxTargets = maxTargets + 2; // Augmentation du nombre d'ennemi ciblé d'un 
            currentTurretDamage = (currentTurretDamage * 2); // augmentation de 200% des dégats
            upgradePrice = upgradePrice * 2; // augmentation du prix pour 
            upgradePriceUI.text = "-" + upgradePrice;
            audioSource.Play();
            UpdateScoreAmount();
            TurretLevel++;

            if (TurretLevel == 2)
            {
                centerStar.SetActive(false);
                mediumLStar.SetActive(true);
                mediumRStar.SetActive(true);
            }
            if (TurretLevel >= 3)
            {
                centerStar.SetActive(true);
                leftStar.SetActive(true);
                rightStar.SetActive(true);
                mediumLStar.SetActive(false);
                mediumRStar.SetActive(false);
                upgradeAvailable = false;
            }
        }
    }

    public double getScoreAmount()
    {
        return scoreAmount;
    }
    
    private void UpdateScoreAmount()
    {
        scoreAmount = scoreAmount * 2;
    }

}
