using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioMixer audioMixer;
    public Slider masterSlider, musicSlider, sfxSlider;
    void Start()
    {
       //PlayerPrefs keeps track of values in between sessions
       if (PlayerPrefs.HasKey("MasterVolume"))
       {
            audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
       }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        }

        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
        }

    }
        
    

}
