using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour {

	public static Global instance;

	public MarioController mario;
	public Transform player;
	public Transform tiles;
	public GameObject tile;
	public Vector2 startingPosition = Vector2.zero;

	public AudioClip deathAudio, stomp, smallJump, superJump, bump, breakBlock, powerUp, powerDown;

	public bool death = false, pause = false;

	Transform returnLocation;

	AudioSource audioSource;

	void Awake() {
		// Screen.SetResolution (1280, 720, false);
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

	void AudioPlay(AudioClip a) {
		audioSource.clip = a;
		audioSource.Play ();
	}

	public void ResetWorld() {
		ResetPosition ();
		foreach (GameObject item in GameObject.FindGameObjectsWithTag ("Item")) {
			item.GetComponent<ItemBlock> ().Reset ();
		}
		foreach (GameObject block in GameObject.FindGameObjectsWithTag("Block")) {
			block.GetComponent<Block> ().SetVisible (true);
		}
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
			enemy.GetComponent<Enemy> ().Revive ();
		}
	}

	public void ResetPosition() {
		death = false;
		player.position = startingPosition;
		mario.Revive ();
		Camera.main.GetComponent<AudioSource> ().Play ();
		Camera.main.GetComponent<CameraFollow> ().ResetPosition ();
	}

	public void JumpAudio(bool jumpType) {
		AudioPlay (jumpType ? superJump : smallJump);
	}

	public void StompAudio() {
		AudioPlay (stomp);
	}

	public void DeathAudio() {
		AudioPlay (deathAudio);
	}

	public void BumpAudio() {
		AudioPlay (bump);
	}

	public void PowerUpAudio() {
		AudioPlay (powerUp);
	}

	public void PowerDownAudio() {
		AudioPlay (powerDown);
	}

	public void WarpToMap(string mapName, Transform returnLocation) {
		SetReturn (returnLocation);
		SceneManager.LoadScene (mapName);
	}

	public void SetReturn(Transform r) {
		returnLocation = r;
	}
}
