using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	public Transform bumpCheck;
	public GameObject[] debris = new GameObject[4];
	public AudioClip bump, brickBreak;

	protected AudioSource audioSource;
	protected Animator anim;

	SpriteRenderer sr;
	Collider2D[] colliders;

	protected void Start () {
		audioSource = gameObject.AddComponent<AudioSource> ();
		audioSource.playOnAwake = false;
		anim = GetComponent<Animator> ();
		sr = GetComponent<SpriteRenderer> ();
		colliders = GetComponents<Collider2D> ();
	}

//	void OnCollisionEnter2D(Collision2D c) {
		
//	}

	public virtual void Hit() {
		if (!Global.instance.mario.super) {
			audioSource.clip = bump;
			audioSource.Play ();
		} else {
			audioSource.clip = brickBreak;
			audioSource.Play ();
			SetVisible (false);
			Debris ();
		}
		anim.SetTrigger ("bump");
	}

	public void SetVisible(bool visible) {
		sr.enabled = visible;
		foreach (Collider2D c in colliders) {
			c.enabled = visible;
		}
	}

	public void Debris() {
		for (int i = 0; i < debris.Length; i++) {
			float x = i % 2 == 0 ? -0.125f : 0.125f;
			float y = i < 2 ? 0.125f : -0.125f;

			Transform d = (Instantiate (debris [i],
				new Vector2(transform.position.x + x, transform.position.y + y),
				Quaternion.identity
			) as GameObject).transform;

			Rigidbody2D drb = d.GetComponent<Rigidbody2D> ();
			float forceX = i % 2 == 0 ? -1f : 1f;
			float forceY = i < 2 ? 1f : 0.66f;
			drb.AddForce ((new Vector2(forceX, forceY)).normalized * 5f, ForceMode2D.Impulse);
		}
	}
}
