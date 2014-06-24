using UnityEngine;
using System.Collections;

public class PauseGameButton : ButtonBehaviour
{

		public GameObject mPauseMenuObject;
		public WinLoseBehavior mButtons;	
		const float kPauseInterval = 0.3f;
		
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (Input.GetKeyDown (KeyCode.Space))
				OnMouseDown ();
		}

		void OnMouseDown ()
		{
			if (Time.timeScale == 1 && !GameState.WonGame && !GameState.LostGame) {
				mPauseMenuObject.SetActive (true);
				Time.timeScale = 0;
				}
		}
}
