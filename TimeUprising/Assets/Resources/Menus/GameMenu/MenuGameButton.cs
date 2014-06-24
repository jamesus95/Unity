using UnityEngine;
using System.Collections;

public class MenuGameButton : ButtonBehaviour {

	// Use this for initialization
	const float kMenuInterval = 0.3f;

	public WinLoseBehavior mButtons;
	public GameObject GameMenuFrame;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
			OnMouseDown();
	}
	void OnMouseDown(){
		if(Time.timeScale == 1 && !GameState.WonGame && !GameState.LostGame){
			Time.timeScale = 0;
			GameMenuFrame.SetActive(true);
		}
	}

}
