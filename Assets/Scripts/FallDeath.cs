using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDeath : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	void OnCollisionStay2D (Collision2D c) {
		if (c.transform.tag == "Player" && !Global.instance.death) {
			Global.instance.mario.Die (false);
		}

		if (c.transform.tag == "Enemy" || c.transform.tag == "Item") {
			Destroy (c.gameObject);
		}
	}
}
