using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMushroom : MonoBehaviour {

	bool move;
	float moveSpeed;
	int selfLayer;

	Vector2 dir;
	Rigidbody2D rb;

	public Transform bumpCheck;
	public int points;

	void Start () {
		GetComponent<Animator> ().SetTrigger ("rise");
		GetComponent<AudioSource> ().Play ();

		rb = GetComponent<Rigidbody2D> ();

		move = false;
		moveSpeed = 1f;
		dir = Vector2.right;
		selfLayer = ~(1 << LayerMask.NameToLayer ("Item"));

		StartCoroutine (StartMovement (1f));
	}

	void Update() {
		if (move) {
			rb.velocity = new Vector2(dir.x, rb.velocity.y);
		}
		/*if (move) {
			transform.Translate (dir * moveSpeed * Time.deltaTime);
			Collider2D c = Physics2D.OverlapBox (bumpCheck.position, new Vector2 (0.5f, 0f), 0f, selfLayer);

			if (c) {
				if (c.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
					dir.x = -dir.x;
				} else if (c.gameObject.layer == LayerMask.NameToLayer ("Player")) {
					Global.instance.PowerUpAudio ();
					Global.instance.mario.Super (true);
					Global.instance.GetPoints (points);
					Destroy (gameObject);
				}
			}
		}*/
	}

	void OnCollisionEnter2D (Collision2D c) {
		if (c.gameObject.tag == "Wall") {
			dir.x = -dir.x;
		} else if (c.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			Global.instance.PowerUpAudio ();
			Global.instance.mario.Super (true);
			Global.instance.GetPoints (points);
			Destroy (gameObject);
		}
	}

	IEnumerator StartMovement(float t) {
		yield return new WaitForSeconds(t);
		move = true;
		GetComponent<Animator> ().enabled = false;
	}
}
