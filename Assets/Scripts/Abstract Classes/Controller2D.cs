using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {

	public static float gravity;

	public LayerMask collisionMask;

	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	private const float skinWidth = 0.015f;

	private float horrizontalRaySpacing;
	private float verticalRaySpacing;

	public new BoxCollider2D collider;
	private RaycastOrigins raycastOrigins;
	public CollisionInfo collisions;

	void Awake() 
	{
		collider = GetComponent<BoxCollider2D>();
		collisions.faceDir = 1;
	}

	void Start()
	{
		CalculateRaySpacing();
	}

	void Update()
	{

	}

	public void Move(Vector3 velocity)
	{
		UpdateRaycastOrigins();
		collisions.Reset();

		if(velocity.x != 0)
		{
			collisions.faceDir = (int)Mathf.Sign(velocity.x);
		}

		HorizontalCollisions(ref velocity);
		VerticalCollisions(ref velocity);

		transform.Translate(velocity);
	}

	private void HorizontalCollisions(ref Vector3 velocity)
	{
		float directionX = collisions.faceDir;
		float rayLength = Mathf.Abs(velocity.x) + skinWidth;

		if(Mathf.Abs(velocity.x) < skinWidth)
		{
			rayLength = 2 * skinWidth;
		}

		for(int i = 0; i < horizontalRayCount; i++)
		{
			Vector2 rayOrigin = (directionX == -1? raycastOrigins.bottomLeft : raycastOrigins.bottomRight);
			rayOrigin += Vector2.up * (horrizontalRaySpacing * i);

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);
			if(hit)
			{
				velocity.x = (hit.distance - skinWidth) * directionX;
				rayLength = hit.distance;

				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}
		}
	}

	private void VerticalCollisions(ref Vector3 velocity)
	{
		float directionY = 1;
		if(velocity.y != 0)
		{
			directionY = Mathf.Sign(velocity.y);
		}

		float rayLength = Mathf.Abs(velocity.y) + skinWidth;
		if(Mathf.Abs(velocity.y) < skinWidth)
		{
			rayLength = 2 * skinWidth;
		}

		for(int i = 0; i < verticalRayCount; i++)
		{
			Vector2 rayOrigin = (directionY == -1? raycastOrigins.bottomLeft : raycastOrigins.topLeft);
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);
			if(hit)
			{
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}
	}

	private void UpdateRaycastOrigins() 
	{
		Bounds bounds = CalcRayBounds();

		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	private void CalculateRaySpacing() 
	{
		Bounds bounds = CalcRayBounds();

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

		horrizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	private Bounds CalcRayBounds() 
	{
		Bounds bounds = collider.bounds;
		bounds.Expand(skinWidth * -2);

		return bounds;
	}

	struct RaycastOrigins 
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	public struct CollisionInfo
	{
		public bool above, below;
		public bool left, right;
		public int faceDir;

		public void Reset() 
		{
			above = below = false;
			left = right = false;
		}
	}
}
