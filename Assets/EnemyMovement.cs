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

	[SerializeField]
	private Vector2 interestPoint;

	private Controller2D controller;
	private Vector2 velocity;
	private Vector2 inputDirection;

	public void SetInterestPoint(Vector2 position)
	{
		interestPoint = position;
	}

	// Use this for initialization
	void Start () {

		controller = GetComponent<Controller2D>();
	}
	
	// Update is called once per frame
	void Update () {

		inputDirection = Vector2.zero;
		if((Mathf.Abs(interestPoint.x - transform.position.x) > 0.5f))
		{
			Vector2 direction = (interestPoint - (Vector2)this.transform.position).normalized;
			inputDirection.x = direction.x;
		}

		float targetVelocityX = inputDirection.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXsmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

		velocity.y += Controller2D.gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);


		if(Mathf.Sign(this.transform.localScale.x) != Mathf.Sign(this.controller.collisions.faceDir))
		{
			Vector3 scale = this.transform.localScale;
			scale.x *= -1;
			this.transform.localScale = scale;
		}

	}
}
