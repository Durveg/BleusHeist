using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteractScript : MonoBehaviour {

	[SerializeField]
	private PlayerStats stats;
	[SerializeField]
	private PlayerControllerData controller;

	private Dictionary<Collider2D, Interactable> interactablesInRange;

	// Use this for initialization
	void Start () {

		interactablesInRange = new Dictionary<Collider2D, Interactable>();
	}

	void OnEnable()
	{
		controller.InteractButtonEvent += PlayerInteracted;
	}

	void OnDisable()
	{
		controller.InteractButtonEvent -= PlayerInteracted;
	}

	private void PlayerInteracted()
	{
		if(interactablesInRange.Count > 0)
		{
			Interactable[] interactables = new Interactable[interactablesInRange.Count];
			Collider2D[] colliders = new Collider2D[interactablesInRange.Count];
			interactablesInRange.Values.CopyTo(interactables, 0);
			interactablesInRange.Keys.CopyTo(colliders, 0);

			for(int i = 0; i < interactables.Length; i++)
			{
				
				stats.PickedUpJewel(interactables[i]);
				interactables[i].gameObject.SetActive(false);

				interactablesInRange.Remove(colliders[i]);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Interactable")
		{
			interactablesInRange.Add(other, other.GetComponent<Interactable>());
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.tag == "Interactable")
		{
			interactablesInRange.Remove(other);
		}
	}
}
