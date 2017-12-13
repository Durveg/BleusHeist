using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenu : MonoBehaviour {

	[SerializeField]
	private GameSettings gameSettings;
	[SerializeField]
	private PlayerStats playerStats;

	[SerializeField]
	private GameObject quitButton;


	[SerializeField]
	private Canvas canvas;

	[SerializeField]
	private TextMeshProUGUI timeText;
	[SerializeField]
	private TextMeshProUGUI jewelsText;

	[SerializeField]
	private GameObject[] UIElements;

	private void GameOver()
	{
		canvas.enabled = true;
		foreach(GameObject obj in UIElements)
		{
			obj.SetActive(true);
		}

		playerStats.timesPlayerCaught++;

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
		jewelsText.text = playerStats.returnedJewels.ToString();
	}

	void OnEnable()
	{
		canvas.enabled = false;
		foreach(GameObject obj in UIElements)
		{
			obj.SetActive(false);
		}

		gameSettings.GameOverEvent += GameOver;

		#if UNITY_WEBGL
		quitButton.SetActive(false);
		#endif
	}

	void OnDisable()
	{
		gameSettings.GameOverEvent -= GameOver;
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
