using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{

    private bool pause = false;
    private float currentTimeScale;
    public AudioSource bgmusic;
    public GameObject pauseMenu;
    public GameObject confirmMenu;
    public GameObject[] GUIButtons;
    public GameObject GameUI;

    public WaveSpawner WaveSpawner; // reference to know gamephase

    private void Awake()
    {
        bgmusic = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        pauseMenu.SetActive(false);
        
    }

    private void Start()
    {
        GUIButtons = GameObject.FindGameObjectsWithTag("GUI");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!confirmMenu.activeSelf)
            {
                if (pause)
                {
                    UnPause();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    private void SetActiveGUIButtons(bool val)
    {
        foreach(GameObject button in GUIButtons)
        {
            if (button.name == "StartWaveButton" && val == true)
            {
                button.GetComponent<Button>().interactable = !WaveSpawner.isGamePhase();
            }
            else
            {
                button.GetComponent<Button>().interactable = val;
            }
        }
    }

    public void Pause()
    {
        pause = true;
        currentTimeScale = Time.timeScale;
        Time.timeScale = 0;
        pauseMenu.SetActive(pause);
        SetActiveGUIButtons(false);
        GameUI.SetActive(false);
        bgmusic.Pause();
    }

    public void UnPause()
    {
        pause = false;
        Time.timeScale = currentTimeScale;
        pauseMenu.SetActive(pause);
        SetActiveGUIButtons(true);
        GameUI.SetActive(true);
        bgmusic.UnPause();
    }
}
