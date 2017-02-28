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
            if (Input.GetKey(play.gameObject.GetComponent<Player>().upKey))
            {
                activateLadder(play);
            }
        }
    }

    internal void activateLadder(Player overlappingPlayer)
    {
        Debug.Log("Activating Ladder");
        Vector3 upForce = new Vector3(0, climbSpeed, 0);
        Debug.Log(upForce);
        Debug.Log(overlappingPlayer.GetComponent<Controller2D>().velocity.y);
        if (overlappingPlayer.GetComponent<Controller2D>().velocity.y < 0.2f)
        {
            overlappingPlayer.GetComponent<Controller2D>().addToVelocity(upForce); // adds a force on the object.
        }
    }

    internal void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>())
        { //This is how you can access variables and functions in another component.
            Debug.Log("Player Added");
            overlappingPlayer.Add(other.gameObject.GetComponent<Player>());
        }
    }
    internal void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            Debug.Log("Player Removed");
                overlappingPlayer.Remove(other.gameObject.GetComponent<Player>());
        }
    }
}
