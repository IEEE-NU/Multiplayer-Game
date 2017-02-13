using UnityEngine;
using System.Collections;
//This component provides an example for how we can do triggers
// on contact
public class deathZone : MonoBehaviour {

	internal void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ("entered Zone");
	}

	internal void onTriggerExit2D(Collider2D other)
	{
		Debug.Log ("exit Zone");
	}
}
