﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public float cameraSpeed = 2.0f;
	public float cameraHeight = 2.5f;
	public Transform target;

	// Use this for initialization
	void Start () {
		if (!target) {
			target = GameObject.FindGameObjectWithTag ("Player").transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!Global.instance.death) {
			transform.position = Vector3.Lerp (transform.position, new Vector3(target.position.x, cameraHeight, transform.position.z), cameraSpeed * Time.deltaTime);
		}
	}
}
