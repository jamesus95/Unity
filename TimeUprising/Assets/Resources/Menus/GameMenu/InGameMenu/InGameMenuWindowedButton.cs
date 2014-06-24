using UnityEngine;
using System.Collections;

public class InGameMenuWindowedButton : ButtonBehaviour {

	void OnMouseDown(){
		if(Screen.fullScreen)
			Screen.fullScreen = false;
	}

}
