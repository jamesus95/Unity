using UnityEngine;
using System.Collections;

public class ConfirmButton : ButtonBehaviour {
	void OnMouseDown()
	{ 
		ChangeScreen();
		Application.LoadLevel("Medieval0");	
	}
}
