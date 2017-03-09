using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	public static UI instance;
	public Text points, coins, winText;

	void Awake() {
		if (instance == null) {
			DontDestroyOnLoad (gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	public void SetPoints(int p) {
		points.text = p.ToString ("000000");
	}

	public void SetCoins(int c) {
		coins.text = "x" + c.ToString ("00");
	}

	public void WinText() {
		winText.enabled = true;
	}
}
