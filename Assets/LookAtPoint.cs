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
	[SerializeField]
	private EnemyMovement thisEnemy;

	public void LookAtPosition(Vector2 position)
	{
		

//		if(thisEnemy.controller.collisions.faceDir == -1)
//		{
//			targetAngle += 180;
//		}
//
//
//
//		if(LookTowardsCoRoutine != null)
//		{
//			StopCoroutine(LookTowardsCoRoutine);
//		}
//
//		LookTowardsCoRoutine = LookTowards(targetAngle);
//		StartCoroutine(LookTowardsCoRoutine);
	}

	public void UpdateLookAt(Vector2 position)
	{
		Vector2 dir = (position - (Vector2)this.transform.position).normalized;
		float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

		targetAngle = ClampAngle(targetAngle);
		Debug.Log("Target angle before Clamp: " + targetAngle);

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

	private IEnumerator LookTowards(float targetAngle)
	{
		while(Mathf.Abs(Mathf.DeltaAngle(lookAtArea.transform.eulerAngles.z, targetAngle)) > 0.05f)
		{
			
			yield return null;
		}

		LookTowardsCoRoutine = null;
	}
}
