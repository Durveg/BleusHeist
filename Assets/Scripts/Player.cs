using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

	[Header("Controller Data")]
	[SerializeField]
	private PlayerControllerData controllerData;

	[Header("Normal Jump Variables")]
	[SerializeField]
	private float jumpHeight = 4;
	[SerializeField]
	private float timeToJumpApex = 0.4f;

	[Header("Movement Speed Variables")]
	[SerializeField]
	private float moveSpeed = .6f;
	[SerializeField]
	private float wallMoveSpeed = 2f;

	[Header("Wall Jump Variables")]
	[SerializeField]
	private float wallStickTime = 0.25f;
	private float timeToWallUnstick;
	[SerializeField]
	private Vector2 wallJumpClimb;
	[SerializeField]
	private Vector2 wallJumpOff;
	[SerializeField]
	private Vector2 wallLeap;

	private float accelerationTimeAirborne = .2f;
	private float accelerationTimeGrounded = .1f;

	private float velocityXsmoothing;

	private float gravity;
	private float jumpVelocity;

	private Vector2 velocity;

	private Controller2D controller;

	private bool jumpDown = false;
	private bool firstJumpFrame = false;
	private bool ceilingSticking = false;

	// Use this for initialization
	void Start () {
	
		controller = GetComponent<Controller2D>();
		CalculateGravityAndJumpVel();
	}

	void OnEnable()
	{
		controllerData.JumpButtonDown += OnJumpInputDown;
		controllerData.JumpButtonUp += OnJumpInputUp;
	}

	void OnDisable()
	{
		controllerData.JumpButtonDown -= OnJumpInputDown;
		controllerData.JumpButtonUp -= OnJumpInputUp;
	}

	private void CalculateGravityAndJumpVel()
	{
		gravity = -1 * (2 * jumpHeight) / (Mathf.Pow(timeToJumpApex, 2));
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
//		wallSlideSpeedMax = gravity / 4;
		Debug.Log("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
	}

	public void SetDirectionalInput (Vector2 input) {
		controllerData.inputDirection = input;
	}

	public void OnJumpInputDown() 
	{
		jumpDown = true;
		firstJumpFrame = false;
	}

	public void OnJumpInputUp() 
	{
		jumpDown = false;
	}

	// Update is called once per frame
	void Update () {

		int wallDirX = (controller.collisions.left) ? -1 : 1;
		float targetVelocityX = controllerData.inputDirection.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXsmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

		bool wallSliding = false;
		if((controller.collisions.left || controller.collisions.right) && controller.collisions.below == false)// && velocity.y < 0)
		{
			wallSliding = true;

			velocity.y = controllerData.inputDirection.y * wallMoveSpeed;
			velocity.y = Mathf.Clamp(velocity.y, -wallMoveSpeed, wallMoveSpeed);

			if(timeToWallUnstick > 0)
			{
				velocity.x = 0;
				velocityXsmoothing = 0;

				if(controllerData.inputDirection.x != wallDirX && controllerData.inputDirection.x != 0)
				{
					timeToWallUnstick -= Time.deltaTime;
				} else
				{
					timeToWallUnstick = wallStickTime;
				}
			} else
			{
				timeToWallUnstick = wallStickTime;
			}
		}

		if((controller.collisions.above || controller.collisions.below) && wallSliding == false)
		{
			velocity.y = 0;
		}
			
		ceilingSticking = false;
		if(controller.collisions.above == true)
		{
			ceilingSticking = true;
		}

		if(jumpDown == true && firstJumpFrame == false)
		{
			firstJumpFrame = true;
			if(wallSliding)
			{
				if(wallDirX == controllerData.inputDirection.x)
				{
					velocity.x = -wallDirX * wallJumpClimb.x;
					velocity.y = wallJumpClimb.y;
				} else if(controllerData.inputDirection.x == 0)
				{
					velocity.x = -wallDirX * wallJumpOff.x;
					velocity.y = wallJumpOff.y;
				} else
				{
					velocity.x = -wallDirX * wallLeap.x;
					velocity.y = wallLeap.y;
				}
			}

			if(controller.collisions.below)
			{
				velocity.y = jumpVelocity;
			}

			if(ceilingSticking == true)
			{
				velocity.y = -jumpVelocity;
			}
		}
			
		if(wallSliding == false && ceilingSticking == false)
		{
			velocity.y += gravity * Time.deltaTime;
		}

		controller.Move(velocity * Time.deltaTime);
	}
}
