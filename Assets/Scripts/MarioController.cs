using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioController : MonoBehaviour {

	public float speed = 3.0f;
	public float jumpHeight = 3.0f;
	public float bounceHeight = 2.0f;
	public Transform groundCheck;
	public float groundCheckRadius = 0.1f;

	Animator anim;
	SpriteRenderer sr;
	Rigidbody2D rb;

	int groundLayer;
	bool grounded;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		sr = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();

		groundLayer = 1 << LayerMask.NameToLayer ("Ground");
		grounded = true;

		sr.sortingOrder = 32767;
	}
	
	// Update is called once per frame
	void Update () {
		if (!Global.instance.death) {
			Move ();
			Jump ();
		} else {
			anim.SetBool ("death", true);
		}
	}

	void FixedUpdate() {
		if (!Global.instance.death) {
			grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, groundLayer);
			if (grounded) {
				anim.SetBool ("jump", false);
			} else {
				anim.SetBool ("jump", true);
			}
		}	
	}

	void OnCollisionEnter2D(Collision2D c) {
		if (c.gameObject.tag == "Enemy") {
			// rb.AddForce (Vector2.up * bounceHeight, ForceMode2D.Impulse);
		}
	}

	void Move() {
		float h = Input.GetAxis ("Horizontal");

		anim.SetFloat ("speed", Mathf.Abs(h));
		sr.flipX = h == 0f ? sr.flipX : (h < 0f ? true : false);
		transform.Translate (Vector3.right * h * Time.deltaTime * speed);
	}

	void Jump() {
		if ((Input.GetButtonDown ("Jump") || Input.GetButtonDown("Up"))&& grounded) {
			Bounce (jumpHeight);
			Global.instance.JumpAudio (false);
		}
	}

	IEnumerator DeathAnimation(float t, bool bounce) {
		yield return new WaitForSeconds (t);
		if (bounce) {
			Bounce (2.5f);
		}
		transform.GetComponent<Collider2D> ().enabled = false;
		yield return new WaitForSeconds (Global.instance.deathAudio.length);
		Global.instance.ResetPosition ();
	}

	public void Bounce() {
		rb.AddForce (Vector2.up * bounceHeight, ForceMode2D.Impulse);
	}

	public void Bounce(float height) {
		rb.AddForce (Vector2.up * height, ForceMode2D.Impulse);
	}

	public void Revive() {
		anim.SetBool ("death", false);
		transform.GetComponent<Collider2D> ().enabled = true;
		rb.velocity = Vector2.zero;
	}

	public void Die(bool bounce) {
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		anim.SetBool ("death", true);
		StartCoroutine (DeathAnimation (0.1f, bounce));
		Global.instance.DeathAudio ();
		Global.instance.death = true;
	}
}
