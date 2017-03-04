using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecific : MonoBehaviour {

	BoxCollider2D mCollider;
	public string collideThisPlayer = "Player 2";
	// Use this for initialization
	void Start () {
		mCollider = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	internal void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag (collideThisPlayer)) {
			mCollider.isTrigger = false;
		} else {
			mCollider.isTrigger = true;
		}
	}
}
