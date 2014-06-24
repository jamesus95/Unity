using UnityEngine;
using System.Collections;

public class InGameMenuResumeButton : ButtonBehaviour {

	public GameObject GameMenuFrame;

	// Update is called once per frame
	void Update () {
		if(GameMenuFrame.activeSelf)
				if(Input.GetKeyDown(KeyCode.Escape))
					OnMouseDown();
	}

	void OnMouseDown(){
		Time.timeScale = 1;
		GameMenuFrame.SetActive(false);
	}
}
