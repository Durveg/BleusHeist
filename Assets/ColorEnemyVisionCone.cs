﻿using System.Collections;
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

	private new SpriteRenderer renderer;

	void Start()
	{
		renderer = this.GetComponent<SpriteRenderer>();
		this.SetSpriteColor(lookingColor);
	}

	void OnEnable()
	{
		renderer = this.GetComponent<SpriteRenderer>();
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if(other.transform.tag == "Player")
		{
			this.CheckLOS(other);
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(other.transform.tag == "Player")
		{
			this.CheckLOS(other);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.transform.tag == "Player")
		{
			this.SetSpriteColor(lookingColor);
		}
	}

	private void CheckLOS(Collider2D other)
	{
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
			this.SetSpriteColor(seenColor);
		} else
		{
			this.SetSpriteColor(lookingColor);
		}
	}

	private void SetSpriteColor(Color color)
	{
		this.renderer.color = color;
	}
}
