using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlayerControllerData : ScriptableObject {

	public delegate void InputButtonEvent();
	public event InputButtonEvent JumpButtonDown;
	public event InputButtonEvent JumpButtonUp;

	public Vector2 inputDirection;

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
}
