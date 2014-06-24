using UnityEngine;
using System.Collections;

public class ButtonBehaviour : MonoBehaviour {


	public Sprite mButton;
	public Sprite mButtonOver;
	void OnMouseOver(){
		gameObject.GetComponent<SpriteRenderer> ().sprite = mButtonOver;
	}
	void OnMouseExit(){
		gameObject.GetComponent<SpriteRenderer> ().sprite = mButton;
	}
	public void ChangeScreen(){
		gameObject.GetComponent<SpriteRenderer> ().sprite = mButton;
	}
}
