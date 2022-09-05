using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMissileRed : MonoBehaviour
{
    private Transform target;
    private Vector3 dir;
    private float speed = 50f;
    private bool startMoving = false;
    private bool targetHit = false;

    public void SetTarget(Transform _target)
    {
        target = _target;
        dir = target.position - transform.position;
        startMoving = true;
    }

    private void Update()
    {
        if (startMoving && target != null)
        {
            float distanceThisFrame = speed * Time.deltaTime;
            dir = target.position - transform.position;
            if (dir.magnitude > distanceThisFrame)
            {
                transform.Translate(dir.normalized * distanceThisFrame);
            }
            else
            {
                           
        
                
            }
        }
    }
}
