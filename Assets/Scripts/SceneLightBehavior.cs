using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLightBehavior : MonoBehaviour
{
    bool waveStart = false;
    bool intruder = false;
    bool emergencyRoutine = false;

    private Light currentLight;
    private float originalIntensity;
    private Color originalColor;
    private Color EmergencyColor;
    private WaveSpawner wspwn;

    private void Awake()
    {
        currentLight = GetComponent<Light>();
        originalColor = currentLight.color;
        originalIntensity = currentLight.intensity;
        EmergencyColor = Color.red;
        wspwn = GameObject.Find("GameManager").GetComponent<WaveSpawner>();
    }
    // Update is called once per frame
    void Update()
    {
        if (intruder)
        {
            StartCoroutine(EnemyDetected());
        }
        
        if (!emergencyRoutine && wspwn.hasStarted())
        {
            StartCoroutine(EmergencyLight());
        }

        if (wspwn.hasEnded())
        {
            emergencyRoutine = false;
        }
    }

    private IEnumerator EmergencyLight()
    {
        emergencyRoutine = true;
        currentLight.color = EmergencyColor;

        for (int i = 0; i < 7; i++)
        {
            yield return new WaitForSeconds(0.2f);
            currentLight.intensity = 0.5f;
            yield return new WaitForSeconds(0.2f);
            currentLight.intensity = originalIntensity;
        }
        currentLight.color = originalColor;
        
        yield break;
    }

    private IEnumerator EnemyDetected()
    {
        intruder = false;
        currentLight.color = EmergencyColor;

        for (int i = 0; i < 7; i++)
        {
            yield return new WaitForSeconds(0.2f);
            currentLight.intensity = 0.5f;
            yield return new WaitForSeconds(0.2f);
            currentLight.intensity = originalIntensity;
        }
        currentLight.color = originalColor;
    }

    public void WaveStarted()
    {
        waveStart = true;
    }

    public void IntruderDetected()
    {
        Debug.Log("Called");
        intruder = true;
    }

  
}
