using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


/**
 * author : Bui Minh-Quân, Reino Anthony
 * last update : 04/02/2020
 * Script gérant les vagues d'ennemis successives
 */
public class WaveSpawner : MonoBehaviour
{
    private Vector3 spawnPoint;

    private float spawnRate = 1f;

    private int healthMultiplier = 0;

    public int waveNumberUpgrade = 10;

    private readonly int START_UPGRADING_HEALTH = 15;

    [SerializeField]
    public GameObject gameManager;

    List<List<GameObject>> hardCodeWaves;

    [SerializeField]
    public Wave wave;


    public WaveBuilder[] allWaveBlocs;
    public List<GameObject> WaveMonsters;

    //private ScriptableObject[] waveAlmanac; // Contient TOUT les types d'ennemis dans le dossier Resources/Prefabs/EnemyPrefabs

    public float timeBetweenWaves = 5f;
    private float countdown = 3f;

    private int waveIndex = 0;
    private bool isSpawning = false;
    private bool waveStarted = false;
    private bool waveEnded = false;
    private bool gamePhase = false;


    public Text waveNumber;

    public AudioSource announcer;



    [Header("UI")]
    private GameObject startButton;

    private void Awake()
    {
        startButton = GameObject.Find("StartWaveButton");
        allWaveBlocs = Resources.LoadAll<WaveBuilder>("MonsterPacks");
        spawnPoint = gameManager.GetComponent<GridManager>().getSpawnPointCoord();
        hardCodeWaves = new List<List<GameObject>>();
        buildFirstWaves();
    }
    void Update()
    {
        if (isSpawning && waveStarted)
        {
            StartCoroutine(SpawnWave());
            return;
        }
        else
        {
            int enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (!isSpawning && enemiesAlive <= 0)
            {
                StopWave();
            }
        }

        //countdown -= Time.deltaTime;

        //countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
    }

    public List<GameObject> createWave()
    {
        WaveMonsters = new List<GameObject>();
        WaveDifficulty scoreDiff = new WaveDifficulty();
        float scoreDifficulty = scoreDiff.GetDifficultyValue(waveIndex);

        int scoreMonstre = 0;
        List<WaveBuilder> monstersList = new List<WaveBuilder>();
        monstersList.AddRange(allWaveBlocs);

        while (scoreMonstre < scoreDifficulty && monstersList.Count > 0)
        {
            int n = UnityEngine.Random.Range(0, monstersList.Count);
            UnityEngine.Debug.Log(n);
            WaveBuilder monster = monstersList[n];
            int testScore = scoreMonstre += monster.Score;

            if (testScore <= scoreDifficulty)
            {
                scoreMonstre = testScore;
                WaveMonsters.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/" + monster.prefabName));
            }
        }

        return WaveMonsters;
    }

    IEnumerator SpawnWave()
    {
        waveEnded = false;
        waveStarted = false;
        gamePhase = true;
        waveIndex++;

       

        //Liste qui va être retourné et qui contient la wave à lancer
        List<GameObject> monsterList = new List<GameObject>();

        if (waveIndex <= 10)
        {
            monsterList = hardCodeWaves[waveIndex - 1];
        }
        else
        {
            monsterList = createWave();
        }
        yield return new WaitForSeconds(3f); // 3 secondes avant le début;
        if (waveIndex >= START_UPGRADING_HEALTH)
        {
            if ((waveIndex % waveNumberUpgrade) == 0)
            {
                UpgradePrefabsMaxPV();
                healthMultiplier++;
            }
        }
        //Spawning des ennemis
        for (int i = 0; i < monsterList.Count; ++i)
        {
            SpawnEnemy(monsterList[i]);
            yield return new WaitForSeconds(1/spawnRate);
        }
        Debug.Log("Vague : " + waveIndex + " multiplicateur : " +healthMultiplier);

        isSpawning = false;
        yield break;
    }


    void SpawnEnemy(GameObject prefabs)
    {
        Instantiate(prefabs, spawnPoint, Quaternion.identity);
    }

    public void StartWave()
    {
        if (Time.timeScale != 0)
        {
            DesactivateEveryOtherUI();
            isSpawning = true;
            waveStarted = true;
            HandleStartButton(false);
            EmergencyLight();
            gameManager.GetComponent<UIManager>().gameStatut("Vague en cours", true);
            
            if (Time.timeScale == 4)
            {
                announcer.pitch = 4;
            }
            else
            {
                announcer.pitch = 1;
            }

            announcer.Play();
        }
    }

