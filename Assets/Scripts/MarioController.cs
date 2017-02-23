using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioController : MonoBehaviour {

	public float speed = 3.0f;
	public float jumpHeight = 3.0f;
	public float bounceHeight = 2.0f;
	public Transform groundCheck, headCheck;
	public bool super;

	Animator anim;
	SpriteRenderer sr;
	Rigidbody2D rb;
	ArrayList headBlocks;

	int groundLayer;
	float width;
	bool grounded;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		sr = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();

		groundLayer = 1 << LayerMask.NameToLayer ("Ground") | 1 << LayerMask.NameToLayer("Block");
		grounded = true;
		super = false;

		sr.sortingOrder = 32767;

		width = sr.bounds.size.x;

		headBlocks = new ArrayList();
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
			grounded = Physics2D.OverlapBox (groundCheck.position, new Vector2(width, 0.01f), 0.0f, groundLayer);
			if (grounded) {
				anim.SetBool ("jump", false);
			} else {
				anim.SetBool ("jump", true);
			}
		}	
	}

	void OnCollisionEnter2D(Collision2D c) {
		int blockLayer = 1 << LayerMask.NameToLayer ("Block");
		if (Physics2D.OverlapBox (headCheck.position, new Vector2 (width, 0.01f), 0.0f, blockLayer)) {
			headBlocks.Add (c.transform);
		}
	}

	void OnCollisionExit2D(Collision2D c) {
		if (c.gameObject.layer == LayerMask.NameToLayer("Block")) {
			ProcessHeadBlocks ();
		}
	}

	void ProcessHeadBlocks() {
		float minxdist = Mathf.Infinity;
		int minxindex = 0;
		for (int i = 0; i < headBlocks.Count; i++) {
			Transform c = headBlocks [i] as Transform;
			float xdist = Mathf.Abs(c.position.x - transform.position.x);
			if (xdist < minxdist) {
				minxdist = xdist;
				minxindex = i;
			}
		}
		if (headBlocks.Count > 0) {
			Block b = ((Transform)headBlocks [minxindex]).GetComponent<Block> ();
			b.Hit ();
		}
		headBlocks.Clear ();
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
			Global.instance.JumpAudio (super);
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
		anim.SetBool ("jump", false);
		StartCoroutine (DeathAnimation (0.1f, bounce));
		Camera.main.GetComponent<AudioSource> ().Stop ();
		Global.instance.DeathAudio ();
		Global.instance.death = true;
	}
}
