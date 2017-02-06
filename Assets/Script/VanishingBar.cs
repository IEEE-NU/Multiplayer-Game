using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingBar : MonoBehaviour {

    public bool vanish = false;
    public float countDown = 0.0f; //when use decimal alwasy put "f" at the end
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (vanish)
        {
            countDown += Time.deltaTime; //difference between time from this update and another
            if (countDown > 3.0f)
            {
                GameObject.Destroy(gameObject);
            }
        }
	}
    internal void OnTriggerEnter2D(Collider2D collision) //called when collision
    {
        if (collision.gameObject.GetComponent<Player>()) //return false if no compoent, return the component if true
            vanish = true; //return what actually collided with you.
    }
}
