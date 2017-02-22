using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	void Start () {
		GetComponent<Animator> ().SetTrigger ("appear");
		StartCoroutine (Disappear (0.1f));
	}

	IEnumerator Disappear(float t) {
		yield return new WaitForSeconds(t);
		Destroy (gameObject);
	}
}
