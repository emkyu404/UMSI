using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * author : Bui Minh-Quân
 * last update : 03/02/2020
 * Il y a beaucoup de chose je commenterais plus tard
 */
 
public class GridManager : MonoBehaviour
{
    [SerializeField]
    private int width;

    [SerializeField]
    private int length;

    [SerializeField]
    private LineRenderer pathLine;

    private SelectManager selectManager;

    private GameObject spawnPointPrefabs, endPointPrefabs, basicTilePrefabs;
    private Vector2 spawnPoint;
    private Vector2 endPoint;
    private List<Node> enemyPath;
    private int[,] grid;
    private PathFinding pathfinder;
    private GameObject currentTurretSelected;
    private GameObject spawnPortal;
    



    private void Awake()
    {
        spawnPointPrefabs = Resources.Load<GameObject>("Prefabs/TilePrefabs/SpawnSample");
        endPointPrefabs = Resources.Load<GameObject>("Prefabs/TilePrefabs/EndPointSample");
        basicTilePrefabs = Resources.Load<GameObject>("Prefabs/TilePrefabs/BasicTile");
        currentTurretSelected = Resources.Load <GameObject>("Prefabs/TurretPrefabs/TurretSample1");
        spawnPortal = Resources.Load<GameObject>("Prefabs/Effects/Portal/SimplePortalBlue");
        selectManager = GetComponent<SelectManager>();
    }


    private void GridGeneration()
    {

        grid = new int[width, length];
        spawnPoint = new Vector2(0,0);
        endPoint = new Vector2(width - 1, length - 1);
        pathfinder = new PathFinding(spawnPoint, endPoint);

        for(int x = 0; x < width; ++x)
        {
            for(int y = 0; y < length; ++y)
            {
                if (spawnPoint == new Vector2(x, y))
                {
                    GameObject tile = (GameObject)Instantiate(spawnPointPrefabs, new Vector2(x, y), Quaternion.identity);
                    GameObject portal = (GameObject)Instantiate(spawnPortal, new Vector3(x, y, -0.8f), Quaternion.identity);
                    tile.transform.parent = GameObject.Find("Grid").transform;
                    portal.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
                    grid[x, y] = 1;
                }
                else if (endPoint == new Vector2(x, y))
                {
                    GameObject tile = (GameObject)Instantiate(endPointPrefabs, new Vector2(x, y), Quaternion.identity);
                    tile.transform.parent = GameObject.Find("Grid").transform;
                    grid[x, y] = 2;
                }
                else
                {
                    GameObject tile = (GameObject)Instantiate(basicTilePrefabs, new Vector2(x, y), Quaternion.identity);
                    tile.transform.parent = GameObject.Find("Grid").transform;
                    grid[x, y] = 0;
                }

            }
        }

        enemyPath = new List<Node>();
        enemyPath = pathfinder.AStarPathFinding(grid);
   
    }
    // Start is called before the first frame update
    void Start()
    {
        if (spawnPointPrefabs != null && endPointPrefabs != null && basicTilePrefabs != null)
        {
            GridGeneration();
            drawPath();
        }
        else
        {
            Debug.Log("Erreur");
        }
    }


    public Vector2 getSpawnPointCoord()
    {
        return spawnPoint;
    }

    public Vector2 getEndPointCoord()
    {
        return endPoint;
    }

    public int[,] getGrid()
    {
        return grid;
    }

    public List<Node> getPath()
    {
        return enemyPath;
    }

    public bool buildTurret(Vector3 tilePosition)
    {
        WaveSpawner waveSpawner = GameObject.Find("GameManager").GetComponent<WaveSpawner>();
        if (!waveSpawner.hasStarted())
        {
            grid[(int)tilePosition.x, (int)tilePosition.y] = -1;
            List<Node> testedPath = pathfinder.AStarPathFinding(grid);
            if (testedPath != null)
            {
                enemyPath = testedPath;
                drawPath();
                return true;
            }
            else
            {
                grid[(int)tilePosition.x, (int)tilePosition.y] = 0;
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void destroyTurret(Vector3 tilePosition)
    {
        grid[(int)tilePosition.x, (int)tilePosition.y] = 0;
        enemyPath =  pathfinder.AStarPathFinding(grid);
        drawPath();
    }

    private void drawPath()
    {

        if(enemyPath.Count > 0)
        {
            pathLine.positionCount = enemyPath.Count;
            Vector3[] points = new Vector3[enemyPath.Count];
            for(int i = 0; i < enemyPath.Count; ++i)
            {
                points[i] = new Vector3(enemyPath[i].getCoordX(), enemyPath[i].getCoordY());
            }
            pathLine.SetPositions(points);
        }
        pathLine.enabled = true;
    }

 

    public GameObject getSelectedTurret()
    {
        return selectManager.getSelectedTurret();
    }

    public void disablePathLine()
    {
        pathLine.enabled = false;
    }

    public void enablePathLine()
    {
        pathLine.enabled =  true;
    }
}
