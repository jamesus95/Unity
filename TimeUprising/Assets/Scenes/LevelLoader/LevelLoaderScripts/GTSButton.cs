using UnityEngine;
using System.Collections;

public class GTSButton : MonoBehaviour {

    public Sprite mButton;
    public Sprite mButtonOver;
    public GUIText mDisplayTextGUI;
    public string mDisplayText;


    void OnMouseOver(){
        mDisplayTextGUI.text = mDisplayText;
        gameObject.GetComponent<SpriteRenderer> ().sprite = mButtonOver;
    }
    void OnMouseExit(){
        mDisplayTextGUI.text = "";
        gameObject.GetComponent<SpriteRenderer> ().sprite = mButton;
    }
    void OnMouseDown(){
		Application.LoadLevel("TowerStore");
	}
}
