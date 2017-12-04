using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JewelCounter : MonoBehaviour {

	[SerializeField]
	private PlayerStats playerStats;
	[SerializeField]
	private TextMeshProUGUI jewelCounter;
	
	void Update () {

		//TODO: make this only change when the value changes instead of every frame.
		jewelCounter.text = playerStats.jewelsCarrying.ToString();
	}
}
