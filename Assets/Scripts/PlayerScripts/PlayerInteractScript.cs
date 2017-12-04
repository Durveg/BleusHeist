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
		stats.ResetStats();
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
			interactablesInRange.Values.CopyTo(interactables, 0);

			foreach(Interactable interactable in interactables)
			{
				stats.jewelsCarrying += interactable.value;
				GameObject.Destroy(interactable.gameObject);
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
