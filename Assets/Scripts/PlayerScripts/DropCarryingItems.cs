using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCarryingItems : MonoBehaviour {

	[SerializeField]
	private PlayerControllerData controllerData;
	[SerializeField]
	private PlayerStats stats;

	private void DropItems ()
	{
		Interactable dropped = stats.DroppedJewel();
		if(dropped != null)
		{
			dropped.gameObject.SetActive(true);
			dropped.transform.position = this.transform.position;
		}
	}

	void OnEnable()
	{
		controllerData.DropItemButtonEvent += DropItems;
	}
		
	void OnDisable()
	{
		controllerData.DropItemButtonEvent -= DropItems;
	}
}
