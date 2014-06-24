using UnityEngine;
using System.Collections;
using System;

public class WinSkipButton : ButtonBehaviour {

    public GameScenes mScenes;
    void OnMouseDown(){
		GameState.WonGame = GameState.LostGame = false;
        if(mScenes == null)
            Application.LoadLevel("LevelLoader");
        Application.LoadLevel(mScenes.GetNextLevelOnly(Application.loadedLevelName));
	}
}
