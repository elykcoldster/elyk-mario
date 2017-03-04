using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (DieAfterTime (1.5f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator DieAfterTime(float t) {
		yield return new WaitForSeconds (t);
		Destroy (gameObject);
	}
}
