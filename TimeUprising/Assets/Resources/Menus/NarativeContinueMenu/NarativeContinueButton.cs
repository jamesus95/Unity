using UnityEngine;
using System.Collections;

public class NarativeContinueButton : ButtonBehaviour {

	public GUIText mText;
	// Use this for initialization
	void Start () {
		mText.text = "WOULD YOU LIKE \nTO SKIP THE TUTORIAL?";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown(){
		Application.LoadLevel("Medieval0");
	}
}
