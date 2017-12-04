using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPoint : MonoBehaviour {

	[SerializeField]
	private float maxLookAngle;
	[SerializeField]
	private float lookSpeed;
	[SerializeField]
	private GameObject lookAtArea;

	public void UpdateLookAt(Vector2 position)
	{
		Vector2 dir = (position - (Vector2)this.transform.position).normalized;
		float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

		targetAngle = ClampAngle(targetAngle);

		float angle = Mathf.MoveTowardsAngle(lookAtArea.transform.eulerAngles.z, targetAngle, lookSpeed * Time.deltaTime);
		lookAtArea.transform.eulerAngles = Vector3.forward * angle;
	}

	public static float ClampAngle(float angle) {
		if(angle < 0f)
			return angle + (360f * (int) ((angle / 360f) + 1));
		else if(angle > 360f)
			return angle - (360f * (int) (angle / 360f));
		else
			return angle;
	}
}
