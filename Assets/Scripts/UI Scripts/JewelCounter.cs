using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JewelCounter : MonoBehaviour {

	[SerializeField]
	private PlayerStats playerStats;
	[SerializeField]
	private GameSettings gameSettings;

	[SerializeField]
	private TextMeshProUGUI jewelCounter;
	[SerializeField]
	private TextMeshProUGUI timer;

	void Update () {

		float minutes = Mathf.Floor(gameSettings.gameTime / 60); 
		float seconds = Mathf.RoundToInt(gameSettings.gameTime % 60);

		string sMinutes = minutes.ToString();
		string sSeconds = Mathf.RoundToInt(seconds).ToString();
		if(minutes < 10) 
		{ 
			sMinutes = "0" + sMinutes; 
		} 
		if(seconds < 10) 
		{ 
			sSeconds = "0" + sSeconds;
		} 

		timer.text = sMinutes + ":" + sSeconds;

		//TODO: make this only change when the value changes instead of every frame.
		jewelCounter.text = playerStats.JewelValue.ToString();
	}
}
