using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * author : Bui Minh-Quân
 * last update : 05/02/2020
 * Script gérant la construction des tours notamment
 */
public class TileManager : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer rend;

 
    private Sprite hoverSprite;
    private PlayerManager pm;
    private Sprite basicSprite;
    private GameObject gameManager;
    private string gameObjectName;

    [Header("Turret in")]
    private bool isAvailable;

    private bool UIActive = false;
    [SerializeField]
    private GameObject turretPlaced;

    private Material currentMat;

    [SerializeField]
    private Material diffuseMat;

    [SerializeField]
    private Material defaultMat;

    private bool destructMode = false;

    // Start is called before the first frame update
    void Start()
    {
        isAvailable = true;
        rend = GetComponent<SpriteRenderer>();
        basicSprite = Resources.Load<Sprite>("Sprites/EnvironnementSprites/Ground/BasicTile");
        hoverSprite = Resources.Load<Sprite>("Sprites/EnvironnementSprites/Ground/HoveredTile");
        currentMat = GetComponent<SpriteRenderer>().material;
        gameManager = GameObject.Find("GameManager");
        gameObjectName = gameObject.name.Replace("(Clone)", "");
    }

    private void Update()
    {
        if(destructMode != gameManager.GetComponent<PlayerManager>().IsDestructMode())
        {
            DesactivateEveryOtherUI();
        }
        destructMode = gameManager.GetComponent<PlayerManager>().IsDestructMode();
           
    }


    private void OnMouseEnter()
    {
        if (Time.timeScale != 0) // Vérifie si le jeu n'est pas en pause
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (isAvailable)
                {
                    rend.sprite = hoverSprite;
                    rend.material = defaultMat;
                }
            }
            else
            {
                rend.sprite = basicSprite;
                rend.material = diffuseMat;
            }
        }
        else
        {
            rend.sprite = basicSprite;
            rend.material = diffuseMat;
        }
    }

    private void OnMouseExit()
    {
        if (Time.timeScale != 0) // Vérifie si le jeu n'est pas en pause
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (isAvailable)
                {
                    rend.sprite = basicSprite;
                    rend.material = diffuseMat;
                }
            }
            else
            {
                rend.sprite = basicSprite;
                rend.material = diffuseMat;
            }
        }
        else
        {
            rend.sprite = basicSprite;
            rend.material = diffuseMat;
        }

    }

    private void OnMouseDown()
    {
        if (Time.timeScale != 0) // Vérifie si le jeu n'est pas en pause
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                GameObject currentTurretPrefabs = gameManager.GetComponent<GridManager>().getSelectedTurret();
                int currentPlayerIncome = gameManager.GetComponent<PlayerManager>().getIncome();
                int currentTurretPrice = currentTurretPrefabs.GetComponent<TurretAttributes>().getBuildPrice();
                DesactivateEveryOtherUI();

                if (turretPlaced != null)
                {
                    if (destructMode)
                    {
                        DestroyTower(turretPlaced.GetComponent<TurretAttributes>().getBuildPrice());
                    }
                    else
                    {
                        if (!UIActive)
                        {
                            ActivateUI();
                            UIActive = true;
                        }
                        else
                        {
                            DesactivateUI();
                            UIActive = false;
                        }
                    }
                }
                else if (gameManager.GetComponent<WaveSpawner>().hasEnded())
                {
                    BuildTower(currentPlayerIncome, currentTurretPrice, currentTurretPrefabs);
                }
                else
                {
                    Debug.Log("Construction impossible Une vague est en cours");
                }
            }
        }
    }

    private void BuildTower(int currentPlayerIncome, int currentTurretPrice, GameObject currentTurretPrefabs)
    {
        if (currentTurretPrice <= currentPlayerIncome)
        {
            if (isAvailable && gameManager.GetComponent<GridManager>().buildTurret(transform.position))
            {
                turretPlaced = Instantiate(currentTurretPrefabs, transform.position, Quaternion.identity);
                turretPlaced.GetComponent<TurretAttributes>().SetTileManager(this);
                isAvailable = false;
                gameManager.GetComponent<PlayerManager>().spendIncome(currentTurretPrice);
                gameManager.GetComponent<PlayerManager>().receiveScore(turretPlaced.GetComponent<TurretAttributes>().getScoreAmount());
                gameManager.GetComponent<PlayerManager>().TotalSpendIncome(currentTurretPrice);
                gameManager.GetComponent<PlayerManager>().TurretBuilt();
                rend.sprite = basicSprite;
                rend.material = diffuseMat;
            }
            else
            {
                Debug.Log("Construction impossible : toutes les issues seraient bloqué");
            }
        }
        else
        {
            Debug.Log("Construction impossible  : vous n'avez pas assez de ressource !");
        }
    }

    public bool UpgradeTower(int upgradeCost)
    {
        gameManager.GetComponent<PlayerManager>().receiveScore(turretPlaced.GetComponent<TurretAttributes>().getScoreAmount());
        gameManager.GetComponent<PlayerManager>().TotalSpendIncome(upgradeCost);
        gameManager.GetComponent<PlayerManager>().TurretUpgraded();
        return gameManager.GetComponent<PlayerManager>().spendIncome(upgradeCost);
    }

    public void DestroyTower(int currentTurretPrice)
    {
        if (gameManager.GetComponent<WaveSpawner>().hasEnded())
        {
            Destroy(turretPlaced);
            gameManager.GetComponent<PlayerManager>().removeScore(turretPlaced.GetComponent<TurretAttributes>().getScoreAmount());
            gameManager.GetComponent<PlayerManager>().receiveIncome(currentTurretPrice);
            gameManager.GetComponent<PlayerManager>().TotalSpendIncome(-currentTurretPrice);
            gameManager.GetComponent<PlayerManager>().TurretDestroyed();
            gameManager.GetComponent<GridManager>().destroyTurret(transform.position);
            isAvailable = true;

        }
        else
        {
            Debug.Log("La wave à commencer vous ne pouvez pas réalisé cette action");
        }
    }

    private void ActivateUI()
    {
        turretPlaced.GetComponent<TurretAttributes>().ActivateUI();
    }

    private void DesactivateUI()
    {
        turretPlaced.GetComponent<TurretAttributes>().DesactivateUI();
    }

    private void DesactivateEveryOtherUI()
    {
        GameObject[] UIs = GameObject.FindGameObjectsWithTag("TurretUI");

        foreach(GameObject tUI in UIs)
        {
            tUI.SetActive(false);
        }
    }

    public bool waveHasEnded()
    {
        return gameManager.GetComponent<WaveSpawner>().hasEnded();
    }


}
