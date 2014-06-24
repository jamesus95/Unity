using UnityEngine;
using System.Collections;
using System.IO;
public class NewGameButton : ButtonBehaviour {
	//public GameObject mSaveMenuObject; //uncomment after save load is working

	//remove after save / load is working
	public GameObject mNarativeContinueFrame;
	public GameObject mMenuObject;
    void OnMouseDown(){
        SaveLoad.ResetGameState ();

		Application.LoadLevel("TutorialCutScene");
		//remove after save load is working
		//mNarativeContinueFrame.SetActive(true);
		//mMenuObject.SetActive(false);
		//ChangeScreen();
		
	}

}

