using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

	public Transform flagBase, flag;

	bool slide;

	// Use this for initialization
	void Start () {
		slide = false;
	}
	
	// Update is called once per frame
	void Update () {
		SlideDownFlag ();
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.tag == "Player") {
			Global.instance.mario.Flag ();
			StartCoroutine (StartSlideDownFlag (0.5f));
		}
	}

	void SlideDownFlag() {
		if (slide && MarioController.instance.transform.position.y > flagBase.position.y + 0.5f) {
			MarioController.instance.transform.Translate (Vector2.down * Time.deltaTime * 2f);
		}

		if (slide && flag.position.y > flagBase.position.y + 0.5f) {
			flag.Translate (Vector2.down * Time.deltaTime * 2f);
		}
	}

	IEnumerator StartSlideDownFlag(float t) {
		yield return new WaitForSeconds(t);
		slide = true;
	}
}
