using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class EasterEggCollider : MonoBehaviour {
	[SerializeField]
	private CameraFollow cameraFollow;

	private Vector2 cameraNormalBounds;
	[SerializeField]
	private Vector2 cameraEasterEggBounds = new Vector2(12.25f, 100);

	[SerializeField]
	private AnalyticsTracker playerFoundEasterEgg;

	private bool eggFound = false;

	void OnEnable()
	{
		cameraFollow = Camera.main.GetComponent<CameraFollow>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.transform.tag == "Player")
		{
			cameraNormalBounds = cameraFollow.SetEasterEggBounds(cameraEasterEggBounds);

			if(eggFound == false)
			{
				eggFound = true;
				playerFoundEasterEgg.TriggerEvent();
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.transform.tag == "Player")
		{
			cameraFollow.SetNormalBounds(cameraNormalBounds);
		}
	}
}
