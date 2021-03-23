﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISystemConfig : UIWindow
{

    public Image musicOff;

    public Image soundOff;

    public Toggle toggleMusic;

    public Toggle toggleSound;

    public Slider sliderMusic;

    public Slider sliderSound;

	// Use this for initialization
	void Start ()
    {
        this.toggleMusic.isOn = Config.MusicOn;
        this.toggleSound.isOn = Config.SoundOn;
        this.sliderMusic.value = Config.MusicVolume;
        this.sliderSound.value = Config.SoundVolume;
        toggleMusic.onValueChanged.AddListener(MusicToggle);
        toggleSound.onValueChanged.AddListener(SoundToggle);
        sliderMusic.onValueChanged.AddListener(MusicVolume);
        sliderSound.onValueChanged.AddListener(SoundVolume);
        MusicToggle(this.toggleMusic.isOn);
        SoundToggle(this.toggleSound.isOn);
    }

    public override void OnYesClick()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        PlayerPrefs.Save();
        base.OnYesClick();
    }

    public void MusicToggle(bool on)
    {
        musicOff.enabled = !on;
        Config.MusicOn = on;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }

    public void SoundToggle(bool on)
    {
        soundOff.enabled = !on;
        Config.SoundOn = on;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }

    public void MusicVolume(float vol)
    {
        Config.MusicVolume = (int) vol;
        PlaySound();
    }

    public void SoundVolume(float vol)
    {
        Config.SoundVolume = (int) vol;
        PlaySound();
    }

    private float lastPlay = 0;

    private void PlaySound()
    {
        if (Time.realtimeSinceStartup - lastPlay > 0.1f)
        {
            lastPlay = Time.realtimeSinceStartup;
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        }
    }
}
