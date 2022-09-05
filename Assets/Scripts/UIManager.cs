using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("PlayerLife")]
    public Image[] playerlives;

    [Header("PauseMenu")]
    public GameObject PauseMenu;
    public GameObject ConfirmMenu;

    [Header("Shop")]
    public GameObject classicPrefabs;
    public GameObject teslaPrefabs;
    public GameObject deathLaserPrefabs;
    public GameObject woodenBoxPrefabs;

    public Text classicPrice;
    public Text teslaPrice;
    public Text deathLaserPrice;
    public Text woodenBoxPrice;

    public Button woodenBoxButton;
    public Button deathLaserButton;
    public Button teslaturretButton;
    public Button classiqueturetButton;

    

    [Header("Statut")]
    public Text statut;

    private int income;

    private void Start()
    {
        InitializeShopPrice();
        Destroy(GameObject.Find("MenuAudio"));
        income = GameObject.Find("GameManager").GetComponent<PlayerManager>().getIncome();

    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            income = GameObject.Find("GameManager").GetComponent<PlayerManager>().getIncome();
            ShopManager();
        }
    }

    private void InitializeShopPrice()
    {
        classicPrice.text = "-"+ classicPrefabs.GetComponent<TurretAttributes>().getBuildPrice();
        teslaPrice.text = "-" + teslaPrefabs.GetComponent<TurretAttributes>().getBuildPrice();
        deathLaserPrice.text = "-" + deathLaserPrefabs.GetComponent<TurretAttributes>().getBuildPrice();
        woodenBoxPrice.text = "-" + woodenBoxPrefabs.GetComponent<TurretAttributes>().getBuildPrice();
    }

    private void ShopManager()
    {
        if (income < woodenBoxPrefabs.GetComponent<TurretAttributes>().getBuildPrice())
        {
            woodenBoxButton.interactable = false;
        }
        else
        {
            woodenBoxButton.interactable = true;
        }

        if (income < teslaPrefabs.GetComponent<TurretAttributes>().getBuildPrice())
        {
            teslaturretButton.interactable = false;
        }
        else
        {
            teslaturretButton.interactable = true;
        }

        if (income < deathLaserPrefabs.GetComponent<TurretAttributes>().getBuildPrice())
        {
            deathLaserButton.interactable = false;
        }
        else
        {
            deathLaserButton.interactable = true;
        }

        if (income < classicPrefabs.GetComponent<TurretAttributes>().getBuildPrice())
        {
            classiqueturetButton.interactable = false;
        }
        else
        {
            classiqueturetButton.interactable = true;
        }
    }

    public void gameStatut(string gameStatut, bool gamePhase)
    {
        statut.text = gameStatut;
        if (gamePhase)
        {
            statut.color = Color.red;
        }
        else
        {
            statut.color = Color.white;
        }
    }

    public void EnemyPassed(int count)
    {
        playerlives[count].color = new Color(0f/255f,90f/255f,90f/255f);
    }

    public void SurrenderCancel()
    {
        PauseMenu.SetActive(true);
        ConfirmMenu.SetActive(false);
    }

    public void Surrender()
    {
        PauseMenu.SetActive(false);
        ConfirmMenu.SetActive(true);
    }
   
}
