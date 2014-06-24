using UnityEngine;
using System.Collections;

public class CurrentSceneText : MonoBehaviour {

	public StoryBook mStoryBook;
	public GUIText mText;
	// Use this for initialization
	void Start () {
		mText.text = "1";
	}
	
	// Update is called once per frame
	void Update () {
		mText.text = mStoryBook.GetCurrentImage().ToString();
	}
}
