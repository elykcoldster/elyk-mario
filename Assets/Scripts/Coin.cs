using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	public int points;

	void Start () {
		GetComponent<Animator> ().SetTrigger ("appear");
		Global.instance.GetCoin ();
		Global.instance.GetPoints (points);
		StartCoroutine (Disappear (0.1f));
	}

	IEnumerator Disappear(float t) {
		yield return new WaitForSeconds(t);
		Destroy (gameObject);
	}
}
