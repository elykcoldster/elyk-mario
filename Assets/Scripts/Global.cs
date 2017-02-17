using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {

	public static Global instance;

	public Transform player;
	public Transform tiles;
	public GameObject tile;

	void Awake() {
		if (instance == null) {
			DontDestroyOnLoad (gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GenerateGround(int length) {
		for (int i = 0; i < length; i++) {
			GameObject.Instantiate (tile, new Vector2(-10f + 0.5f*i, -3f), Quaternion.identity, tiles);
		}
	}
}
