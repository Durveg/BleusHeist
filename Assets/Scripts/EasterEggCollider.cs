using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggCollider : MonoBehaviour {
	[SerializeField]
	private CameraFollow cameraFollow;

	private Vector2 cameraNormalBounds;
	[SerializeField]
	private Vector2 cameraEasterEggBounds;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.transform.tag == "Player")
		{
			cameraNormalBounds = cameraFollow.SetEasterEggBounds(cameraEasterEggBounds);
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
