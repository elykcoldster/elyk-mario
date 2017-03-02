using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMushroom : MonoBehaviour {

	bool move;
	float moveSpeed;
	Vector2 dir;
	int selfLayer;

	public Transform bumpCheck;

	void Start () {
		GetComponent<Animator> ().SetTrigger ("rise");
		GetComponent<AudioSource> ().Play ();

		move = false;
		moveSpeed = 1f;
		dir = Vector2.right;
		selfLayer = ~(1 << LayerMask.NameToLayer ("Item"));

		StartCoroutine (StartMovement (1f));
	}

	void Update() {
		if (move) {
			transform.Translate (dir * moveSpeed * Time.deltaTime);
			Collider2D c = Physics2D.OverlapBox (bumpCheck.position, new Vector2 (0.5f, 0f), 0f, selfLayer);
			if (c) {
				if (c.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
					dir.x = -dir.x;
				} else if (c.gameObject.layer == LayerMask.NameToLayer ("Player")) {
					Global.instance.PowerUpAudio ();
					Destroy (gameObject);
				}
			}
		}
	}

	IEnumerator StartMovement(float t) {
		yield return new WaitForSeconds(t);
		GetComponent<Animator> ().enabled = false;
		move = true;
	}
}
