using UnityEngine;
using System.Collections;

public class InGameMenuMusicButton : InGameMenuButtonBehavior {

	float mSliderValue = 100.0f;
	float mPreviousValue = 0.0f;
	public GUIText VolumeReadout;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		VolumeReadout.text =((int)mSliderValue).ToString();
		GameObject.Find("GameManager").GetComponent<ControlPanel>().SetMusicVolume(mSliderValue/100.0f);
	}
	void OnGUI() {
		mSliderValue = GUI.HorizontalSlider(new Rect(240f, 250f, 200, 20), mSliderValue, 0.0f, 100.0f);		
	}
	public override void MuteUnmute ()
	{	
		if(mMuted)
			Unmute();
		else
			Mute();
	}
	void Mute(){
		mPreviousValue = mSliderValue;
		mSliderValue = 0.0f;
		mMuted = true;
	}
	void Unmute(){
		mSliderValue = mPreviousValue;
		mMuted = false;
	}

}
