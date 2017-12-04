using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerNoiseManager : MonoBehaviour {

	public delegate void SoundMade(Vector2 soundPosition);
	public event SoundMade soundMadeEvent;

	[SerializeField]
	private PlayerControllerData controllerData;
	[SerializeField]
	private PlayerStats playerStats;

	[SerializeField]
	private float baseScale = 0.5f;
	[SerializeField]
	private float constantScaler = 1;
	private int jewels = 0;

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

	private void UpdateSoundArea()
	{
		float scale = baseScale;
		if(jewels > 0)
		{
			scale += (constantScaler * Mathf.Sqrt(jewels));
		}

		this.transform.localScale = new Vector3(scale, scale, 1);
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
		if(other.transform.tag == "Enemy")
		{
			other.GetComponent<EnemySoundListener>().OutOfRange(this);
		}
	}

	void Update()
	{
		if(this.jewels != playerStats.jewelsCarrying)
		{
			this.jewels = playerStats.jewelsCarrying;
			this.UpdateSoundArea();
		}
	}
}
