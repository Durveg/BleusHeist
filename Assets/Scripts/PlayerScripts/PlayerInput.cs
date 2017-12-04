using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
public class PlayerInput : MonoBehaviour {

	[SerializeField]
	private PlayerControllerData controllerData;
	
	void Update () {

		controllerData.inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if(Input.GetButtonDown("Jump"))
		{
			controllerData.InvokeJumpButtonDown();
		}

		if(Input.GetButtonUp("Jump"))
		{
			controllerData.InvkokeJumpButtonUp();
		}

		if(Input.GetButtonDown("Grapple"))
		{
			controllerData.InvokeGrappleButtonDown();
		}

		if(Input.GetButtonDown("MakeSound"))
		{
			controllerData.InvokeMakeSoundButtonDown();
		}

		if(Input.GetButtonDown("Interact"))
		{
			controllerData.InvokeInteractButtonEvent();
		}

		if(Input.GetButtonDown("DropItem"))
		{
			controllerData.InvokeDropItemButtonEvent();
		}
	}
}
