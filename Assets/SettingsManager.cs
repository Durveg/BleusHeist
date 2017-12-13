using System.Collections;
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

	[SerializeField]
	private GameObject[] UIElements;

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
		foreach(GameObject obj in UIElements)
		{
			obj.SetActive(false);
		}
	}

	public void OpenSettingsManager()
	{
		settingsCanvas.enabled = true;
		foreach(GameObject obj in UIElements)
		{
			obj.SetActive(true);
		}
	}
		
	public void SetMasterVolume(float percentVol)
	{
		volumeControl.MasterVolumeScale = percentVol;
		if(volumeControl.MasterMuted == false)
		{
			float dbVol = CalcDBVol(percentVol);
			masterMixer.SetFloat("MasterVolume", dbVol);
		}
	}

	public void SetMusicVolume(float percentVol) 
	{
		volumeControl.MusicVolumeScale = percentVol;
		if(volumeControl.MasterMuted == false)
		{
			float dbVol = CalcDBVol(percentVol);
			masterMixer.SetFloat("MusicVolume", dbVol);
		}
	}

	public void SetSoundFXVolume(float percentVol)
	{
		volumeControl.SoundFXVolumeScale = percentVol;

		if(volumeControl.MasterMuted == false)
		{
			float dbVol = CalcDBVol(percentVol);
			masterMixer.SetFloat("SoundFXVolume", dbVol);
		}
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
			db = 30.0f * Mathf.Log10(linear);
		}

		if(db < -55 || linear == 0)
		{
			db = -144.0f;
		}

		return db;
	}
}
