using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	public Transform bumpCheck;
	public AudioClip bump;

	AudioSource audioSource;
	Animator anim;

	void Start () {
		audioSource = gameObject.AddComponent<AudioSource> ();
		audioSource.playOnAwake = false;
		anim = GetComponent<Animator> ();
	}

	void OnCollisionEnter2D(Collision2D c) {
		int layerMask = 1 << LayerMask.NameToLayer ("Player");
		if (Physics2D.OverlapBox (bumpCheck.position, new Vector2(0.5f, 0f), 0.0f, layerMask)) {
			if (c.gameObject.tag == "Player") {
				audioSource.clip = bump;
				audioSource.Play ();
				anim.SetTrigger ("bump");
			}
		}
	}
}
