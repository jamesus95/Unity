using UnityEngine;
using System.Collections;

public class PauseMenuTimelineButton :  ButtonBehaviour {

	public GameObject mConfirmQuitFrame;
	public GameObject mPauseMenuFrame;
	public DestinationBehavior destination;
	
	void OnMouseDown(){
		ChangeScreen();
		destination.mDestination = "LevelLoader";
		mConfirmQuitFrame.SetActive(true);
		mPauseMenuFrame.SetActive(false);
	}
}
