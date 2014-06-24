using UnityEngine;
using System.Collections;
//using UnityEditor;

public class WinContinueButton : ButtonBehaviour {



    public GameScenes mScenes;

	void OnMouseDown(){
		GameState.WonGame = GameState.LostGame = false;
        if(mScenes == null)
            Application.LoadLevel("LevelLoader");
		Application.LoadLevel(mScenes.GetNextLevelAndCutScenes(Application.loadedLevelName));
	}
}
