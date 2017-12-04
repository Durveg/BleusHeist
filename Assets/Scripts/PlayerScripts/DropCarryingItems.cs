using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCarryingItems : MonoBehaviour {

	[SerializeField]
	private PlayerControllerData controllerData;
	[SerializeField]
	private PlayerStats stats;
	[SerializeField]
	private GameObject jewelPrefab;

	private void DropItems ()
	{
		if(stats.jewelsCarrying > 0)
		{
			int droppedJewels = (stats.jewelsCarrying / 2);
			if(stats.jewelsCarrying % 2 != 0)
			{
				//if odd number take half of jewels + 1 to round up;
				droppedJewels += 1;
			}

			//Remove jewels from carrying.
			stats.jewelsCarrying -= droppedJewels;
			GameObject newJewel = GameObject.Instantiate(jewelPrefab, this.transform.position, Quaternion.identity);
			newJewel.GetComponent<Interactable>().SetValue(droppedJewels);
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
