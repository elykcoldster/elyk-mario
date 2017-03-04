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

	Vector2 dir;
	Animator anim;
	Rigidbody2D rb;

	bool dead;

	// Use this for initialization
	void Start () {
		if (!topCheck) {
			topCheck = transform.FindChild ("TopCheck");
		}
		if (!bumpCheck) {
			bumpCheck = transform.FindChild ("BumpCheck");
		}
		dir = Vector2.left;
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		dead = false;
		GetComponent<SpriteRenderer> ().sortingOrder = 30000;
	}

	void FixedUpdate() {
		if (!Global.instance.death && !dead) {
			Move ();
		}
	}

	void Update() {
		if (!dead) {
			LayerMask selfLayer = ~(1 << LayerMask.NameToLayer ("Enemy"));
			Collider2D c = Physics2D.OverlapBox (bumpCheck.position, new Vector2 (0.5f, 0f), 0f, selfLayer);

			if (c) {
				if (c.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
					dir.x = -dir.x;
				}
			}
		}
	}

	void Move() {
		rb.velocity = new Vector2(0f, rb.velocity.y) + dir;
	}

	void Death() {
		dead = true;
		Global.instance.mario.Bounce ();
		Global.instance.StompAudio ();

		anim.SetBool ("death", true);
		GetComponent<Collider2D> ().enabled = false;
		StartCoroutine (DisappearAfterTime (2.5f));

		rb.AddTorque (12f, ForceMode2D.Impulse);
		rb.AddForce (Vector2.up * bounceHeight + Vector2.right * (-bounceLengthVariance + 2 * bounceLengthVariance * Random.value), ForceMode2D.Impulse);
	}

	IEnumerator DisappearAfterTime (float t) {
		yield return new WaitForSeconds (t);
		Destroy (gameObject);
	}

	void OnCollisionStay2D(Collision2D c) {
		int layerMask = 1 << LayerMask.NameToLayer ("Player");
		if (c.transform.tag == "Player" && !Global.instance.death) {
			if (Physics2D.OverlapBox (bumpCheck.position, new Vector2 (0.51f, 0.1f), 0f, layerMask)) {
				Global.instance.mario.Die (true);
			} else if (Physics2D.OverlapBox (topCheck.position, new Vector2 (0.5f, 0.01f), 0f, layerMask)) {
				Death ();
			}
		}
	}
}
