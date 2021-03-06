﻿using UnityEngine;
using System.Collections.Generic;
// This component can be given to an object in order to give it "physics"
// AKA, the object has gravity and can collide with things.
[RequireComponent (typeof (BoxCollider2D))]
public class Controller2D : MonoBehaviour {

	public LayerMask collisionMask;

	const float skinWidth = .015f;
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;
	public Vector2 playerInput = Vector2.zero;
	public Vector2 accumulatedVelocity = Vector2.zero;
	public bool isGravity = true;
	public float gravityScale = 40.0f;
	public float health = 100.0f;
	public bool alive = true;

	float maxClimbAngle = 80;

	float horizontalRaySpacing;
	float verticalRaySpacing;
	public Vector3 velocity;
	List<Vector2> CharForces = new List<Vector2>();
	List<float> timeForces = new List<float>();

	BoxCollider2D collider;
	RaycastOrigins raycastOrigins;
	public CollisionInfo collisions;

	void Start() {
		collider = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing ();
	}

	public void Move(Vector3 velocity) {
		Move (velocity, Vector2.zero);
	}
	public void addSelfForce(Vector2 force, float duration) {
		CharForces.Add (force);
		timeForces.Add (duration);
	}


	public void setGravityScale(float gravScale) {
		gravityScale = gravScale;
	}

	void Update() {
		if (Mathf.Abs (accumulatedVelocity.x) > 0.0f) {
			accumulatedVelocity.x *= 0.95f;
		} else {
			accumulatedVelocity.x = 0f;
		}
		if (Mathf.Abs (accumulatedVelocity.y) > 0.0f) {
			accumulatedVelocity.y *= 0.95f;
		} else {
			accumulatedVelocity.y = 0f;
		}
	}

	public void addToVelocity(Vector2 veloc) {
		accumulatedVelocity += veloc;
	}

	public void Move(Vector3 veloc, Vector2 input) {
		//Debug.Log ("----");
		//Debug.Log (veloc.y);
		//if (isGravity) {
		//		veloc.y = veloc.y +  (gravityScale * Time.deltaTime);
		//	Debug.Log (veloc.y);
		//}

		veloc = veloc * Time.deltaTime;
		velocity.x = veloc.x;
		velocity.x += (accumulatedVelocity.x * Time.deltaTime);
		velocity.y = veloc.y;
		velocity.y += (accumulatedVelocity.y * Time.deltaTime);
		//Debug.Log (velocity.y);
		UpdateRaycastOrigins ();
		collisions.Reset ();
		playerInput = input;
	
		for (int i = CharForces.Count - 1; i >= 0; i--) {
			Vector2 selfVec = CharForces [i];
			velocity = new Vector3(velocity.x + (selfVec.x * Time.deltaTime),velocity.y + (selfVec.y * Time.deltaTime),velocity.z);
			timeForces [i] = timeForces [i] - Time.deltaTime;
			if (timeForces [i] < 0f) {
				CharForces.RemoveAt (i);
				timeForces.RemoveAt (i);
			}
		}

		if (velocity.x != 0) {
			HorizontalCollisions (ref velocity);
		}
		if (velocity.y != 0) {
			VerticalCollisions (ref velocity);
		}

		transform.Translate (velocity);
	}

	void HorizontalCollisions(ref Vector3 velocity) {
		float directionX = Mathf.Sign (velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;
		
		for (int i = 0; i < horizontalRayCount; i ++) {
			Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength,Color.red);

			if (hit && !hit.collider.isTrigger) {

				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

				if (i == 0 && slopeAngle <= maxClimbAngle) {
					float distanceToSlopeStart = 0;
					if (slopeAngle != collisions.slopeAngleOld) {
						distanceToSlopeStart = hit.distance-skinWidth;
						velocity.x -= distanceToSlopeStart * directionX;
					}
					ClimbSlope(ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart * directionX;
				}

				if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;

					if (collisions.climbingSlope) {
						velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
					}

					collisions.left = directionX == -1;
					collisions.right = directionX == 1;
				}
			}
		}
	}
	
	void VerticalCollisions(ref Vector3 velocity) {
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i ++) {
			Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength,Color.red);

			if (hit && !hit.collider.isTrigger) {
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				if (collisions.climbingSlope) {
					velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
				}

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}
	}

	void ClimbSlope(ref Vector3 velocity, float slopeAngle) {
		float moveDistance = Mathf.Abs (velocity.x);
		float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (velocity.y <= climbVelocityY) {
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
		}
	}

	void UpdateRaycastOrigins() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}

	void CalculateRaySpacing() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;

		public bool climbingSlope;
		public float slopeAngle, slopeAngleOld;

		public void Reset() {
			above = below = false;
			left = right = false;
			climbingSlope = false;

			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}

}