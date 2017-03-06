using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	public Transform ground;
	public Transform spawnPoint;
	public Color backgroundColor;
	public AudioClip bgm;

	// Use this for initialization
	void Start () {
		if (Global.instance.returnLocation == -1) {
			Global.instance.spawnPoint = spawnPoint;
		} else {
			Global.instance.spawnPoint = Global.instance.GetPipe(Global.instance.returnLocation);
		}
		Global.instance.ResetPosition ();
		MainCamera.instance.SetColor (backgroundColor);
		MainCamera.instance.SetBGM (bgm);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
