using UnityEngine;
using System.Collections;

public class WinLoseTimelineButton : ButtonBehaviour {

	void OnMouseDown(){
		GameState.WonGame = GameState.LostGame = false;
		Application.LoadLevel("LevelLoader");
	}
}
