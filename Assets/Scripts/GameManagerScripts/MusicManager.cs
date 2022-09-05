using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] tabMusic;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        tabMusic = Resources.LoadAll<AudioClip>("Sounds/Music/");
    }

    public void ChangeBattleMusic()
    {
            audioSource.clip = tabMusic[1];
            audioSource.Play();
            Debug.Log(audioSource.clip.name);
    }

    public void ChangePreparationMusic()
    {
        if (audioSource.clip == tabMusic[0])
        {
            return;
        }
        else
        {
            audioSource.clip = tabMusic[0];
            audioSource.Play();
            Debug.Log(audioSource.clip.name);
        }
    }
}
