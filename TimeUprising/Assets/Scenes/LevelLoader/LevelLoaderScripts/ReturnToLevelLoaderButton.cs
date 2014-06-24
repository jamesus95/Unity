using UnityEngine;
using System.Collections;

public class ReturnToLevelLoaderButton : ButtonBehaviour {

	public GameObject EraButtons;
	public GameObject LevelSelectionButtons;

	void OnMouseDown(){
		ChangeScreen();
		EraButtons.SetActive(true);
		LevelSelectionButtons.SetActive(false);
	}
}
