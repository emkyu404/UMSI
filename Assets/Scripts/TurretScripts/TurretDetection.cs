using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDetection : MonoBehaviour
{
    private List<GameObject> enemiesInRange;
    private List<GameObject> targets;
    private TurretAttributes attributes;
    private GameObject turret;
    private string enemyTag = "Enemy";
    CircleCollider2D circleCollider;

    private void Awake()
    {
        attributes = GetComponentInParent<TurretAttributes>();
        enemiesInRange = new List<GameObject>();
        targets = new List<GameObject>();
        circleCollider = transform.GetComponent<CircleCollider2D>();
        circleCollider.radius = attributes.getRange();
    }

    private void Update()
    {
        circleCollider.radius = attributes.getRange();
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(enemyTag))
        {
            enemiesInRange.Add(collider.gameObject);
        }
    }


    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag(enemyTag))
        {
            if (enemiesInRange.Contains(collider.gameObject))
            {
                enemiesInRange.Remove(collider.gameObject);
            }
            else if (targets.Contains(collider.gameObject))
            {
                targets.Remove(collider.gameObject);
            }
        }
    }

    public bool enemyInRange()
    {
        return enemiesInRange.Count > 0;
    }

    public bool hasTarget()
    {
        return targets.Count > 0;
    }

    public List<GameObject> GetEnemiesInRange()
    {
        return enemiesInRange;
    }

    public void UpdateTargets()
    {
        int i = 0;
        while (targets.Count < attributes.getMaxTargets())
        {
            try
            {
                targets.Add(enemiesInRange[i]);
                enemiesInRange.Remove(enemiesInRange[i]);
                i++;
            }
            catch
            {
                break;
            }
        }
    }

    public List<GameObject> GetTargets()
    {
        return targets;
    }
}
