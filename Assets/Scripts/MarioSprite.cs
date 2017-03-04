using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioSprite : MonoBehaviour {

	SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
	}

	public void SetSpriteOrder(int order) {
		sr.sortingOrder = order;
	}
}
