using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

	public static MainCamera instance;

	public float cameraSpeed = 2.0f;
	public float cameraHeight = 2.5f;
	public float shiftDistance = 2f;
	public Transform target;

	void Awake() {
		// Screen.SetResolution (1280, 720, false);
		if (instance == null) {
			DontDestroyOnLoad (gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		if (!target) {
			target = GameObject.FindGameObjectWithTag ("Player").transform;
		}
		ResetPosition ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!Global.instance.death) {
			transform.position = Vector3.Lerp (transform.position, new Vector3(Mathf.Max(target.position.x, transform.position.x), cameraHeight, transform.position.z), cameraSpeed * Time.deltaTime);
		}
	}

	public void ResetPosition() {
		transform.position = new Vector3 (target.position.x, cameraHeight, transform.position.z);
	}

	public void SetColor(Color c) {
		GetComponent<Camera> ().backgroundColor = c;
	}
}
