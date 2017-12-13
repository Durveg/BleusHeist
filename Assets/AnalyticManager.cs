using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticManager : MonoBehaviour {

	public int stashedGems = 0;
	public int heldGems = 0;
	public int caughtTimes = 0;

	[SerializeField]
	private AnalyticsTracker GameQuitEvent;
	[SerializeField]
	private AnalyticsTracker PlayerCaughtEvent;

	[SerializeField]
	private PlayerStats playerStats;
	[SerializeField]
	private GameSettings gameSettings;

	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		gameSettings.GameOverEvent += OnPlayerCaught;
	}

	void OnApplicationQuit()
	{
		UpdateValues();
		GameQuitEvent.TriggerEvent();
	}

	void OnPlayerCaught()
	{
		UpdateValues();
//		PlayerCaughtEvent.TriggerEvent();
	}

	private void UpdateValues()
	{
		stashedGems = playerStats.returnedJewels;
		heldGems = playerStats.jewelsPickedUp.Count;
		caughtTimes = playerStats.timesPlayerCaught;
	}
}