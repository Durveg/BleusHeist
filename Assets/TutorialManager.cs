using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

	[SerializeField]
	private Canvas[] canvases;
	private int index = 0;

	void Start()
	{
		CloseTutorial();
	}

	public void OpenTutorial()
	{
		index = 0;
		canvases[index].enabled = true;
	}

	public void CloseTutorial()
	{
		foreach(Canvas tutCanvas in this.canvases)
		{
			tutCanvas.enabled = false;
		}
	}

	public void NextWindow()
	{
		index++;
		if(index < canvases.Length)
		{
			canvases[index - 1].enabled = false;
			canvases[index].enabled = true;
		} else
		{
			CloseTutorial();
		}
	}

	public void PrevWindow()
	{
		index--;
		if(index >= 0)
		{
			if(canvases.Length > index + 1)
			{
				canvases[index + 1].enabled = false;
			}
			canvases[index].enabled = true;
		} else
		{
			CloseTutorial();
		}
	}

}
