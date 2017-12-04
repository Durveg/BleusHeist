using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class EnemyMovement : MonoBehaviour {

	[SerializeField]
	private float moveSpeed = 4;
	private float velocityXsmoothing;
	[SerializeField]
	private float accelerationTimeGrounded = .1f;
	[SerializeField]
	private float accelerationTimeAirborne = .2f;

	public Controller2D controller;
	private Vector2 velocity;
	private Vector2 inputDirection;

	[SerializeField]
	private Collider2D viewArea;
	[SerializeField]
	private LookAtPoint lookAt;
	[SerializeField]
	private Vector2 interestPoint;

	private bool interestPointInView = false;
	private bool investigateInterestPoint = false;
	[SerializeField]
	private float timeToInvestigateInterest;
	[SerializeField]
	private float timeBetweenWaypoints;
	private float waitTime = 0;
	private float investigateTime = 0;
	[SerializeField]
	private float maxTimeInvestigatePathing = 15;

	[SerializeField]
	private Transform pathHolder;
	private Vector2[] waypoints;
	private int waypointIndex;

	void OnDrawGizmos()
	{
		foreach(Transform waypoint in pathHolder)
		{
			Gizmos.DrawSphere(waypoint.position, .3f);
		}
	}

	public void SetInterestPoint(Vector2 position)
	{
		interestPoint = position;
		investigateInterestPoint = true;
	}

	// Use this for initialization
	void Start () {

		controller = GetComponent<Controller2D>();
		waypoints = new Vector2[pathHolder.childCount];
		for(int i = 0; i < waypoints.Length; i++)
		{
			waypoints[i] = pathHolder.GetChild(i).position;
		}

		this.transform.position = waypoints[0];
		interestPoint = waypoints[0];
	}

	private void UpdateWaypoint()
	{

	}

	// Update is called once per frame
	void Update () {

		inputDirection = Vector2.zero;
		float distanceX = Mathf.Abs(interestPoint.x - transform.position.x);

		if(investigateInterestPoint == true)
		{
			//Track noise;
			if(interestPointInView == false && (distanceX > 1.25f))
			{
				Vector2 direction = (interestPoint - (Vector2)this.transform.position).normalized;
				inputDirection.x = direction.x;

				interestPointInView = viewArea.bounds.Contains(interestPoint);

				//Set max time to investigate a point just incase guard never reaches a close enogh point to start look timer.
				investigateTime += Time.deltaTime;
				if(investigateTime > maxTimeInvestigatePathing)
				{
					this.interestPoint = waypoints[waypointIndex];
					investigateInterestPoint = false;
				}

				waitTime = 0;
			} else
			{
				waitTime += Time.deltaTime;
				if(waitTime > timeToInvestigateInterest)
				{
					this.interestPoint = waypoints[waypointIndex];
					investigateInterestPoint = false;
				}
			}
		} else
		{
			//follow waypoints
			if(distanceX > 0.75)
			{
				Vector2 direction = (interestPoint - (Vector2)this.transform.position).normalized;
				inputDirection.x = direction.x;

				waitTime = 0;
			} else
			{
				waitTime += Time.deltaTime;
				if(waitTime > timeBetweenWaypoints)
				{
					waitTime = 0;
					waypointIndex = (waypointIndex + 1) % waypoints.Length;
					this.interestPoint = waypoints[waypointIndex];
				}
			}
		}

		lookAt.UpdateLookAt(interestPoint);

		float targetVelocityX = inputDirection.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXsmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.y += Controller2D.gravity * Time.deltaTime;

		controller.Move(velocity * Time.deltaTime);

//		if(Mathf.Sign(this.transform.localScale.x) != Mathf.Sign(this.controller.collisions.faceDir))
//		{
//			Vector3 scale = this.transform.localScale;
//			scale.x *= -1;
//			this.transform.localScale = scale;
//		}

	}
}
