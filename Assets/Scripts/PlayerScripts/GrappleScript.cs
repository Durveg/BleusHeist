using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrappleScript : MonoBehaviour {

	[SerializeField]
	private float distance;
	[SerializeField]
	private LayerMask layerMask;
	[SerializeField]
	private PlayerControllerData controllerData;
	private LineRenderer lineRenderer;

	private void GrappleFired()
	{
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.up, distance, layerMask);
		Debug.DrawRay(this.transform.position, Vector2.up * distance, Color.gray, 2);

		if(hit.collider != null)
		{
			StartCoroutine(AnimateGrapple(hit.point));
		}
	}

	void OnEnable()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.positionCount = 0;
		controllerData.GrappleButtonDown += GrappleFired;
	}

	void OnDisable()
	{
		controllerData.GrappleButtonDown -= GrappleFired;
	}

	private Vector2 calcInitalVelocity(Vector2 startPos, Vector2 endPos, float time)
	{
		Vector2 initalVelocity = Vector2.zero;

		initalVelocity.x = -1 * Mathf.Sign(controllerData.playerVelocity.x) * Mathf.Sqrt(Mathf.Pow(controllerData.playerVelocity.x, 2) - 2 * 0 * (endPos.x - startPos.x));
		initalVelocity.y = Mathf.Sqrt(Mathf.Pow(controllerData.playerVelocity.y, 2) - 2 * Controller2D.gravity * (endPos.y - startPos.y));

		return initalVelocity;
	}

	private IEnumerator AnimateGrapple(Vector2 targetPosition)
	{
		
		lineRenderer.positionCount = 2;

		Vector3[] positions = new Vector3[2];
		float animateTime = 0.15f;

		float timer = animateTime;
		float step = 0;
		while(timer > 0)
		{
			step += Time.deltaTime/animateTime;
			positions[0] = this.transform.localPosition;


			Vector3 localPosition = transform.TransformPoint(positions[1]);
			positions[1] = transform.InverseTransformPoint(Vector2.MoveTowards(localPosition, targetPosition, step));
			lineRenderer.SetPositions(positions);

			timer -= Time.deltaTime;
			yield return null;
		}

		Vector2 start = this.transform.position;
		Vector2 vel = calcInitalVelocity(start, targetPosition, animateTime);
		controllerData.InvokeMovePlayerGrapple(vel);

		while(controllerData.onCeiling == false)
		{
			positions[0] = this.transform.localPosition;
			positions[1] = transform.InverseTransformPoint(targetPosition);

			lineRenderer.SetPositions(positions);
			yield return null;
		}

		lineRenderer.positionCount = 0;
	}
}
