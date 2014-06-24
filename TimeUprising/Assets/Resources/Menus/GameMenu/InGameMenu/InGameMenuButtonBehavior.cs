using UnityEngine;
using System.Collections;

public abstract class InGameMenuButtonBehavior : ButtonBehaviour {

	public Sprite AltButton;
	public Sprite AltButtonOver;
	protected bool mMuted;

	protected void OnMouseDown(){
		ChangeButtons();
		MuteUnmute();
	}
	protected void ChangeButtons(){
		Sprite tempButton = mButton;
		Sprite tempButtonOver = mButtonOver;
		mButton = AltButton;
		mButtonOver = AltButtonOver;
		AltButton = tempButton;
		AltButtonOver = tempButtonOver;
	}
	public abstract void MuteUnmute();

}
