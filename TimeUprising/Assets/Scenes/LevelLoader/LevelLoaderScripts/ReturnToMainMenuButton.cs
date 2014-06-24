using UnityEngine;
using System.Collections;

public class ReturnToMainMenuButton : MonoBehaviour {

    public GUIText mDisplayTextGUI;
    public string mDisplayText;
    public Sprite mButton;
    public Sprite mButtonOver;

    
    void OnMouseOver(){
        mDisplayTextGUI.text = mDisplayText;
        gameObject.GetComponent<SpriteRenderer> ().sprite = mButtonOver;
    }
    void OnMouseExit(){
        mDisplayTextGUI.text = "";
        gameObject.GetComponent<SpriteRenderer> ().sprite = mButton;
    }
    void OnMouseDown(){
		Application.LoadLevel("Menu");

	}
}
