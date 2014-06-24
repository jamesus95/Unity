using UnityEngine;
using System.Collections;

public class CancelQuitButton : ButtonBehaviour {

	public GUIText mText;
	public GameObject ConfirmQuitFrame;
	// Use this for initialization
	void Start () {
		mText.text = "ARE YOU SURE YOU WANT TO QUIT?\nGAME PROGRESS WILL BE LOST!";
	}

	void OnMouseDown(){
		ChangeScreen();
		ConfirmQuitFrame.SetActive(false);
		Time.timeScale = 1;
	}
}
