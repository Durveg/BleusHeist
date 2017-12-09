using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour {

	[SerializeField]
	private AudioMixer masterMixer;

	public void SetMasterVolume(float percentVol)
	{
		float dbVol = CalcDBVol(percentVol);
		masterMixer.SetFloat("MasterVolume", dbVol);
	}

	public void SetMusicVolume(float percentVol) 
	{
		float dbVol = CalcDBVol(percentVol);
		masterMixer.SetFloat("MusicVolume", dbVol);
	}

	public void SetSoundFXVolume(float percentVol)
	{
		float dbVol = CalcDBVol(percentVol);
		masterMixer.SetFloat("SoundFXVolume", dbVol);
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
