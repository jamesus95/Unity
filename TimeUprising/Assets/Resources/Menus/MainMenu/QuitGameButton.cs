using UnityEngine;
using System.Collections;

public class QuitGameButton : ButtonBehaviour {

	void OnMouseDown(){
		Application.Quit();
	}
}
