using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GameSettings : ScriptableObject {

	GameSettings()
	{
		gameIsOver = false;
		difficulty = GameDifficulty.Easy;
	}

	public delegate void GlobalEventTriggered();
	public event GlobalEventTriggered GameOverEvent;
	public event GlobalEventTriggered GameWonEvent;

	public void InvokeGameWonEvent()
	{
		gameIsOver = true;
		if(GameWonEvent != null)
		{
			GameWonEvent.Invoke();
		}
	}

	public void InvokeGameOverEvent()
	{
		gameIsOver = true;

		if(GameOverEvent != null)
		{
			GameOverEvent.Invoke();
		}
	}

	public void ResetValues()
	{
//		difficulty = GameDifficulty.Easy;
		gameIsOver = false;
		gameTime = 0;
	}

	public GameDifficulty difficulty = GameDifficulty.Easy;
	public bool gameIsOver = false;

	public float[] caughtTimeSettings;
	public float gameTime;
}

public enum GameDifficulty
{
	Easy,
	Medium,
	Hard,
	Brutal
}