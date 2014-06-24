using UnityEngine;
using System.Collections;

public class PauseMenuResumeButton : ButtonBehaviour {

	public GameObject mPauseMenuFrame;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(mPauseMenuFrame.activeSelf)
			if(Input.GetKeyDown(KeyCode.Space))
					OnMouseDown();
	}
	void OnMouseDown(){
		ChangeScreen();
		mPauseMenuFrame.SetActive(false);
		Time.timeScale = 1;
	}
}
