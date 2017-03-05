using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPipe : MonoBehaviour {

	public Transform returnPipe;
	public Transform target;
	public Transform warpTrigger;
	public Vector2 warpTriggerSize;
	public Vector2 triggerDirection = Vector2.down;

	// Use this for initialization
	void Start () {
		if (!warpTrigger) {
			warpTrigger = (new GameObject ()).transform;
			warpTrigger.position = transform.position;
			warpTrigger.parent = transform;
		}
	}
	
	// Update is called once per frame
	void OnCollisionStay2D(Collision2D c) {
		if (Global.instance.mario.grounded && c.gameObject.tag == "Player") {
			if (triggerDirection.y == -1 && (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S))) {
				Global.instance.mario.EnterPipe (returnPipe, target, "pipe_down");
			} else if (triggerDirection.x == 1 && (Input.GetKey (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D))) {
				Global.instance.mario.EnterPipe (returnPipe, target, "pipe_right");
			} else if (triggerDirection.x == -1 && (Input.GetKey (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A))) {
				Global.instance.mario.EnterPipe (returnPipe, target, "pipe_left");
			}
		}
	}
}
