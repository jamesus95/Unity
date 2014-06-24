using UnityEngine;
using System.Collections;

public class BackButton : ButtonBehaviour {

	public GameObject mMainMenuObject;
	public GameObject mSaveMenuObject;

	void OnMouseDown(){
		mMainMenuObject.SetActive(true);
		mSaveMenuObject.SetActive(false);
		ChangeScreen();
	}

}
