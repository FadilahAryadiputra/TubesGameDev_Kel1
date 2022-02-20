using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetMusicVolume (float volume)
    {
        audioMixer.SetFloat("MusicVolumeParam", volume);
    }
    public void SetSFXVolume (float volume)
    {
        audioMixer.SetFloat("SFXVolumeParam", volume);
    }
}
