using UnityEngine;
using System.Collections;
//using UnityEditor;

public class WinLoseReplayButton : ButtonBehaviour {
	void OnMouseDown(){
		GameState.WonGame = GameState.LostGame = false;
		Application.LoadLevel(Application.loadedLevelName);
	}
}
