using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour {

	public static Global instance;
	public static int coins, points;

	public MarioController mario;
	public Transform spawnPoint;
	public Transform player;
	public Transform tiles;
	public GameObject tile;

	public AudioClip deathAudio, stomp, smallJump, superJump, bump, breakBlock, powerUp, powerDown, stageClear;

	public bool death = false, pause = false, win = false, control = true;

	public Map activeMap;

	public int returnLocation;

	bool right, left;

	AudioSource audioSource;

	void Awake() {
		Screen.SetResolution (1280, 960, false);
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

	void Update() {
		if (death || pause || win) {
			control = false;
		} else {
			control = true;
		}
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
		player.position = spawnPoint.position;
		mario.Revive ();
		Camera.main.GetComponent<AudioSource> ().Play ();
		MainCamera.instance.ResetPosition ();
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

	public void StageClearAudio() {
		AudioPlay (stageClear);
	}
//	public void WarpToMap(Transform target, Transform returnLocation) {
//		StartCoroutine (Warp (1f, target, returnLocation));
//		// SceneManager.LoadScene (mapName);
//	}

	public void SetReturn(int r) {
		returnLocation = r;
	}

	public Transform GetPipe(int p) {
		WarpPipe[] warpPipes = (WarpPipe[]) GameObject.FindObjectsOfType (typeof(WarpPipe));
		if (warpPipes.Length > 0) {
			foreach (WarpPipe wp in warpPipes) {
				if (wp.warpPipeNumber == p) {
					return wp.transform;
				}
			}
			return null;
		} else {
			return null;
		}
	}

	public void EnterPipe(int returnPipeNumber, string target, string pipeAnim) {
		SetReturn (returnPipeNumber);
		SceneManager.LoadScene (target);
	}

	public void EnterPipe(string target, string pipeAnim) {
		SetReturn (-1);
		SceneManager.LoadScene (target);
	}

	public void GetCoin() {
		coins++;
		UI.instance.SetCoins (coins);
	}

	public void GetPoints(int p) {
		points += p;
		UI.instance.SetPoints (points);
	}

//	IEnumerator Warp(float t, Transform target, Transform returnLocation) {
//		yield return new WaitForSeconds (t);
//		SetReturn (returnLocation);
//
//		player.position = target.position;
//		Map map = target.GetComponentInParent<Map> ();
//		MainCamera.instance.cameraHeight = map.ground.position.y + Camera.main.orthographicSize - 0.5f;
//		Camera.main.backgroundColor = map.backgroundColor;
//	}
}
