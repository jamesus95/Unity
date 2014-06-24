using UnityEngine;
using System.Collections;
using System;

public class LevelButton : ButtonBehaviour {

	public EraLevel mEraLevel;
	void OnMouseDown(){
		DifficultyLoader.mCurrentLevel = Enum.GetName(typeof(EraLevel), mEraLevel);
		DifficultyLoader.LoadGame();
	}
}
