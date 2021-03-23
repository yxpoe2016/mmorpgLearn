using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{

    public AudioMixer audioMixer;

    public AudioSource musicAudioSource;

    public AudioSource soundAudioSource;

    private const string MusicPath = "Music/";

    private const string SoundPath = "Sound/";

    private bool musicOn;
    public bool MusicOn
    {
        get
        {
            return musicOn;
        }
        set
        {
            musicOn = value;
            this.MusicMute(!musicOn);
        }
    }

    private bool soundOn;
    public bool SoundOn
    {
        get
        {
            return soundOn;
        }
        set
        {
            soundOn = value;
            this.SoundMute(!soundOn);
        }
    }

    private int musicVolume;

    public int MusicVolume
    {
        get { return musicVolume; }
        set
        {
            musicVolume = value;
            this.MusicMute(false);
        }
    }
    private int soundVolume;

    public int SoundVolume
    {
        get { return soundVolume; }
        set
        {
            soundVolume = value;
            this.SoundMute(false);
        }
    }
    public void MusicMute(bool mute)
    {
        this.SetVolume("MusicVolume", mute ? 0 : musicVolume);
    }

    public void SoundMute(bool mute)
    {
        this.SetVolume("SoundVolume", mute ? 0 : soundVolume);
    }

    private void SetVolume(string name, int value)
    {
        float volume = value * 0.5f - 50f;
        this.audioMixer.SetFloat(name, volume);
    }

    internal void PlayMusic(string name)
    {
        AudioClip clip = Resloader.Load<AudioClip>(MusicPath + name);
        if (clip == null)
        {
            Debug.LogWarningFormat("PlayMusic:{0} not existed",name);
        }

        if (musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
        }

        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }

    internal void PlaySound(string name)
    {
        AudioClip clip = Resloader.Load<AudioClip>(SoundPath + name);
        if (clip == null)
        {
            Debug.LogWarningFormat("PlayMusic:{0} not existed", name);
            return;
        }
        soundAudioSource.PlayOneShot(clip);
    }
}
