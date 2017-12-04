using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnInZone : MonoBehaviour {

	[SerializeField]
	private PlayerStats playerStats;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.transform.tag == "Player")
		{
			playerStats.TurnInJewels();
		}
	}
}
