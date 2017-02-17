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
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
		Jump ();
	}

	void FixedUpdate() {
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, groundLayer);
		if (grounded) {
			anim.SetBool ("jump", false);
		} else {
			anim.SetBool ("jump", true);
		}
	}

	void OnCollisionEnter2D(Collision2D c) {
		if (c.gameObject.tag == "Enemy") {
			rb.AddForce (Vector2.up * bounceHeight, ForceMode2D.Impulse);
		}
	}

	void Move() {
		float h = Input.GetAxis ("Horizontal");

		anim.SetFloat ("speed", Mathf.Abs(h));
		sr.flipX = h == 0f ? sr.flipX : (h < 0f ? true : false);
		transform.Translate (Vector3.right * h * Time.deltaTime * speed);
	}

	void Jump() {
		if (Input.GetButtonDown ("Jump") && grounded) {
			rb.AddForce (Vector2.up * jumpHeight, ForceMode2D.Impulse);
		}
	}
}
