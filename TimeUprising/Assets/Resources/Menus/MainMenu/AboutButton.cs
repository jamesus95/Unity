using UnityEngine;
using System.Collections;

public class AboutButton : ButtonBehaviour {

	public GameObject mMainMenuObject;
	public GameObject mAboutObject;
	public GUIText mText;

	void OnMouseDown(){
		mMainMenuObject.SetActive(false);
		mAboutObject.SetActive(true);
		ChangeScreen();
	}
}
