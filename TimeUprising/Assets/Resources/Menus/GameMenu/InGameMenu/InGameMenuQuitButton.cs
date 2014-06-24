using UnityEngine;
using System.Collections;

public class InGameMenuQuitButton : ButtonBehaviour {

	public GameObject mInGameMenuFrame;
	public GameObject mConfirmQuitFrame;
	public DestinationBehavior destination;

	void OnMouseDown(){
		ChangeScreen();
		destination.mDestination = "Menu";
		mConfirmQuitFrame.SetActive(true);
		mInGameMenuFrame.SetActive(false);
	}
}
