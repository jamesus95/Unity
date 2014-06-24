using UnityEngine;
using System.Collections;

public class LoseMenuButton : ButtonBehaviour {
	void OnMouseDown(){
		GameState.WonGame = GameState.LostGame = false;
		Application.LoadLevel("Menu");
	}
}
