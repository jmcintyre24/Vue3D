using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// The Audio Channels in the Mixer essentially.
public enum EAudioChannel
{
    SFX,
    Music,
    Master = 100 // Add new Channels above this master channel.
}

/// <summary>
/// - SINGLETON -
/// Controls the playing of Audio Sources and the Mixer.
/// </summary>
public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    public AudioMixer mixer;

    public AudioSource[] srcs;

    private void Awake()
    {
        // Checking if there is already an instance
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public void PlayAudio(AudioClip Audio, EAudioChannel channel)
    {
        int index = (int)channel;
        srcs[index].clip = Audio;
        srcs[index].Play();
    }

    public void StopAllAudio()
    {
        foreach (AudioSource src in srcs)
        {
            src.Stop();
        }
    }

    /* // If we save out options, we'd load them in like this.
    public void LoadAudioOptions()
    {
        // Settings Audio options to what the player had last
        mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
        mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        mixer.SetFloat("UIVolume", PlayerPrefs.GetFloat("UIVolume"));
    }
    */
}
