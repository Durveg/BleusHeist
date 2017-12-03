using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlayerControllerData : ScriptableObject {

	public delegate void InputButtonEvent();
	public event InputButtonEvent JumpButtonDown;
	public event InputButtonEvent JumpButtonUp;

	public event InputButtonEvent GrappleButtonDown;


	public delegate void MoveTowardsEvent(Vector2 targetPosition);
	public event MoveTowardsEvent MovePlayerGrapple;

	public bool onCeiling;

	public Vector2 inputDirection;
	public Vector2 playerVelocity;

	public void InvokeGrappleButtonDown()
	{
		if(GrappleButtonDown != null)
		{
			GrappleButtonDown.Invoke();
		}
	}

	public void InvokeJumpButtonDown()
	{
		if(JumpButtonDown != null)
		{
			JumpButtonDown.Invoke();
		}
	}

	public void InvkokeJumpButtonUp()
	{
		if(JumpButtonUp != null)
		{
			JumpButtonUp.Invoke();
		}
	}

	public void InvokeMovePlayerGrapple(Vector2 targetPosition)
	{
		if(MovePlayerGrapple != null)
		{
			MovePlayerGrapple.Invoke(targetPosition);
		}
	}
}
