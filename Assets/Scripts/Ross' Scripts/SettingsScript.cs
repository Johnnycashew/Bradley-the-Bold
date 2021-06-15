using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Dropdown resolutionDropdown;

    [SerializeField] private Toggle fullscreenToggle;

    [Header("Sliders")]
    [SerializeField] private Slider mainVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sFXVolumeSlider;
    [SerializeField] private Slider voiceVolumeSlider;

    [Header("Setting Windows")]
    [SerializeField] private GameObject audioWindow;
    [SerializeField] private GameObject displayWindow;

    private void Start()
    {
        for (int i = 0; i < resolutionDropdown.options.Count; i++)
        {
            if (Screen.width == int.Parse(resolutionDropdown.options[i].text.Split(' ')[0]) && 
                Screen.height == int.Parse(resolutionDropdown.options[i].text.Split(' ')[2]))
            {
                resolutionDropdown.value = i;
                break;
            }
        }

        fullscreenToggle.isOn = Screen.fullScreen;

        resolutionDropdown.RefreshShownValue();

        audioMixer.GetFloat("MasterVolume", out float mainVolume);
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        audioMixer.GetFloat("VoiceVolume", out float voiceVolume);

        mainVolumeSlider.value = mainVolume;
        musicVolumeSlider.value = musicVolume;
        sFXVolumeSlider.value = sfxVolume;
        voiceVolumeSlider.value = voiceVolume;
    }

    public void SetMainVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }
    
    public void SetVoiceVolume(float volume)
    {
        audioMixer.SetFloat("VoiceVolume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        var width = int.Parse(resolutionDropdown.options[resolutionIndex].text.Split(' ')[0]);
        var height = int.Parse(resolutionDropdown.options[resolutionIndex].text.Split(' ')[2]);
        Screen.SetResolution(width, height, Screen.fullScreen);
    }

    public void WindowChange(int window)
    {
        switch (window)
        {
            case 0:
                audioWindow.SetActive(false);
                displayWindow.SetActive(true);
                break;
            case 1:
                audioWindow.SetActive(true);
                displayWindow.SetActive(false);
                break;
            default:
                break;
        }
        SoundManager.Instance.PlayEnterMenu();
    }

}