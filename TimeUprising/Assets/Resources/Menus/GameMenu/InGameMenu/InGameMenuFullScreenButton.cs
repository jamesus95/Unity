using UnityEngine;
using System.Collections;

public class InGameMenuFullScreenButton : ButtonBehaviour {

	void OnMouseDown(){
		if(!Screen.fullScreen)
			Screen.fullScreen = true;
	}

}
