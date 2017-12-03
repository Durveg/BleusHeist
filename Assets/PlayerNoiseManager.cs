using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerNoiseManager : MonoBehaviour {

	public delegate void SoundMade(Vector2 soundPosition);
	public event SoundMade soundMadeEvent;

	[SerializeField]
	private PlayerControllerData controllerData;

	void OnEnable()
	{
		controllerData.MakeSoundButtonDown += MakeSoundInRadius;
	}

	private void MakeSoundInRadius()
	{
		if(soundMadeEvent != null)
		{
			soundMadeEvent.Invoke(this.transform.position);
		}
	}

	void OnDisable()
	{
		controllerData.MakeSoundButtonDown -= MakeSoundInRadius;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.transform.tag == "Enemy")
		{
			other.GetComponent<EnemySoundListener>().InRange(this);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.transform.tag == "Enemies")
		{
			other.GetComponent<EnemySoundListener>().OutOfRange(this);
		}
	}
}
