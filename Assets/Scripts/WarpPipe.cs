using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPipe : MonoBehaviour {

	public Transform bumpCheck;

	public int returnPipeNumber = -1;
	public int warpPipeNumber;
	public string target;

	public bool active = true;
	public bool walkInPipe = false;

	bool canWarp;

	// Use this for initialization
	void Start () {
		canWarp = false;
	}

	void Update() {
		if (canWarp) {
			if (!walkInPipe && (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S))) {
				if (returnPipeNumber == -1) {
					Global.instance.EnterPipe (target, "pipe_down");
				} else {
					Global.instance.EnterPipe (returnPipeNumber, target, "pipe_down");
				}
			} else if (walkInPipe) {
				Collider2D c = Physics2D.OverlapBox (bumpCheck.position, Vector2.zero, 0f);
				if (c.tag == "Player" && Input.GetButton("Horizontal")) {
					if (returnPipeNumber == -1) {
						Global.instance.EnterPipe (target, "pipe_left");
					} else {
						Global.instance.EnterPipe (returnPipeNumber, target, "pipe_left");
					}
				}
			}
		}
	}

	// Update is called once per frame
	void OnCollisionStay2D(Collision2D c) {
		if (Global.instance.mario.grounded && c.gameObject.tag == "Player" && active) {
			canWarp = true;
		}
	}

	void OnCollisionExit2D(Collision2D c) {
		if (c.gameObject.tag == "Player") {
			canWarp = false;
		}
	}
}
