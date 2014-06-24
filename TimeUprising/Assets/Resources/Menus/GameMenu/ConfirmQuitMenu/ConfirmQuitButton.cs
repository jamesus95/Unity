using UnityEngine;
using System.Collections;

public class ConfirmQuitButton : ButtonBehaviour {

	public DestinationBehavior destination;

	void OnMouseDown(){
		ChangeScreen();
		Time.timeScale = 1;
		Application.LoadLevel(destination.mDestination);
	}

}
