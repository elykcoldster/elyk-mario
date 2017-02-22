using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	public Transform bumpCheck;
	public AudioClip bump;

	protected AudioSource audioSource;
	Animator anim;

	protected void Start () {
		audioSource = gameObject.AddComponent<AudioSource> ();
		audioSource.playOnAwake = false;
		anim = GetComponent<Animator> ();
	}

//	void OnCollisionEnter2D(Collision2D c) {
		
//	}

	public virtual void Hit() {
		if (!Global.instance.mario.super) {
			audioSource.clip = bump;
			audioSource.Play ();
			anim.SetTrigger ("bump");
		}
	}
}
