﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerNoiseManager : MonoBehaviour {

	public delegate void SoundMade(Vector2 soundPosition);
	public event SoundMade soundMadeEvent;

	[SerializeField]
	private PlayerControllerData controllerData;
	[SerializeField]
	private PlayerStats playerStats;
	[SerializeField]
	private GameSettings gameSettings;

	[SerializeField]
	private float baseScale = 0.5f;
	[SerializeField]
	private float constantScaler = 1;
	private int jewels = 0;

	[SerializeField]
	private float noiseRate = 0.5f;
	private float noiseTimer = 0;
	private float noiseEventTimer = 0;

	[SerializeField]
	private GameObject expandCircle;

	void OnEnable()
	{
		controllerData.MakeSoundButtonDown += MakeSoundInRadius;
		this.expandCircle.transform.localScale = Vector3.zero;
		UpdateSoundArea();
	}

	private void MakeSoundInRadius()
	{
		if(soundMadeEvent != null)
		{
			soundMadeEvent.Invoke(this.transform.position);
		}
	}

	private void UpdateSoundArea()
	{
		float scale = baseScale;
		if(jewels > 0)
		{
			scale += (constantScaler * Mathf.Sqrt(jewels));
		}

		this.transform.localScale = new Vector3(scale, scale, 1);
	}

	void OnDisable()
	{
		controllerData.MakeSoundButtonDown -= MakeSoundInRadius;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.transform.tag == "Enemy")
		{
			other.GetComponent<EnemySoundListener>().InRange(this);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.transform.tag == "Enemy")
		{
			other.GetComponent<EnemySoundListener>().OutOfRange(this);
		}
	}

	void Update()
	{
		if(gameSettings.gameIsOver == true)
		{
			return;
		}

		if(this.jewels != playerStats.JewelValue)
		{
			this.jewels = playerStats.JewelValue;
			this.UpdateSoundArea();
		}


		//if the player is moving horizontally or if player is moving up while holding the up button
		if((controllerData.playerVelocity.x > 0.05f && controllerData.inputDirection.x > 0) ||
		   (controllerData.playerVelocity.x < -0.05f && controllerData.inputDirection.x < 0) ||
		   (controllerData.playerVelocity.y > 0.05f && controllerData.inputDirection.y > 0) ||
			(controllerData.playerVelocity.y < -0.05f && controllerData.inputDirection.y < 0 && controllerData.isSliding == true))
		{
			this.noiseTimer += Time.deltaTime;
			this.noiseEventTimer += Time.deltaTime;

			if(this.noiseEventTimer > this.noiseRate / 2)
			{
				this.noiseEventTimer = 0;
				MakeSoundInRadius();
			}

			if(this.noiseTimer > this.noiseRate)
			{
				this.noiseTimer = 0;
				SoundManager.instance.PlayFootStepSound();
			}
		}
	}

	private IEnumerator ExpandNoiseCircle()
	{
		float expandTime = 0.1f;
		float timer = 0;
		float scale = 0;
		float endScale = 1;
		while(timer < expandTime)
		{
			

			scale = Mathf.MoveTowards(scale, endScale, (1 / expandTime) * Time.deltaTime);
			this.expandCircle.transform.localScale = new Vector3(scale, scale, 1);

			timer += Time.deltaTime;
			yield return null;
		}

		this.expandCircle.transform.localScale = Vector3.zero;
		MakeSoundInRadius();
	}
}
