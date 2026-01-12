using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class OptionsScreen : MonoBehaviour
{

    public Toggle fullScreenToggle, vsyncToggle;
    public List<ResSelect> resolutions = new List<ResSelect>();
    private int selectedResolution; //Keeps track of selected res.

    //Reference the text mesh pro so we can edit or update it
    public TMP_Text resolutionLabel;

    //Reference to Audio Mixer
    public AudioMixer audioMixer;

    public TMP_Text masterLabel, musicLabel, sfxLabel;
    public Slider masterSlider, musicSlider, sfxSlider;


    // Start is called before the first frame update
    void Start()
    {
        fullScreenToggle.isOn = Screen.fullScreen;

        if(QualitySettings.vSyncCount == 0)
        {
            vsyncToggle.isOn = false;
        } else
        {
            vsyncToggle.isOn = true;
        }

        //Loops through the list of resolutions and finds if it is the same resolution selected when opening the game
        bool foundRes = false;
        for(int i = 0; i < resolutions.Count; i++)
        {
            if(Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;
                selectedResolution = i;
                UpdateResLabel();
            }
        }

        //If the user has a different screen resolution or monitor size other than the ones we listed already
        //this creates a resolution that matches what the user has and adds it to the list (that's why we also made a res. searcher)
        if (!foundRes)
        {
            //Create a newResolution based on user's Monitor Resolution and add it to the list in the "OptionsScreen" Game Object
            ResSelect newRes = new ResSelect();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);
            selectedResolution = resolutions.Count - 1;

            UpdateResLabel();
        }

        //SOUND
        //Save values through audio manager
        float vol = 0f;
        audioMixer.GetFloat("MasterVolume", out vol);
        masterSlider.value = vol;
        audioMixer.GetFloat("MusicVolume", out vol);
        musicSlider.value = vol;
        audioMixer.GetFloat("SFXVolume", out vol);
        sfxSlider.value = vol;

    }

    //GRAPHICS
    //Cycles through the resolutions provided with ResSelect class within the list provided in the editor
    public void ResLeft()
    {
        selectedResolution--;
        if(selectedResolution < 0)
        {
            selectedResolution = 0;
        }

        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedResolution++;
        if (selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count - 1;
        }

        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        //This updates the screen resolution text that is shown in the options menu to show what is currently selected
        //Grabs the horizontal and vertical values from our resolutions array and converts them into a string that will update our label
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    public void ApplyGraphics()
    {
        //Screen becomes full-screen if the toggle is "ON"
        Screen.fullScreen = fullScreenToggle.isOn;

        //What is VSync: for gamers, we know this as a lock on the FPS
        if (vsyncToggle.isOn)
        {
            //vSyncCount to 1 locks FPS to 60
            //If vSyncToggle button is on, just turn it on
            QualitySettings.vSyncCount = 1;
        } 
        else
        {
            //If its vSyncToggle is not on, vsync is not on
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullScreenToggle.isOn);
    }

    //MUSIC
    //Master Volume is called whenever a change is made to the slider in the options
    public void SetMasterVolume (float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sliderValue); 
    }

}

[System.Serializable] //Allows unity to display the list on the editor
public class ResSelect 
{
    public int horizontal, vertical;
}
    