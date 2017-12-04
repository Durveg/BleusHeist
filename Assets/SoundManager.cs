using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance;

	void Awake () {

		if(instance == null)
		{
			instance = this;
		} else
		{
			GameObject.Destroy(this.gameObject);
		}

		GameObject.DontDestroyOnLoad(this.gameObject);
	}

	[SerializeField]
	private AudioSource PickUpJewelSound;
	[SerializeField]
	private AudioSource EnemyLOSSound;
	[SerializeField]
	private AudioSource JumpSound;
	[SerializeField]
	private AudioSource GrappleSound;
	[SerializeField]
	private AudioSource FootSetSound;

	public void PlayFootStepSound()
	{
		FootSetSound.Play();
	}

	public void PlayGrappleSound()
	{
		GrappleSound.Play();
	}

	public void PlayJumpSound()
	{
		JumpSound.Play();
	}

	public void PlayPickUpJewel()
	{
		PickUpJewelSound.Play();
	}

	private bool enemyLOSCD = false;
	public void PlayEnemyLOS()
	{
		if(enemyLOSCD == false)
		{
			//TODO:
			EnemyLOSSound.Play();
			StartCoroutine(CoolDownEnemyLOSSound());
		}
	}

	private IEnumerator CoolDownEnemyLOSSound()
	{
		enemyLOSCD = true;
		yield return new WaitForSeconds(0.5f);
		enemyLOSCD = false;
	}
}
