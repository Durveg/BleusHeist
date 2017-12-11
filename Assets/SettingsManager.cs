﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour {

	[SerializeField]
	private Canvas settingsCanvas;

	[SerializeField]
	private AudioMixer masterMixer;
	[SerializeField]
	private VolumeControl volumeControl;

	[SerializeField]
	private Slider materSlider;

	[SerializeField]
	private Slider musicSlider;

	[SerializeField]
	private Slider soundFXSlider;

	[SerializeField]
	private Toggle muteToggle;

	void Awake()
	{
		this.CloseSettingsManager();

		materSlider.value = volumeControl.MasterVolumeScale;
		SetMasterVolume(volumeControl.MasterVolumeScale);

		musicSlider.value = volumeControl.MusicVolumeScale;
		SetMusicVolume(volumeControl.MusicVolumeScale);

		soundFXSlider.value = volumeControl.SoundFXVolumeScale;
		SetSoundFXVolume(volumeControl.SoundFXVolumeScale);

		muteToggle.isOn = volumeControl.MasterMuted;
		MuteToggled(volumeControl.MasterMuted);
	}

	public void CloseSettingsManager()
	{
		settingsCanvas.enabled = false;
	}

	public void OpenSettingsManager()
	{
		settingsCanvas.enabled = true;
	}

	public void SetMasterVolume(float percentVol)
	{
		volumeControl.MasterVolumeScale = percentVol;
		float dbVol = CalcDBVol(percentVol);
		masterMixer.SetFloat("MasterVolume", dbVol);
	}

	public void SetMusicVolume(float percentVol) 
	{
		volumeControl.MusicVolumeScale = percentVol;
		float dbVol = CalcDBVol(percentVol);
		masterMixer.SetFloat("MusicVolume", dbVol);
	}

	public void SetSoundFXVolume(float percentVol)
	{
		volumeControl.SoundFXVolumeScale = percentVol;
		float dbVol = CalcDBVol(percentVol);
		masterMixer.SetFloat("SoundFXVolume", dbVol);
	}

	public void MuteToggled(bool toggled)
	{
		volumeControl.MasterMuted = toggled;
		if(volumeControl.MasterMuted == true)
		{
			masterMixer.SetFloat("MasterVolume", -144f);
		} else
		{
			float dbVol = CalcDBVol(volumeControl.MasterVolumeScale);
			masterMixer.SetFloat("MasterVolume", dbVol);
		}
	}

	private float CalcDBVol(float linear)
	{
		float db = 0;
		if(linear != 0)
		{
			db = 20.0f * Mathf.Log10(linear);
		}

		if(db < -55 || linear == 0)
		{
			db = -144.0f;
		}

		return db;
	}
}