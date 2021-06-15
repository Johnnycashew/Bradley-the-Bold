using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Sources")]
    public AudioSource musicAudioSource;
    public AudioSource menuAudioSource;

    [Header("Music")]
    public AudioClip mainMusicClip;

    [Header("Menu Sounds")]
    public AudioClip enterMenu;
    public AudioClip exitMenu;

    private void Start()
    {
        musicAudioSource.clip = mainMusicClip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlayEnterMenu()
    {
        menuAudioSource.Stop();
        menuAudioSource.PlayOneShot(enterMenu);
    }

    public void PlayExitMenu()
    {
        menuAudioSource.Stop();
        menuAudioSource.PlayOneShot(exitMenu);
    }

}
