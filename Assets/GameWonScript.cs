using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Analytics;

public class GameWonScript : MonoBehaviour {

	[SerializeField]
	private GameSettings gameSettings;
	[SerializeField]
	private PlayerStats playerStats;

	[SerializeField]
	private Canvas canvas;

	[SerializeField]
	private TextMeshProUGUI timeText;

	[SerializeField]
	private AnalyticsTracker playerWonEvent;

	[SerializeField]
	private GameObject quitButton;

	[SerializeField]
	private GameObject[] UIElements;

	private void GameWon()
	{
		playerWonEvent.TriggerEvent();

		canvas.enabled = true;
		foreach(GameObject obj in UIElements)
		{
			obj.SetActive(true);
		}

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

		timeText.text = sMinutes + ":" + sSeconds;
	}

	void OnEnable()
	{
		canvas.enabled = false;
		foreach(GameObject obj in UIElements)
		{
			obj.SetActive(false);
		}

		gameSettings.GameWonEvent += GameWon;


		#if UNITY_WEBGL
		quitButton.SetActive(false);
		#endif
	}

	void OnDisable()
	{
		gameSettings.GameWonEvent -= GameWon;
	}

	public void ResetButtonClicked()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);		
	}

	public void MainMenuClicked()
	{
		SceneManager.LoadScene(0);
	}

	public void QuitGameClicked()
	{
		Application.Quit();
	}
}
