using UnityEngine;
using System.Collections;

public class PauseMenuQuitButton : ButtonBehaviour {

	public GameObject mConfirmQuitFrame;
	public GameObject mPauseMenuFrame;
	public DestinationBehavior destination;

	void OnMouseDown(){
			ChangeScreen();
			destination.mDestination = "Menu";
			mConfirmQuitFrame.SetActive(true);
			mPauseMenuFrame.SetActive(false);
	}
}
