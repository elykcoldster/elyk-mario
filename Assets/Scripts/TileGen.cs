using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGen : MonoBehaviour {

	public GameObject tile;

	SpriteRenderer sr;

	Vector2 tileDims;
	int nx, ny;

	// Use this for initialization
	void Start () {
		InitParams ();
		InitTiles ();
	}

	void InitParams() {
		sr = tile.GetComponent<SpriteRenderer> ();
		tileDims = sr.bounds.size;
		nx = (int)(transform.localScale.x / tileDims.x);
		ny = (int)(transform.localScale.y / tileDims.y);
	}

	void InitTiles() {
		for (int x = 0; x < nx; x++) {
			for (int y = 0; y < ny; y++) {
				Transform t = (Instantiate (tile) as GameObject).transform;
				t.parent = transform;
				t.localPosition = new Vector2 (-0.5f + (float)x / nx, 0f - (float)y / ny);
				t.position = new Vector2 (t.position.x + tileDims.x / 2, t.position.y + tileDims.y / 2);
			}
		}
	}
}
