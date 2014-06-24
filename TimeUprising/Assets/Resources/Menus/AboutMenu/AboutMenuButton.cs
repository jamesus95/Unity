using UnityEngine;
using System.Collections;

public enum Buttons{
	AboutButton,
	MenuButton,
	NewGameButton,
	LoadButton,
	Continue,
	Back,
};

public class AboutMenuButton : ButtonBehaviour {
	
	public GameObject mAboutObject;
	public GameObject mMainMenuObject;
    public GameObject mAboutScreen2;
    public GameObject mAboutScreen1;
    public GameObject mNextScreenButton;
    
//	public Buttons myButton;

	// Use this for initialization
	void Start () {
        mAboutScreen2.SetActive(false);
	}

	void OnMouseDown(){
        ChangeScreen();
        if(!mAboutScreen2.activeSelf){
		    mMainMenuObject.SetActive(true);
		    mAboutObject.SetActive(false);
        }
        else{
            mNextScreenButton.SetActive(true);
            mAboutScreen2.SetActive(false);
            mAboutScreen1.SetActive(true);
        }
	}
}
