using UnityEngine;
using System.Collections;

public class AboutMenuNextScreenButton : ButtonBehaviour {

    public GameObject mAboutMenuScreen1;
    public GameObject mAboutMenuScreen2;
    public GameObject mNextScreenButton;
    
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnMouseDown(){
        ChangeScreen();
        mAboutMenuScreen1.SetActive(false);
        mAboutMenuScreen2.SetActive(true);
        mNextScreenButton.SetActive(false);
    }

}
