using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarioController : MonoBehaviour {

	public static MarioController instance;

	public float speed = 3.0f;
	public float jumpHeight = 3.0f;
	public float bounceHeight = 2.0f;
	public Transform groundCheck, headCheck;
	public bool super, starPower;
	public AudioClip smallJump, superJump;

	Animator anim;
	SpriteRenderer sr;
	Rigidbody2D rb;
	ArrayList headBlocks;
	AudioSource audioSource;

	int groundLayer;
	float width;
	float height;
	public bool grounded;

	/* System functions */
	void Awake() {
		// Screen.SetResolution (1280, 720, false);
		if (instance == null) {
			DontDestroyOnLoad (gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	void OnEnable() {
		Start();
	}

	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator> ();
		sr = GetComponentInChildren<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();

		groundLayer = 1 << LayerMask.NameToLayer ("Ground") | 1 << LayerMask.NameToLayer("Block");
		grounded = true;
		super = false;
		starPower = false;

		sr.sortingOrder = 32767;

		width = sr.bounds.size.x;
		height = sr.bounds.size.y;

		headBlocks = new ArrayList();
		audioSource = gameObject.AddComponent<AudioSource> ();
		audioSource.playOnAwake = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!Global.instance.death && !Global.instance.win) {
			Move ();
			Jump ();
		} else {
			if (Global.instance.death) {
				anim.SetBool ("death", true);
			}
		}
	}

	void FixedUpdate() {
		if (!Global.instance.death) {
			grounded = Physics2D.OverlapBox (groundCheck.position, new Vector2(width*0.95f, 0.01f), 0.0f, groundLayer);
			if (grounded || Global.instance.win) {
				anim.SetBool ("jump", false);
			} else {
				anim.SetBool ("jump", true);
			}
		}	
	}

	void OnCollisionEnter2D(Collision2D c) {
		int blockLayer = 1 << LayerMask.NameToLayer ("Block");
		if (Physics2D.OverlapBox (headCheck.position, new Vector2 (width, 0.01f), 0.0f, blockLayer)) {
			headBlocks.Add (c.transform);
		}
	}

	void OnCollisionExit2D(Collision2D c) {
		if (c.gameObject.layer == LayerMask.NameToLayer("Block")) {
			ProcessHeadBlocks ();
		}
	}

	/* Private functions */
	void ProcessHeadBlocks() {
		float minxdist = Mathf.Infinity;
		int minxindex = 0;
		for (int i = 0; i < headBlocks.Count; i++) {
			Transform c = headBlocks [i] as Transform;
			float xdist = Mathf.Abs(c.position.x - transform.position.x);
			if (xdist < minxdist) {
				minxdist = xdist;
				minxindex = i;
			}
		}
		if (headBlocks.Count > 0) {
			Block b = ((Transform)headBlocks [minxindex]).GetComponent<Block> ();
			b.Hit ();
		}
		headBlocks.Clear ();
	}

	void Move() {
		float h = Input.GetAxis ("Horizontal");

		anim.SetFloat ("speed", Mathf.Abs(h));
		sr.flipX = h == 0f ? sr.flipX : (h < 0f ? true : false);
		rb.velocity = new Vector2(h * speed, rb.velocity.y);
		// transform.Translate (Vector3.right * h * Time.deltaTime * speed);
	}

	void Jump() {
		if ((Input.GetButtonDown ("Jump") || Input.GetButtonDown("Up"))&& grounded) {
			Bounce (jumpHeight);
			audioSource.clip = super ? superJump : smallJump;
			audioSource.Play ();
		}
	}

	/* Coroutines */
	IEnumerator DeathAnimation(float t, bool bounce) {
		yield return new WaitForSeconds (t);
		if (bounce) {
			Bounce (2.5f);
		}
		transform.GetComponent<Collider2D> ().enabled = false;
		yield return new WaitForSeconds (Global.instance.deathAudio.length);
		Global.instance.ResetWorld ();
	}

	IEnumerator DisableInvincible(float t) {
		yield return new WaitForSeconds (t);
		gameObject.layer = LayerMask.NameToLayer ("Player");
		sr.enabled = true;
	}

	IEnumerator FlashSprite(float t) {
		if (gameObject.layer == LayerMask.NameToLayer ("Invincible")) {
			sr.enabled = !sr.enabled;
			yield return new WaitForSeconds (t);
			StartCoroutine (FlashSprite (t));
		}
	}

	/* Public functions */
	public void Bounce() {
		rb.AddForce (Vector2.up * bounceHeight, ForceMode2D.Impulse);
	}

	public void Bounce(float height) {
		rb.AddForce (Vector2.up * height, ForceMode2D.Impulse);
	}

	public void EnterPipe(Transform returnPipe, Transform target, string triggerType) {
		anim.SetTrigger (triggerType);
		// Global.instance.WarpToMap (target, returnPipe);
	}

	public void Revive() {
		Super (false);
		anim.SetBool ("death", false);
		transform.GetComponent<Collider2D> ().enabled = true;
		rb.velocity = Vector2.zero;
	}

	public void Die(bool bounce) {
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		anim.SetBool ("death", true);
		anim.SetBool ("jump", false);
		StartCoroutine (DeathAnimation (0.1f, bounce));
		Camera.main.GetComponent<AudioSource> ().Stop ();
		Global.instance.DeathAudio ();
		Global.instance.death = true;
	}

	public void Mini() {
		Super (false);
		Invincible (2.5f);
		Global.instance.PowerDownAudio ();
	}

	public void Super(bool s) {
		super = s;
		anim.SetBool ("super", super);
		if (super) {
			GetComponent<BoxCollider2D> ().size = new Vector2 (width, 1.0f);
		} else {
			GetComponent<BoxCollider2D> ().size = new Vector2 (width, 0.5f);
		}
		height = GetComponent<BoxCollider2D>().size.y;

		groundCheck.localPosition = new Vector2 (0f, -height / 2);
		headCheck.localPosition = new Vector2 (0f, height / 2);
	}

	public void Invincible(float t) {
		gameObject.layer = LayerMask.NameToLayer ("Invincible");
		StartCoroutine (FlashSprite (0.1f));
		StartCoroutine (DisableInvincible (t));
	}

	public void ResetTrigger(string trig) {
		anim.ResetTrigger (trig);
	}

	public void Flag() {
		Global.instance.win = true;
		rb.isKinematic = true;
		rb.velocity = Vector2.zero;
		anim.SetTrigger ("flag");
	}
}
