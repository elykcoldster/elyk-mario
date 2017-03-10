using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardBlock : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D c) {
		if (c.gameObject.layer == LayerMask.NameToLayer("Fireball")) {
			// Destroy (c.gameObject);
		}
	}
}
