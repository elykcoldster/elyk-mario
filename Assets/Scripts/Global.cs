using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {

	public static Global instance;

	public MarioController mario;
	public Transform player;
	public Transform tiles;
	public GameObject tile;
	public Vector2 startingPosition = Vector2.zero;

	public AudioClip deathAudio, stomp, smallJump, superJump;

	public bool death = false;

	AudioSource audioSource;

	void Awake() {
		if (instance == null) {
			DontDestroyOnLoad (gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		audioSource = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () {
		ResetPosition();
	}

	void GenerateGround(int length) {
		for (int i = 0; i < length; i++) {
			GameObject.Instantiate (tile, new Vector2(-10f + 0.5f*i, -3f), Quaternion.identity, tiles);
		}
	}

	public void ResetPosition() {
		death = false;
		player.position = startingPosition;
		mario.Revive ();
	}

	public void JumpAudio(bool jumpType) {
		audioSource.clip = jumpType ? superJump : smallJump;
		audioSource.Play ();
	}

	public void StompAudio() {
		audioSource.clip = stomp;
		audioSource.Play ();
	}

	public void DeathAudio() {
		audioSource.clip = deathAudio;
		audioSource.Play ();
	}
}
