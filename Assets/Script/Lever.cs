﻿using UnityEngine;
using System.Collections.Generic;

//This provides an example of an "interactable" object.
//an object that players can press the "use" button on and then can perform an action.
public class Lever : MonoBehaviour {

	public string collideThisPlayer = "Player 1";
	public string geyserID = "default";
	List<Player> overlappingPlayer = new List<Player> (); 

	// Use this for initialization
	void Start () { }

	// Update is called once per frame
	void Update () {
		foreach(Player play in overlappingPlayer) {
			if (play.attemptingInteraction == true) {
				activateLever (play);
			}
		}
	}

	internal void activateLever(Player interactor) {
		Debug.Log ("Activating Lever");
		Geyser[] geysers = GameObject.FindObjectsOfType<Geyser> ();
		foreach (Geyser geyser in geysers) {
			if (geyser.geyserID == geyserID) {
				geyser.activateGeyser(3.0f);
			}
		}
	}

	internal void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<Player> ()) { //This is how you can access variables and functions in another component.
			Debug.Log ("Player Added");
			overlappingPlayer.Add (other.gameObject.GetComponent<Player> ());
		}
	}
	internal void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.GetComponent<Player> ()) {
			Debug.Log ("Player Removed");
			overlappingPlayer.Remove (other.gameObject.GetComponent<Player> ());
		}
	}
}