using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

	public float speed = 6f;

	// Use this for initialization
	void Start () {
		StartCoroutine (DieAfterSeconds (3f));
	}
	
	void OnCollisionEnter2D(Collision2D c) {
		if (c.transform.tag == "Enemy") {
			c.gameObject.GetComponent<Enemy> ().Death (false);
			Destroy (gameObject);
		}
	}

	public void Initialize(Vector2 dir) {
		GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
	}

	IEnumerator DieAfterSeconds(float t) {
		yield return new WaitForSeconds(t);
		Destroy (gameObject);
	}
}
