using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioBehavior : MonoBehaviour
{
    private AudioSource enemyAudioSource;
    public bool loop;

    private void Awake()
    {
        enemyAudioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (enemyAudioSource.isPlaying)
            {
                if (Time.timeScale == 4)
                {
                    enemyAudioSource.pitch = 4;
                }
                else
                {
                    enemyAudioSource.pitch = 1;
                }
            }
            else
            {
                if (loop)
                {
                    enemyAudioSource.Play();
                }
            }
        }
        else
        {
            enemyAudioSource.Pause();
        }
    }
}
