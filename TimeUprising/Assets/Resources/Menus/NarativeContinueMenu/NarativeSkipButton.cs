using UnityEngine;
using System.Collections;

public class NarativeSkipButton : ButtonBehaviour {

	void OnMouseDown(){
		ChangeScreen();
		Application.LoadLevel("Level1");
	}
}
