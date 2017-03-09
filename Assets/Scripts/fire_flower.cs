using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_flower : MonoBehaviour {

	public int points;

	Animator anim;
	AudioSource asource;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		asource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D c) {
		if (c.gameObject.tag == "Player") {
			Global.instance.PowerUpAudio ();
			MarioController.instance.Fire (true);
			Global.instance.GetPoints (points);
			Destroy (gameObject);
		}
	}

	public void FinishEmerge() {
		anim.SetTrigger ("finish_emerge");
	}
}
