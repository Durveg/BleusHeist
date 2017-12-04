using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour {

	[SerializeField]
	private GameObject quitButton;

	void OnEnable()
	{
		#if UNITY_WEBGL
		quitButton.SetActive(false);
		#endif
	}

	public void PlayButtonClicked()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);		
	}

	public void TutorialClicked()
	{
//		SceneManager.LoadScene(0);
	}

	public void QuitGameClicked()
	{
		Application.Quit();
	}

}
