using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlayerStats : ScriptableObject {

	[SerializeField]
	private const int jewelsCarryingDefaultValue = 0;

	public Stack<Interactable> jewelsPickedUp;

	PlayerStats()
	{
		jewelsPickedUp = new Stack<Interactable>();
		JewelValue = 0;
	}

	public int JewelValue;

	public void PickedUpJewel(Interactable pickedUp)
	{
		JewelValue += pickedUp.value;
		jewelsPickedUp.Push(pickedUp);
	}

	public Interactable DroppedJewel()
	{
		Interactable jewelDropped = null;
		if(jewelsPickedUp.Peek() != null)
		{
			jewelDropped = jewelsPickedUp.Pop();
			JewelValue -= jewelDropped.value;
		}

		return jewelDropped;
	}
}