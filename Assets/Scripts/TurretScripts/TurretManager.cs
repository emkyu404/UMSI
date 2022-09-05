using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * authors : Bui Minh-Quân, Reino Anthony
 * last update : 04/02/2020
 * Script qui permet de gérer la détection des ennemis par une prefabs "tour" monocible et qui gère le changement de sprite en conséquence
 */

public class TurretManager : MonoBehaviour, ITurretManager
{


    [Header("Sprites")]
    private Sprite[] currentSprites;
    private SpriteRenderer rend;

    [Header("FirePoints")]
    [SerializeField]
    private Transform allFirePoints;
    [SerializeField]
    private Transform currentFirePoint;

    [SerializeField]
    private GameObject shootEffectPrefabs;

    [Header("Detection")]
    private string gameObjectName;
    private DIRECTION looksAt = DIRECTION.IDLE;

    [Header("Attributes")]
    private float fireCountdown = 0f;

    private TurretAttributes ta;
    private TurretDetection td;


    private string enemyTag = "Enemy";
    // Start is called before the first frame update

    void Awake()
    {
        gameObjectName = gameObject.name.Replace("(Clone)", "");
        rend = GetComponentInChildren<SpriteRenderer>();
        string path = "Sprites/TurretSprites/" + gameObjectName;
        currentSprites = Resources.LoadAll<Sprite>(path);
        td = GetComponentInChildren<TurretDetection>();
        ta = GetComponent<TurretAttributes>();
        transform.LookAt(Camera.main.transform);
        transform.rotation = Camera.main.transform.rotation;
        allFirePoints = gameObject.transform.Find("FirePoints");

    }

    private void Update()
    {
        if (td.enemyInRange())
        {
            UpdateTargets();
        }

        if (td.hasTarget())
        {
            LookAtTarget();
            if (fireCountdown <= 0f)
            {
                StartCoroutine(TurretAction());
                fireCountdown = 1f / ta.getFireRate();
            }
            fireCountdown -= Time.deltaTime;
        }
    }

    public void UpdateTargets()
    {
        td.UpdateTargets();
    }

    private void LookAtTarget()
    {
        Vector3 dir = td.GetTargets()[0].transform.position - transform.position;
        float rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Sprite newSprite;

        if (rotation > -180 && rotation <= -157.5 || rotation > 157.5)
        {
            looksAt = DIRECTION.WEST;
            newSprite = findSprite(gameObjectName + "_Left");
            currentFirePoint = allFirePoints.Find("Left");
            if (newSprite != null)
            {
                rend.sprite = newSprite;
            }
        }
        else if (rotation > -157.5 && rotation <= -112.5)
        {
            looksAt = DIRECTION.SOUTH_WEST;
            newSprite = findSprite(gameObjectName + "_DownLeft");
            currentFirePoint = allFirePoints.Find("DownLeft");

            if (newSprite != null)
            {
                rend.sprite = newSprite;
            }
        }
        else if (rotation > -112.5 && rotation <= -67.5)
        {
            looksAt = DIRECTION.SOUTH;
            newSprite = findSprite(gameObjectName + "_Down");
            currentFirePoint = allFirePoints.Find("Down");

            if (newSprite != null)
            {
                rend.sprite = newSprite;
            }
        }
        else if (rotation > -67.5 && rotation <= -22.5)
        {
            looksAt = DIRECTION.SOUTH_EAST;
            newSprite = findSprite(gameObjectName + "_DownRight");
            currentFirePoint = allFirePoints.Find("DownRight");

            if (newSprite != null)
            {
                rend.sprite = newSprite;
            }
        }
        else if (rotation > -22.5 && rotation <= 22.5)
        {
            looksAt = DIRECTION.EAST;
            newSprite = findSprite(gameObjectName + "_Right");
            currentFirePoint = allFirePoints.Find("Right");

            if (newSprite != null)
            {
                rend.sprite = newSprite;
            }
        }
        else if (rotation > 22.5 && rotation <= 67.5)
        {
            looksAt = DIRECTION.NORTH_EAST;
            newSprite = findSprite(gameObjectName + "_UpRight");
            currentFirePoint = allFirePoints.Find("UpRight");

            if (newSprite != null)
            {
                rend.sprite = newSprite;
            }
        }
        else if (rotation > 67.5 && rotation <= 112.5)
        {
            looksAt = DIRECTION.NORTH;
            newSprite = findSprite(gameObjectName + "_Up");
            currentFirePoint = allFirePoints.Find("Up");

            if (newSprite != null)
            {
                rend.sprite = newSprite;
            }
        }
        else if (rotation > 112.5 && rotation <= 157.5)
        {
            looksAt = DIRECTION.NORTH_WEST;
            newSprite = findSprite(gameObjectName + "_UpLeft");
            currentFirePoint = allFirePoints.Find("UpLeft");

            if (newSprite != null)
            {
                rend.sprite = newSprite;
            }
        }
    }


    /**
     * Coroutine lancé lorsque la tour tir
     * Affiche un effet "Laser" qui se trace depuis sa position vers la position de l'ennmi ciblé
     */
    public IEnumerator TurretAction()
    {
        try
        {
            GameObject shootParticle = Instantiate(shootEffectPrefabs, currentFirePoint.position, Quaternion.identity);
            LaserMissileBlue laserMissileBlue = shootParticle.GetComponent<LaserMissileBlue>();
            if(laserMissileBlue != null)
            {
                laserMissileBlue.SetTarget(td.GetTargets()[0].GetComponent<EnemyAttributes>().getHitPoint().transform);
                laserMissileBlue.SetDamage(ta.getTurretDamage());
            }

        }
        catch (System.NullReferenceException ex)
        {
           
        }
        yield return new WaitForSeconds(0.2f);
    
        yield break;

    }


    private Sprite findSprite(string name)
    {
        foreach (Sprite s in currentSprites)
        {
           
            if (s.name == name)
            {
                return s;
            }
        }
        return null;
    }
}
