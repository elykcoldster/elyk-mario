using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalCoin : MonoBehaviour {

	public int points;

	SpriteRenderer sr;
	BoxCollider2D bc;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		bc = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.transform.tag == "Player") {
			sr.enabled = false;
			bc.enabled = false;
			GetComponent<AudioSource> ().Play ();
			Global.instance.GetPoints (points);
			Global.instance.GetCoin ();
		}
	}
}
