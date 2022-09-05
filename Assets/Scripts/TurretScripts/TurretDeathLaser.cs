using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDeathLaser : MonoBehaviour, ITurretManager
{
    [Header("Sprites")]
    private Sprite[] currentSprites;
    private SpriteRenderer rend;

    [Header("Detection")]
    private string gameObjectName;
    private DIRECTION looksAt = DIRECTION.IDLE;

    [SerializeField]
    private GameObject laserBeam;

    [Header("Attributes")]
    private float fireCountdown = 0f;
    private TurretAttributes turretAttributes;
    private GameObject currentTarget;
    private TurretDetection td;
    private bool particleInstantiate = false;

    public GameObject firePoint;


    private string enemyTag = "Enemy";
    // Start is called before the first frame update

    void Awake()
    {
        gameObjectName = gameObject.name.Replace("(Clone)", "");
        rend = GetComponent<SpriteRenderer>();
        string path = "Sprites/TurretSprites/" + gameObjectName;
        currentSprites = Resources.LoadAll<Sprite>(path);
        td = GetComponentInChildren<TurretDetection>();
        turretAttributes = gameObject.GetComponent<TurretAttributes>();
        transform.LookAt(Camera.main.transform);
        transform.rotation = Camera.main.transform.rotation;
        laserBeam.SetActive(false);
    }

    private void UpdateTowerDamage()
    {
        turretAttributes.updateTurretDamage();
    }

    private void Update()
    {
        if (td.enemyInRange())
        {
            UpdateTargets();
        }

        if (td.hasTarget())
        {
            laserBeam.SetActive(true);
            if (currentTarget == null || currentTarget != td.GetTargets()[0])
            {
                turretAttributes.resetTurretDamage();
                currentTarget = td.GetTargets()[0];
            }
            else {
                Transform startPoint = firePoint.transform;
                Transform endPoint = currentTarget.transform;
                Vector3 startPointVector = transform.InverseTransformPoint(startPoint.position);
                Vector3 endPointVector = transform.InverseTransformPoint(endPoint.position);
                laserBeam.GetComponent<VolumetricLines.VolumetricLineBehavior>().SetStartAndEndPoints(startPointVector, endPointVector);
                if (fireCountdown <= 0f)
                {
                    StartCoroutine(TurretAction());
                    fireCountdown = 1f / turretAttributes.getFireRate();
                }
                fireCountdown -= Time.deltaTime;
            }
        }
        else
        {
            laserBeam.SetActive(false);
            currentTarget = null;
        }
    
    }

    public void UpdateTargets()
    {
        td.UpdateTargets();
    }



    /**
        * Coroutine lancé lorsque la tour tir
        * Affiche un effet "Laser" qui se trace depuis sa position vers la position de l'ennmi ciblé
        */
    public IEnumerator TurretAction()
    {
        try
        {
            td.GetTargets()[0].GetComponent<EnemyAttributes>().TakeDamage(turretAttributes.getTurretDamage());
        }
        catch (System.NullReferenceException ex)
        {
            
        }
        yield return new WaitForSeconds(1f);
        turretAttributes.updateTurretDamage();
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
