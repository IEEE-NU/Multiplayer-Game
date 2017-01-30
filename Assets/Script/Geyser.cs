using UnityEngine;
using System.Collections.Generic; //REMEMBER! In order to use lists! Make sure it is System.Collections.Generic instead of System.Collections

public class Geyser : MonoBehaviour {

	public string geyserID = "default";
	List<Controller2D> overlappingControl = new List<Controller2D> (); 
	public float geyserTime = 0.0f;
	public float geyserSpeed = -60.0f;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
		if (geyserTime > 0.0f) {
			Vector3 upForce = new Vector3 (0, geyserSpeed, 0);
			foreach(Controller2D cont in overlappingControl) {
				Debug.Log (upForce);
				cont.Move (upForce);
			}
			geyserTime -= Time.deltaTime;
		}
	}
	public void activateGeyser(float time) {
		Debug.Log (time);
		geyserTime = time;
	}
	internal void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("collision detected with Geyser");
		overlappingControl.Add (other.gameObject.GetComponent<Controller2D> ());
	}
	internal void OnTriggerExit2D(Collider2D other) {
		Debug.Log ("Collision ended with Geyser");
		overlappingControl.Remove (other.gameObject.GetComponent<Controller2D> ());
	}
}
