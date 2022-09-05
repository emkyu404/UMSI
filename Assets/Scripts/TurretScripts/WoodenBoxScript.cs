using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenBoxScript : MonoBehaviour
{
    

    
    private void Awake()
    {
        transform.LookAt(Camera.main.transform);
        transform.rotation = Camera.main.transform.rotation;
       
    }
}
