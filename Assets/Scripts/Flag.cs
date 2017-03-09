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
			Global.instance.win = true;
			MainCamera.instance.Audio (false);
			StartCoroutine (StartSlideDownFlag (0.5f));
		}
	}

	void SlideDownFlag() {
		float ydel = MarioController.instance.super ? 0.75f : 0.5f;
		if (slide && MarioController.instance.transform.position.y > flagBase.position.y + ydel) {
			MarioController.instance.transform.Translate (Vector2.down * Time.deltaTime * 3f);
		}
		/*if (slide && flag.position.y > flagBase.position.y + 0.5f) {
			flag.Translate (Vector2.down * Time.deltaTime * 2f);
		} else {
			slide = false;
		}*/
	}

	IEnumerator StartSlideDownFlag(float t) {
		yield return new WaitForSeconds(t);
		slide = true;
		GetComponent<AudioSource> ().Play ();
		flag.GetComponent<Animator> ().SetTrigger ("drop");
		StartCoroutine (WaitForStageClear (1.5f));
	}

	IEnumerator WaitForStageClear(float t) {
		yield return new WaitForSeconds(t);
		Global.instance.StageClearAudio ();
		UI.instance.WinText ();
	}
}
