using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public Transform topCheck;
	public Transform bumpCheck;

	public float speed = 1.0f;
	public float bounceHeight = 2.0f;
	public float bounceLengthVariance = 2.0f;
	public float bounceTorque = 12.0f;
	public Vector2 startingPosition;
	public Vector2 startingVelocity = Vector2.left;

	protected Vector2 dir;
	protected Animator anim;
	protected Rigidbody2D rb;
	protected bool dead, canMove;
	protected SpriteRenderer sr;

	// Use this for initialization
	protected void Start () {
		if (!topCheck) {
			topCheck = transform.FindChild ("TopCheck");
		}
		if (!bumpCheck) {
			bumpCheck = transform.FindChild ("BumpCheck");
		}
		dir = startingVelocity;
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();

		dead = false;
		canMove = false;
		GetComponent<SpriteRenderer> ().sortingOrder = 30000;
	}

	void FixedUpdate() {
		if (!Global.instance.death && !Global.instance.pause && !dead && canMove) {
			Move ();
		}
	}

	protected void Update() {
		GetComponent<BoxCollider2D>().size = new Vector2(sr.bounds.size.x, sr.bounds.size.y);

		if (Global.instance.death || !canMove) {
			rb.velocity = Vector2.zero;
		}

		if (!dead) {
			// LayerMask selfLayer = ~(1 << LayerMask.NameToLayer ("Enemy"));
			// Collider2D c = Physics2D.OverlapBox (bumpCheck.position, new Vector2 (0.5f, 0f), 0f, selfLayer);

//			if (c) {
//				if (c.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
//					dir.x = -dir.x;
//				}
//			}
		}
	}

	void Move() {
		rb.velocity = new Vector2(0f, rb.velocity.y) + dir * speed;
	}

	protected void OnBecameVisible() {
		canMove = true;
	}

	public void Death(bool bounce) {
		dead = true;
		rb.freezeRotation = false;
		if (bounce) {
			Global.instance.mario.Bounce ();
		}

		Global.instance.StompAudio ();
		Global.instance.GetPoints (100);

		anim.SetBool ("death", true);
		GetComponent<Collider2D> ().enabled = false;
		StartCoroutine (DisappearAfterTime (2.5f));

		rb.AddTorque (12f, ForceMode2D.Impulse);
		rb.AddForce (Vector2.up * bounceHeight + Vector2.right * (-bounceLengthVariance + 2 * bounceLengthVariance * Random.value), ForceMode2D.Impulse);
	}

	IEnumerator DisappearAfterTime (float t) {
		yield return new WaitForSeconds (t);
		GetComponent<SpriteRenderer> ().enabled = false;
	}

	void OnCollisionEnter2D(Collision2D c) {
		int layerMask = 1 << LayerMask.NameToLayer ("Player");
		if (c.transform.tag == "Player" && !Global.instance.death) {
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

	public void Revive() {
		transform.position = startingPosition;
		transform.rotation = Quaternion.identity;

		dead = false;
		anim.SetBool ("death", false);

		GetComponent<SpriteRenderer> ().enabled = true;
		GetComponent<Collider2D> ().enabled = true;

		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0f;
	}
}
