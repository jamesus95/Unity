using UnityEngine;
using System.Collections;

public class GoldText : MonoBehaviour {

	public GUIText mText;
	// Use this for initialization
	void Start () {
	 
	}
	
	// Update is called once per frame
	void Update () {
		mText.text = GameState.Gold.ToString();
	}
}
