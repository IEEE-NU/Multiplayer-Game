using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {

    public string collideThisPlayer = "Player 1";
    List<Player> overlappingPlayer = new List<Player>();
    List<Controller2D> overlappingControl = new List<Controller2D>();
    public float climbSpeed = 0.0f;

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        foreach (Player play in overlappingPlayer)
        {
			if (Input.GetKey (play.gameObject.GetComponent<Player> ().upKey)) {
				activateLadder (play, true);
			} else if (Input.GetKey (play.gameObject.GetComponent<Player> ().downKey)) {
				activateLadder (play, false);
			}
        }
    }

	internal void activateLadder(Player overlappingPlayer, bool up)
    {
		float upSpeed = climbSpeed;
		if (overlappingPlayer.GetComponent<Controller2D>().velocity.y < 0 ) {
			upSpeed -= (overlappingPlayer.GetComponent<Controller2D> ().velocity.y * 2);
		}
        //Debug.Log("Activating Ladder");
		if (!up) {
			upSpeed *= -1;
		}
		Vector3 upForce = new Vector3(0, upSpeed, 0);

		//Debug.Log(upForce);
		//Debug.Log(overlappingPlayer.GetComponent<Controller2D>().velocity.y);
        //if (overlappingPlayer.GetComponent<Controller2D>().velocity.y < 0.2f)
        //{
			overlappingPlayer.GetComponent<Controller2D>().addSelfForce(upForce,Time.deltaTime); // adds a force on the object.
        //}
    }

    internal void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>())
        { //This is how you can access variables and functions in another component.
            //Debug.Log("Player Added");
			other.gameObject.GetComponent<Player>().gravity = 0.0f;
            overlappingPlayer.Add(other.gameObject.GetComponent<Player>());
        }
    }
    internal void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            //Debug.Log("Player Removed");
			other.gameObject.GetComponent<Player>().gravity = -25.0f;
            overlappingPlayer.Remove(other.gameObject.GetComponent<Player>());
        }
    }
}
