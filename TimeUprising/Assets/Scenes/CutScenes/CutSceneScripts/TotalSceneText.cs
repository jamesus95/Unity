using UnityEngine;
using System.Collections;

public class TotalSceneText : MonoBehaviour {

	public StoryBook mStoryBook;
	public GUIText mText;
	// Use this for initialization

	void Start () {
		mText.text = mStoryBook.GetTotalImages().ToString();
	}
}
