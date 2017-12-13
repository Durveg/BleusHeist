using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour {

	[SerializeField]
	private PlayerControllerData controllerData;
	[SerializeField]
	private Canvas canvas;

	[SerializeField]
	private GameObject quitButton;

	[SerializeField]
	private GameObject[] UIElements;

	void Start()
	{
		DisableUI();
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
		foreach(GameObject obj in UIElements)
		{
			obj.SetActive(true);
		}

		#if UNITY_WEBGL
		quitButton.SetActive(false);
		#endif
	}

	private void DisableUI()
	{
		canvas.enabled = false;
		foreach(GameObject obj in UIElements)
		{
			obj.SetActive(false);
		}
	}

	public void ResetButtonClicked()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);		
	}

	public void MainMenuClicked()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(0);
	}

	public void QuitGameClicked()
	{
		Application.Quit();
	}

	public void ResumeGameClicked()
	{
		DisableUI();
		controllerData.UnPauseGame();
	}
}
