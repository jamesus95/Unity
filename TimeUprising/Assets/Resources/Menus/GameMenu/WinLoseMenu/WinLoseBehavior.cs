using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WinLoseBehavior : MonoBehaviour {

	public GameObject WinGameFrame;
	public GameObject LoseGameFrame;
	public List<GameObject> mAllFrames;
    	
	void Awake(){
        GameState.WonGame = GameState.LostGame = false;
		CloseAllFrames();
	}
	// Update is called once per frame
	void Update () {
        if(GameState.IsDebug) {
            if(Input.GetKeyDown(KeyCode.W))
                GameState.WonGame = true;
            if(Input.GetKeyDown(KeyCode.L))
                GameState.LostGame = true;
        }

        if (GameState.WonGame){
            GameObject.Find("Background").GetComponent<AudioSource>().mute = true;
			WinGameFrame.SetActive(true);
        }
		if(GameState.LostGame){
            GameObject.Find("Background").GetComponent<AudioSource>().mute = true;
            LoseGameFrame.SetActive(true);
        }

	}
	public void CloseAllFrames(){
		for(int i = 0; i < mAllFrames.Count; i++){
			mAllFrames[i].SetActive(false);
		}
	}
}
