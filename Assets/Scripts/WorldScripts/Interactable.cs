using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour {

	public int value = 1;

	[SerializeField]
	private TextMeshPro text;

	void OnEnable()
	{
		SetValue(value);
	}

	public void SetValue(int value)
	{
		this.value = value;
		text.text = this.value.ToString();
	}
}
