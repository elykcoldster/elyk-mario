using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : Enemy {

	public float slideSpeed = 4f;
	public float moveSpeed = 1.0f;

	// Use this for initialization
	void Start () {
		base.Start ();
		base.speed = moveSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();

		if (dead) {
			if (Mathf.Abs (rb.velocity.x) > 0.1f) {
				gameObject.layer = LayerMask.NameToLayer ("Destroy");
			} else {
				gameObject.layer = LayerMask.NameToLayer ("Enemy");
			}
		}
//		if (dead) {
//			LayerMask selfLayer = ~(1 << LayerMask.NameToLayer ("Enemy"));
//			Collider2D c = Physics2D.OverlapBox (bumpCheck.position, new Vector2 (0.5f, 0f), 0f, selfLayer);
//
//			if (c) {
//				if (c.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
//					print ("asdf");
//					rb.velocity = -rb.velocity;
//				}
//			}
//		}
	}

	public void Death(bool bounce) {
		if (!dead) {
			Global.instance.GetPoints (200);
		}
		dead = true;
		rb.velocity = Vector2.zero;
		if (bounce) {
			Global.instance.mario.Bounce ();
		}
		Global.instance.StompAudio ();

		anim.SetBool ("dead", true);
	}

	void Slide(Vector2 f) {
		dir = f;
		rb.velocity = dir;
	}

	void OnCollisionEnter2D(Collision2D c) {
		int layerMask = 1 << LayerMask.NameToLayer ("Player");
		if (c.transform.tag == "Player" && !Global.instance.death && !dead) {
			if (Physics2D.OverlapBox (bumpCheck.position, new Vector2 (0.51f, 0.1f), 0f, layerMask)) {
				if (Global.instance.mario.starPower) {
					Death (false);
				} else {
					if (MarioController.instance.super) {
						Global.instance.mario.Mini ();
					} else {
						Global.instance.mario.Die (true);
					}
				}
			} else if (Physics2D.OverlapBox (topCheck.position, new Vector2 (0.5f, 0.01f), 0f, layerMask)) {
				Death (true);
			}
		} else if (c.transform.tag == "Player" && !Global.instance.death && dead) {
			Vector2 f = (transform.position - c.transform.position);
			f.y = 0f;
			f.Normalize ();
			if (Physics2D.OverlapBox (bumpCheck.position, new Vector2 (0.51f, 0.1f), 0f, layerMask)) {
				if (Mathf.Abs(rb.velocity.x) < 0.1f) {
					Slide (f * slideSpeed);
				} else {
					if (MarioController.instance.super) {
						Global.instance.mario.Mini ();
					} else {
						Global.instance.mario.Die (true);
						rb.velocity = Vector2.zero;
					}
				}
			}
			if (Physics2D.OverlapBox (topCheck.position, new Vector2 (0.5f, 0.01f), 0f, layerMask)) {
				Death (true);
				Slide (f * slideSpeed);
			}
		} else if (c.transform.tag == "Enemy" && !Global.instance.death && dead && Mathf.Abs(rb.velocity.x) > 0.1f) {
			print (rb.velocity.x);
			c.gameObject.GetComponent<Enemy> ().Death (false);
			rb.velocity = dir;
		}

		if (c.transform.tag == "Wall") {
			dir.x = -dir.x;
			rb.velocity = dir;
			if (dir.x > 0f) {
				sr.flipX = true;
			} else {
				sr.flipX = false;
			}
		}
	}
}
