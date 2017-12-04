using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundListener : MonoBehaviour {

	[SerializeField]
	private EnemyMovement movementManager;

	private void SoundMadeInRange (Vector2 soundPosition)
	{
		movementManager.SetInterestPoint(soundPosition);
	}

	public void InRange(PlayerNoiseManager nm)
	{
		nm.soundMadeEvent += SoundMadeInRange;
	}

	public void OutOfRange(PlayerNoiseManager nm)
	{
		nm.soundMadeEvent -= SoundMadeInRange;
	}
}
