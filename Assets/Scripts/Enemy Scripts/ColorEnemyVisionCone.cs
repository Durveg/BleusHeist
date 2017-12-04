using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(Collider2D))]
public class ColorEnemyVisionCone : MonoBehaviour {

	[SerializeField]
	private Color seenColor;
	[SerializeField]
	private Color lookingColor;
	[SerializeField]
	private LayerMask checkHitLayers;
	[SerializeField]
	private GameObject enemyGameObject;
	[SerializeField]
	private GameSettings gameSettings;

	private new SpriteRenderer renderer;

	private float timeInVision = 0;
	[SerializeField]
	private float maxTimeBeforeCaught = 3;

	private Collider2D playerCollider;

	private bool hasPlayedSound = false;

	void Start()
	{
		renderer = this.GetComponent<SpriteRenderer>();
		this.SetSpriteColor(lookingColor);

		switch(gameSettings.difficulty)
		{
		case(GameDifficulty.Easy):
			maxTimeBeforeCaught = gameSettings.caughtTimeSettings[0];
			break;
		case(GameDifficulty.Medium):
			maxTimeBeforeCaught = gameSettings.caughtTimeSettings[1];
			break;
		case(GameDifficulty.Hard):
			maxTimeBeforeCaught = gameSettings.caughtTimeSettings[2];
			break;
		case(GameDifficulty.Brutal):
			maxTimeBeforeCaught = gameSettings.caughtTimeSettings[2];
			break;
		}
	}

	void OnEnable()
	{
		renderer = this.GetComponent<SpriteRenderer>();
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if(other.transform.tag == "Player")
		{
			this.playerCollider = other;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.transform.tag == "Player")
		{
			hasPlayedSound = false;
			this.timeInVision = 0;
			this.playerCollider = null;
		}
	}

	void Update()
	{
		if(this.playerCollider != null)
		{
			CheckLOS(this.playerCollider);
		} else
		{
			this.SetSpriteColor(lookingColor);
		}
	}

	private void CheckLOS(Collider2D other)
	{
		if(gameSettings.gameIsOver == true)
		{
			return;
		}

		bool playerIsInLOS = false;

		Vector2 direction = other.transform.position - this.transform.position;
		RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, direction, direction.magnitude, checkHitLayers);
		Debug.DrawRay(this.transform.position, direction, Color.red);

		foreach(RaycastHit2D hit in hits)
		{
			if(hit.collider != null && hit.collider.transform.gameObject != enemyGameObject)
			{
				if(hit.collider != null && (hit.collider.gameObject.layer == 15 || hit.collider.gameObject.layer == 16))
				{
					break;
				}

				if(hit.collider != null && hit.collider.transform.tag == "Player")
				{
					playerIsInLOS = true;
					break;
				}
			}
			
		}

		if(playerIsInLOS == true)
		{
			if(this.hasPlayedSound == false)
			{
				this.hasPlayedSound = true;
				SoundManager.instance.PlayEnemyLOS();
			}

			this.timeInVision += Time.deltaTime;

			//TODO: Set inidcator that time is running out to get away.
			this.SetSpriteColor(seenColor);

			if(this.timeInVision > maxTimeBeforeCaught)
			{
				gameSettings.InvokeGameOverEvent();
			}
		} else
		{
			hasPlayedSound = false;
			this.timeInVision = 0;
			this.SetSpriteColor(lookingColor);
		}
	}

	private void SetSpriteColor(Color color)
	{
		this.renderer.color = color;
	}
}
