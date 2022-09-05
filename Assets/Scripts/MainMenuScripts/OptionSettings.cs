using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class OptionSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    void Start()
    {
        resolutions = Screen.resolutions;
        if (!resolutionDropdown == false)
        {
            resolutionDropdown.ClearOptions();//censé clear la liste déroulante de unity mais problème de null pointer





            List<string> options = new List<string>();
            int currentRes = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height + ":" + resolutions[i].refreshRate + "Hz";
                options.Add(option);
                if (resolutions[i].width == Screen.width
                    && resolutions[i].height == Screen.height)
                {
                    currentRes = i;
                }
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentRes;
            resolutionDropdown.RefreshShownValue();
        }
    }
    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    }
    public void SetVolume(float volume)
    {
        if (volume < 25)
        {
            audioMixer.SetFloat("volume", volume);
        }
    }
    public void SetQuality(int qualityindex)
    {
        QualitySettings.SetQualityLevel(qualityindex);

    }
    public void SetfFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
