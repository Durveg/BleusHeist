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

	private IEnumerator LookTowardsCoRoutine;

	public void LookAtPosition(Vector2 position)
	{
		Vector2 dir = (position - (Vector2)this.transform.position).normalized;
		float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

		targetAngle = Mathf.Clamp(targetAngle, -maxLookAngle, maxLookAngle);

		if(LookTowardsCoRoutine != null)
		{
			StopCoroutine(LookTowardsCoRoutine);
		}

		LookTowardsCoRoutine = LookTowards(targetAngle);
		StartCoroutine(LookTowardsCoRoutine);
	}

	private IEnumerator LookTowards(float targetAngle)
	{
		while(Mathf.Abs(Mathf.DeltaAngle(lookAtArea.transform.eulerAngles.z, targetAngle)) > 0.05f)
		{
			float angle = Mathf.MoveTowardsAngle(lookAtArea.transform.eulerAngles.z, targetAngle, lookSpeed * Time.deltaTime);
			lookAtArea.transform.eulerAngles = Vector3.forward * angle;

			yield return null;
		}

		LookTowardsCoRoutine = null;
	}
}
