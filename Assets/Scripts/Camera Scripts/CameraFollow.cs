using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	[SerializeField]
	private Controller2D target;

	[SerializeField]
	private Vector2 focusAreaSize;

	[SerializeField]
	private PlayerControllerData controllerData;

	[SerializeField]
	private float verticalOffset;
	[SerializeField]
	private float lookAheadDstX;
	[SerializeField]
	private float lookSmoothTimeX;
	[SerializeField]
	private float verticalSmoothTime;

	[SerializeField]
	private Vector2 clampCameraX;
	[SerializeField]
	private Vector2 clampCameraY;

	private float currentLookAheadX;
	private float targetLookAheadX;
	private float lookAheadDirX;
	private float smoothLookVelocityX;
	private float smoothVelocityY;

	private bool lookAheadStopped;

	private FocusArea focusArea;

	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private float cameraSize;

	struct FocusArea
	{
		public Vector2 center;
		public Vector2 velocity;
		float left, right;
		float top, bottom;

		public FocusArea(Bounds targetBounds, Vector2 size)
		{
			left = targetBounds.center.x - size.x / 2;
			right = targetBounds.center.x + size.x / 2;

			bottom = targetBounds.min.y;
			top = bottom + size.y;

			velocity = Vector2.zero;
			center = new Vector2((left + right) / 2, (top + bottom) / 2);
		}

		public void Update(Bounds targetBounds)
		{
			float shiftX = 0;
			if(targetBounds.min.x < left)
			{
				shiftX = targetBounds.min.x - left;
			} else if(targetBounds.max.x > right)
			{
				shiftX = targetBounds.max.x - right;
			}
				
			float shiftY = 0;
			if(targetBounds.min.y < bottom)
			{
				shiftY = targetBounds.min.y - bottom;
			} else if(targetBounds.max.y > top)
			{
				shiftY = targetBounds.max.y - top;
			}


			left += shiftX;
			right += shiftX;
			top += shiftY;
			bottom += shiftY;
			center = new Vector2((left + right) / 2, (top + bottom) / 2);
			velocity = new Vector2(shiftX, shiftY);
		}
	}

	public Vector2 SetEasterEggBounds(Vector2 newBounds)
	{
		Vector2 returnBounds = this.clampCameraX;
		this.clampCameraX = newBounds;
		return returnBounds;
	}

	public void SetNormalBounds(Vector2 newBounds)
	{
		this.clampCameraX = newBounds;
	}

	void Start () {
		AdjustCameraOrthoSize();
	}

	void LateUpdate()
	{
		if(target == null)
		{
			target = GameObject.FindWithTag("Player").GetComponent<Controller2D>();
			focusArea = new FocusArea(target.collider.bounds, focusAreaSize);
			return;
		}

		focusArea.Update(target.collider.bounds);

		Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;

		if(focusArea.velocity.x != 0)
		{
			lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
			if(Mathf.Sign(controllerData.inputDirection.x) == Mathf.Sign(focusArea.velocity.x) && controllerData.inputDirection.x != 0)
			{
				lookAheadStopped = false;
				targetLookAheadX = lookAheadDirX * lookAheadDstX;
			} else
			{
				if(lookAheadStopped == false)
				{
					lookAheadStopped = true;
					targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX) / 4f;
				}
			}
		}

		currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

		focusPosition += Vector2.right * currentLookAheadX;
		Vector3 newPosition = (Vector3)focusPosition + Vector3.forward * -10;

		newPosition.x = Mathf.Clamp(newPosition.x, this.clampCameraX.x, this.clampCameraX.y);
		newPosition.y = Mathf.Clamp(newPosition.y, this.clampCameraY.x, this.clampCameraY.y);

		transform.position = newPosition;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(1, 0, 0, 0.5f);
		Gizmos.DrawCube(focusArea.center, focusAreaSize);
	}

	private void AdjustCameraOrthoSize() 
	{
		// Sets the orthographic size of the screen to match the proper width of the objects in the scene
		float size = this.cameraSize / (2 * this.mainCamera.aspect);
		size = Mathf.Round(size * 1000f) / 1000f;
		this.mainCamera.orthographicSize = size;
	}
}
