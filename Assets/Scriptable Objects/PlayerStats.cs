﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlayerStats : ScriptableObject {

	[SerializeField]
	private GameSettings gameSettings;

	public Stack<Interactable> jewelsPickedUp;
	private List<Interactable> registeredInteractables;

	PlayerStats()
	{
		jewelsPickedUp = new Stack<Interactable>();
		jewelsPickedUp.Clear();

		registeredInteractables = new List<Interactable>();
		JewelValue = 0;
	}

	public int JewelValue;
	public int returnedJewels;
	public int totalJewels;
	public int timesPlayerCaught;

	public void ReturnedJewel()
	{
		returnedJewels++;
	}

	public void RegisterJewel(Interactable jewel)
	{
		if(registeredInteractables.Contains(jewel) == false)
		{
			registeredInteractables.Add(jewel);
			totalJewels = registeredInteractables.Count;
		}
	}

	public void PickedUpJewel(Interactable pickedUp)
	{
		SoundManager.instance.PlayPickUpJewel();
		JewelValue += pickedUp.value;
		jewelsPickedUp.Push(pickedUp);
	}

	public Interactable DroppedJewel()
	{
		Interactable jewelDropped = null;
		if(jewelsPickedUp.Count > 0)
		{
			jewelDropped = jewelsPickedUp.Pop();
			JewelValue -= jewelDropped.value;
		}

		return jewelDropped;
	}

	public void TurnInJewels()
	{
		int turnedIn = this.jewelsPickedUp.Count;
		this.jewelsPickedUp.Clear();
		this.JewelValue = 0;
		this.returnedJewels += turnedIn;

		if(returnedJewels >= totalJewels)
		{
			//WIN!
			gameSettings.InvokeGameWonEvent();
		}
	}

	public void ResetValues()
	{
		JewelValue = 0;
		returnedJewels = 0;

		totalJewels = 0;
		if(registeredInteractables != null)
		{
			registeredInteractables.Clear();
		}
	}
}