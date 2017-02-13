﻿using UnityEngine;
using System.Collections;

using UnityEngine.UI;

// This component represents a controllable player character and has controls
// that you can modify. 
[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	public float jumpHeight = 4.0f;
	public float timeToJumpApex = .4f;
	public string playerName = "Player 1";
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	float moveSpeed = 6.0f;

	float gravity;
	float jumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;
	public string leftKey = "a";
	public string rightKey = "d";
	public string upKey = "w";
	public string downKey = "s";
	public string jumpKey = "space";

    public Text P1WinText;
    public Text P2WinText;
    public bool goUp = false;

    Controller2D controller;

	public bool attemptingInteraction = false;

	void Start() {
        P1WinText.text = "";
        P2WinText.text = "";
        controller = GetComponent<Controller2D> ();
		float grav = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		gravity = grav;
		controller.setGravityScale(grav);
		jumpVelocity = Mathf.Abs(grav) * timeToJumpApex;
		print ("Gravity: " + grav + "  Jump Velocity: " + jumpVelocity);
	}

	void Update() {

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0.0f;
		}

		float inputX = 0.0f;
		if (Input.GetKey(leftKey)) {
			inputX = -1.0f;
		} else if (Input.GetKey(rightKey)) {
			inputX = 1.0f;
		}
		float inputY = 0.0f;
		if (Input.GetKey(upKey)) {
			inputY = 1.0f;
		} else if (Input.GetKey(downKey) ){
			inputY = -1.0f;
		}
		if (Input.GetKeyDown (downKey)) {
			attemptingInteraction = true;
		} else {
			attemptingInteraction = false;
		}

				
		if (Input.GetKey (jumpKey) && controller.collisions.below) {
			velocity.y = jumpVelocity;
		}

        float targetVelocityX = inputX * moveSpeed;

		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		Vector2 input = new Vector2 (inputX, inputY);
		velocity.y += gravity * Time.deltaTime;
		//controller.Move (velocity * Time.deltaTime, input);
		//Debug.Log(velocity.y);
		controller.Move (velocity, input);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Door1"))
        {
            P1WinText.text = "Player 1 Won!";
        }
        else if (other.gameObject.CompareTag("Door2"))
        {
            P2WinText.text = "Player 2 Won!";
        }
    }
}
