using UnityEngine;
using System.Collections;

public class SetRendererLayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<SpriteRenderer> ().sortingOrder = (int)(-transform.position.y * 100);
	}
}
