using UnityEngine;
using System.Collections;

public class CutSceneQuitButton : ButtonBehaviour {

	void OnMouseDown(){
		Application.LoadLevel("Menu");
	}
}
