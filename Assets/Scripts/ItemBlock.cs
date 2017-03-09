using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlock : Block {

	public AudioClip coinSound;
	public GameObject item;
	public GameObject alternateItem;

	bool active;

	void Start () {
		base.Start ();
		active = true;
	}

	void SpawnItem() {
		if (MarioController.instance.super && item.name == "Super Mushroom" && alternateItem) {
			Instantiate (alternateItem, transform.position, Quaternion.identity);
		} else {
			Instantiate (item, transform.position, Quaternion.identity);
		}
	}

	public override void Hit() {
		if (active) {
			anim.SetTrigger ("bump");
			audioSource.clip = coinSound;
			audioSource.Play ();
			active = false;
			SpawnItem ();
		} else {
			audioSource.clip = bump;
			audioSource.Play ();
		}
	}

	public void Reset() {
		active = true;
		anim.SetTrigger ("reset");
		StartCoroutine (ResetAnimTrigger(Time.deltaTime));
	}

	IEnumerator ResetAnimTrigger(float t) {
		yield return new WaitForSeconds (t);
		anim.ResetTrigger ("reset");
	}
}
