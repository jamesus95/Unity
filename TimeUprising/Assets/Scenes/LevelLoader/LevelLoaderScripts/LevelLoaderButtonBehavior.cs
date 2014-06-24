using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum Era{
	Prehistoric = 0,
	Medieval = 1,
	Japanese = 2,
	ModernAmerica = 3,
	Future = 4,
	None = 5,
};
public enum EraLevel{
	_1 = 1,
	_2 = 2,
};
public class LevelLoaderButtonBehavior : MonoBehaviour {

	public Sprite mButton;
	public Sprite mButtonOver;
	public Sprite mLocked;
	public Sprite mLockedOver;

	public string mDisplayText;
	string mLockedText = "-Locked-";

	public Era mEra;

	public GameObject mLevelSelectionButtons;
	public GameObject mEraButtons;

	public bool mMultipleLevels;
	
	bool mLevelLocked;





	// Use this for initialization
	void Start () {
        //to be replaced with gamestate "testing".
        if(GameState.IsDebug)
    		GameState.CurrentEra = Era.Japanese;
        else
        {
            
        }
		
        if(mEra <= GameState.CurrentEra)
			mLevelLocked = false;
		else
			mLevelLocked = true;

		if(mLevelLocked){
			mButton = mLocked;
			mButtonOver = mLockedOver;
		}
		gameObject.GetComponent<SpriteRenderer> ().sprite = mButton;
	}
	void OnMouseOver(){
		gameObject.GetComponent<SpriteRenderer> ().sprite = mButtonOver;
		if(mLevelLocked){
			GameObject.Find("EraNameText").guiText.fontSize = 40;
			GameObject.Find("EraNameText").guiText.text = mLockedText;
		}
		else{
			GameObject.Find("EraNameText").guiText.fontSize = 30;
			GameObject.Find("EraNameText").guiText.text = mDisplayText;
		}
	}
	void OnMouseExit(){
		gameObject.GetComponent<SpriteRenderer> ().sprite = mButton;
		GameObject.Find("EraNameText").guiText.text = "";
		
	}
	public void ChangeScreen(){
		gameObject.GetComponent<SpriteRenderer> ().sprite = mButton;
	}	

	void OnMouseDown(){
		if(mEra <= GameState.CurrentEra){
			if(mMultipleLevels){
				ChangeScreen();	
				DifficultyLoader.mCurrentEra = mEra;
				mLevelSelectionButtons.SetActive(true);
				mEraButtons.SetActive(false);
				GameObject.Find("LevelPicture").GetComponent<SpriteRenderer>().sprite = mButton;
				mEraButtons.SetActive(false);
			}
			else{
				Application.LoadLevel(Enum.GetName(typeof(Era),mEra) + Enum.GetName(typeof(EraLevel), EraLevel._1));
			}
		}
	}
	public Era GetEra(){
		return mEra;
	}

}
