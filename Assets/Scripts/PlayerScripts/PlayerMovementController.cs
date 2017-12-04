using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerMovementController : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer sprite;

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

	[SerializeField]
	private float accelerationTimeGrounded = .1f;
	[SerializeField]
	private float accelerationTimeAirborne = .2f;

	private float velocityXsmoothing;

	private float jumpVelocity;

	private Vector2 velocity;

	private Controller2D controller;

	private bool jumpDown = false;
	private bool firstJumpFrame = false;
	private bool movingOutsideSource = false;

	// Use this for initialization
	void Start () {
	
		controller = GetComponent<Controller2D>();
		CalculateGravityAndJumpVel();
	}

	void OnEnable()
	{
		controllerData.JumpButtonDown += OnJumpInputDown;
		controllerData.JumpButtonUp += OnJumpInputUp;
		controllerData.MovePlayerGrapple += SetMoveTowards;
	}

	void OnDisable()
	{
		controllerData.JumpButtonDown -= OnJumpInputDown;
		controllerData.JumpButtonUp -= OnJumpInputUp;
		controllerData.MovePlayerGrapple -= SetMoveTowards;
	}

	private void CalculateGravityAndJumpVel()
	{
		Controller2D.gravity = -1 * (2 * jumpHeight) / (Mathf.Pow(timeToJumpApex, 2));
		jumpVelocity = Mathf.Abs(Controller2D.gravity) * timeToJumpApex;
//		wallSlideSpeedMax = gravity / 4;
		Debug.Log("Gravity: " + Controller2D.gravity + " Jump Velocity: " + jumpVelocity);
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

	public void SetMoveTowards(Vector2 targetPosition)
	{
		if(movingOutsideSource == false)
		{
			StartCoroutine(MoveTowards(targetPosition));
		}
	}

	private IEnumerator MoveTowards(Vector2 targetPosition)
	{
		movingOutsideSource = true;
		while(controllerData.onCeiling == false)
		{
			if(controller.collisions.above == true)
			{
				controllerData.onCeiling = true;
				break;
			}

			controller.Move(targetPosition * Time.deltaTime);
			yield return null;
		}

		movingOutsideSource = false;
	}

	// Update is called once per frame
	void Update () {

		if(movingOutsideSource == true)
		{
			return;
		}

		int wallDirX = (controller.collisions.left) ? -1 : 1;
		float targetVelocityX = controllerData.inputDirection.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXsmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

		//Handel moving when on a wall
		//============================//
		bool wallSliding = false;
		sprite.transform.rotation = Quaternion.identity;
		if((controller.collisions.left || controller.collisions.right) && controller.collisions.below == false)// && velocity.y < 0)
		{
			wallSliding = true;

			sprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, wallDirX * 90));

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

		//adjust gravity when hitting head or landing on ground.
		//=====================================================//
		if((controller.collisions.above || controller.collisions.below) && wallSliding == false)
		{
			velocity.y = 0;
		}
			
		controllerData.onCeiling = sprite.flipY = false;
		if(controller.collisions.above == true)
		{
			controllerData.onCeiling = true;

			if(wallSliding == false)
			{
				sprite.flipY = true;
			}
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

			if(controllerData.onCeiling == true)
			{
				velocity.y = -jumpVelocity;
			}
		}
			
		if(wallSliding == false && controllerData.onCeiling == false)
		{
			velocity.y += Controller2D.gravity * Time.deltaTime;
		}

		controllerData.playerVelocity = this.velocity;
		controller.Move(velocity * Time.deltaTime);
	}
}
