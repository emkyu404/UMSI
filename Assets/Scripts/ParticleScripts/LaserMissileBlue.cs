using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMissileBlue : MonoBehaviour
{
    [SerializeField]
    private GameObject ExplosionPrefab;
    private Transform target;
    private Vector3 dir;
    private float speed = 50f;
    private bool startMoving = false;
    private bool targetHit = false;
    private float damage;

    public void SetTarget(Transform _target)
    {
        target = _target;
        dir = target.position - transform.position;
        startMoving = true;
    }

    

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    private void Update()
    {
       if(startMoving && target != null)
       {
            float distanceThisFrame = speed * Time.deltaTime;
            dir = target.position - transform.position;
            if(dir.magnitude > distanceThisFrame)
            {
                transform.Translate(dir.normalized * distanceThisFrame);
            }
            else
            {
                if (!targetHit)
                {
                    target.GetComponentInParent<EnemyAttributes>().TakeDamage(damage);
                    GameObject explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
                    Destroy(explosion, 1f);
                    targetHit = true;
                }
                GetComponent<ParticleSystem>().Stop();
                
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Destroy(gameObject, 3f);
                }
            }
       }
    }

   
}
