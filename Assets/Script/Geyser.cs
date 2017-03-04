using UnityEngine;
using System.Collections.Generic; //REMEMBER! In order to use lists! Make sure it is System.Collections.Generic instead of System.Collections
// Example of an object that can affect the physics of another object.
public class Geyser : MonoBehaviour {

	public string geyserID = "default";
	List<Controller2D> overlappingControl = new List<Controller2D> (); 
	public float geyserTime = 0.0f;
	public float geyserSpeed = 5.0f;
	public float maxVerticalSpeed = 1.0f;
    public string collideThisPlayer = "Player 2";
	ParticleSystem geyserParticles;
	bool isPlaying = false;
    // Use this for initialization
    void Start () {
		geyserParticles = GetComponentInChildren<ParticleSystem> ();
		geyserParticles.Stop ();
	}
	
	// Update is called once per frame
	void Update () {
		if (geyserTime > 0.0f) {
			Vector3 upForce = new Vector3 (0, geyserSpeed, 0);
			foreach (Controller2D cont in overlappingControl) {
				Debug.Log (upForce);
				Debug.Log (cont.velocity.y);
				if (cont.velocity.y < maxVerticalSpeed) {
					cont.addToVelocity (upForce); // adds a force on the object.
				}
			}
			geyserTime -= Time.deltaTime;
			if (isPlaying == false) {
				isPlaying = true;
				geyserParticles.Play ();
				Debug.Log ("Geyser starting");
			}
		} else {
			if (isPlaying == false) {
				geyserParticles.Stop ();
				isPlaying = true;
				Debug.Log ("Geyser stopping");
			}
		}
	}
	public void activateGeyser(float time) {
		Debug.Log (time);
		geyserTime = time;
	}
    internal void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collision detected with Geyser");
        if (other.gameObject.CompareTag(collideThisPlayer))
        {
            overlappingControl.Add(other.gameObject.GetComponent<Controller2D>()); //Adds the other object's Controller2D to list of contacting objects
        }
	} 
	internal void OnTriggerExit2D(Collider2D other) {
		Debug.Log ("Collision ended with Geyser");
        if (other.gameObject.CompareTag(collideThisPlayer))
        {
            overlappingControl.Remove(other.gameObject.GetComponent<Controller2D>()); //Removes the object from the list
        }
	}
}
