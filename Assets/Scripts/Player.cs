using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2d))]
public class Player : MonoBehaviour {

	[SerializeField]
	private float jumpHeight = 4;
	[SerializeField]
	private float timeToJumpApex = 0.4f;
	[SerializeField]
	private float moveSpeed = .6f;

	private float accelerationTimeAirborne = .2f;
	private float accelerationTimeGrounded = .1f;

	private float velocityXsmoothing;

	private float gravity;
	private float jumpVelocity;

	private Vector2 velocity;

	private Controller2d controller;

	// Use this for initialization
	void Start () {
	
		controller = GetComponent<Controller2d>();
		CalculateGravityAndJumpVel();
	}

	private void CalculateGravityAndJumpVel()
	{
		gravity = -1 * (2 * jumpHeight) / (Mathf.Pow(timeToJumpApex, 2));
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		Debug.Log("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
	}

	// Update is called once per frame
	void Update () {

		if(controller.collisions.above || controller.collisions.below)
		{
			velocity.y = 0;
		}

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if(Input.GetButtonDown("Jump") && controller.collisions.below)
		{
			velocity.y = jumpVelocity;
		}

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXsmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity);
	}
}
