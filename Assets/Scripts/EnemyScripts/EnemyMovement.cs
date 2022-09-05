using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * author : Bui Minh-Quân
 * last Update : 28/01/2020
 * Script permettant à un ennemi de suivre un chemin
 * 
 */


public class EnemyMovement : MonoBehaviour
{

    /* vitesse de l'entité gameObject */
    private float speed;
    public Animator animator;
    private string gameObjectName;
    private GridManager gridManager;

    public RuntimeAnimatorController[] allAnimator;
    /* Coordonnée dans l'espace de la "cible" de l'ennemi, dans notre cas la case suivante */
    private Vector3 target;

    /* index qui permet de parcourir la classe List<Node> pathNodes */
    private int pathNodesIndex = 0;

    /* Liste d'objet "node" contenant des coordonnées */
    private List<Node> pathNodes;

    private PlayerManager playermanager;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        speed = GetComponent<EnemyAttributes>().getSpeed();
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();
        playermanager = GameObject.Find("GameManager").GetComponent<PlayerManager>();
        gameObjectName = gameObject.name.Replace("(Clone)", "");
        string path = "Animations/EnemyAnimations/" + gameObjectName;
        allAnimator = Resources.LoadAll<RuntimeAnimatorController>(path);
        transform.LookAt(Camera.main.transform);
        transform.rotation = Camera.main.transform.rotation;
        PathFinder();
        GetNextPathNode();
    }



    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target) <= 0.1f)
        {
            GetNextPathNode();
        }
        Vector3 dir = target - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
    }

    public void setNewPath(List<Node> newPath)
    {
        pathNodes = newPath;
    }
    
    /**
     * Fonction qui définit le chemin à suivre
     */
    private void PathFinder()
    {
        pathNodes = gridManager.getPath();
    }

    private void GetNextPathNode()
    {
        if (pathNodesIndex >= pathNodes.Count - 1)
        {
            Destroy(gameObject);
            playermanager.setEnnemyArrived();
        }
        else
        {
            DIRECTION currentDirection = pathNodes[pathNodesIndex].getDirection();
            pathNodesIndex++;
            DIRECTION newDirection = pathNodes[pathNodesIndex].getDirection();
            target = new Vector3(pathNodes[pathNodesIndex].getCoordX(), pathNodes[pathNodesIndex].getCoordY());

            if(currentDirection != newDirection)
            {
                ChangeAnimator(newDirection);
            }
        }
    }

    public Vector3 currentPositionInGrid()
    {
        return new Vector3 (pathNodes[pathNodesIndex - 1].getCoordX(), pathNodes[pathNodesIndex - 1].getCoordY());
    }

    private void ChangeAnimator(DIRECTION direction)
    {
        RuntimeAnimatorController newAnimator;
        switch (direction)
        {
            case DIRECTION.NORTH:
                newAnimator = findAnimator(gameObjectName + "_Up");
                if (newAnimator != null)
                {
                    animator.runtimeAnimatorController = newAnimator;
                }
                break;


            case DIRECTION.EAST:
                newAnimator = findAnimator(gameObjectName + "_Right");
                if (newAnimator != null)
                {
                    animator.runtimeAnimatorController = newAnimator;
                }
                break;

            case DIRECTION.SOUTH:
                newAnimator = findAnimator(gameObjectName + "_Down");
                if (newAnimator != null)
                {
                    animator.runtimeAnimatorController = newAnimator;
                }
                break;

            case DIRECTION.WEST:
                newAnimator = findAnimator(gameObjectName + "_Left");
                if (newAnimator != null)
                {
                    animator.runtimeAnimatorController = newAnimator;
                }
                break;
        }
    }

    private RuntimeAnimatorController findAnimator(string name)
    {
        foreach (RuntimeAnimatorController a in allAnimator){
            if(a.name == name)
            {
                return a;
            }
        }
        return null;
    }
}
