using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour {

	[SerializeField]
	private PlayerControllerData controllerData;
	[SerializeField]
	private Canvas canvas;

	void Start()
	{
		canvas.enabled = false;
	}

	void OnEnable()
	{
		controllerData.PauseGameButtonEvent += GamePaused;
	}

	void OnDisable()
	{
		controllerData.PauseGameButtonEvent -= GamePaused;
	}

	private void GamePaused()
	{
		canvas.enabled = true;	
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

	public void ResumeGameClicked()
	{
		canvas.enabled = false;
		controllerData.UnPauseGame();
	}
}
