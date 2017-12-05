using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectManager : MonoBehaviour {

	[SerializeField]
	private GameSettings gameSettings;
	[SerializeField]
	private PlayerStats playerStats;

	public float GameTimeInSeconds
	{
		get { return gameSettings.gameTime; }
	}

	void Awake () {

		playerStats.ResetValues();
		gameSettings.ResetValues();
	}

	void Update()
	{
		if(gameSettings.gameIsOver == false)
		{
			gameSettings.gameTime += Time.deltaTime;
		}
	}
}
