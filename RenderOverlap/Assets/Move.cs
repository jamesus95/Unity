using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 move = new Vector3 (Input.GetAxis ("Horizontal") * 3f * Time.smoothDeltaTime,
		                            Input.GetAxis ("Vertical") * 3f * Time.smoothDeltaTime,
		                            0);
		transform.position += move;

	}

	private float getZ(float y) {
		return y / 10;
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("collide");
	}

}
