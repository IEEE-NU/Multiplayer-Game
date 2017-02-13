using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {

    public string collideThisPlayer = "Player 1";
    public string geyserID = "default";
    List<Player> overlappingPlayer = new List<Player>();

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        foreach (Player play in overlappingPlayer)
        {
            if (Input.GetButtonDown(play.gameObject.GetComponent<Player>().upKey))
            {
                activateLadder(play);
            }
        }
    }

    internal void activateLadder(Player interactor)
    {
        Debug.Log("Activating Ladder");

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