    private void EmergencyLight()
    {
        GameObject[] everySceneLight = GameObject.FindGameObjectsWithTag("Light");

        foreach(GameObject light in everySceneLight)
        {
            light.GetComponent<SceneLightBehavior>().WaveStarted();
        }
    }

    private void DesactivateEveryOtherUI()
    {
        GameObject[] UIs = GameObject.FindGameObjectsWithTag("TurretUI");

        foreach (GameObject tUI in UIs)
        {
            tUI.SetActive(false);
        }
    }

    public void StopWave()
    {
        if (Time.timeScale != 0)
        {
            HandleStartButton(true);
            UpdateUI();
            waveEnded = true;
            gamePhase = false;
            gameObject.GetComponent<GridManager>().enablePathLine();
            gameManager.GetComponent<UIManager>().gameStatut("Phase de préparation", false);
        }
    }

    public bool hasStarted()
    {
        return isSpawning;
    }

    public bool hasEnded()
    {
        return waveEnded;
    }


    private void HandleStartButton(bool b)
    {
        startButton.GetComponent<Button>().interactable = b;
    }

    public bool isPlaying()
    {
        return gamePhase;
    }

    private void UpdateUI()
    {
        waveNumber.text = (waveIndex + 1).ToString();
    }

    private void buildFirstWaves()
    {
        //Liste qui contient les waves hardcode
        

        //Les 10 premières waves hardcode 
        //Wave 1 
        List<GameObject> Wave1 = new List<GameObject>();
        Wave1.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave1.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave1.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        hardCodeWaves.Add(Wave1);

        //Wave2
        List<GameObject> Wave2 = new List<GameObject>();
        Wave2.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave2.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave2.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave2.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        hardCodeWaves.Add(Wave2);

        //wave3
        List<GameObject> Wave3 = new List<GameObject>();
        Wave3.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave3.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave3.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave3.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        hardCodeWaves.Add(Wave3);

        //wave4
        List<GameObject> Wave4 = new List<GameObject>();
        Wave4.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave4.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave4.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave4.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave4.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave4.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        hardCodeWaves.Add(Wave4);

        //wave5
        List<GameObject> Wave5 = new List<GameObject>();
        Wave5.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/BF-Bot"));
        Wave5.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave5.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave5.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave5.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        hardCodeWaves.Add(Wave5);

        //wave6
        List<GameObject> Wave6 = new List<GameObject>();
        Wave6.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave6.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave6.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave6.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/BF-Bot"));
        Wave6.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave6.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        hardCodeWaves.Add(Wave6);

        //wave7
        List<GameObject> Wave7 = new List<GameObject>();
        Wave7.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/BF-Bot"));
        Wave7.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave7.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave7.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave7.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave7.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave7.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave7.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave7.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave7.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave7.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));

        hardCodeWaves.Add(Wave7);

        //wave8
        List<GameObject> Wave8 = new List<GameObject>();
        Wave8.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/BF-Bot"));
        Wave8.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave8.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave8.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave8.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/BF-Bot"));
        Wave8.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave8.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave8.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        hardCodeWaves.Add(Wave8);

        //wave9
        List<GameObject> Wave9 = new List<GameObject>();
        Wave9.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/BF-Bot"));
        Wave9.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/BF-Bot"));
        Wave9.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/BF-Bot"));
        Wave9.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave9.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        hardCodeWaves.Add(Wave9);

        //wave10
        List<GameObject> Wave10 = new List<GameObject>();
        Wave10.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/BF-Bot"));
        Wave10.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave10.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave10.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave10.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave10.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave10.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        Wave10.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MiniTron"));
        Wave10.Add(Resources.Load<GameObject>("Prefabs/EnemyPrefabs/MediaTron"));
        hardCodeWaves.Add(Wave10);
    }

    private void UpgradePrefabsMaxPV()
    {
        GameObject[] allEnemyPrefabs = Resources.LoadAll<GameObject>("Prefabs/EnemyPrefabs/");
        foreach(GameObject prefab in allEnemyPrefabs)
        {
            prefab.GetComponent<EnemyAttributes>().UpgradeHealth(waveIndex, healthMultiplier);
        }
    }

    public int getWaveIndex()
    {
        return waveIndex;
    }

    public bool isGamePhase()
    {
        return gamePhase;
    }
    

}

