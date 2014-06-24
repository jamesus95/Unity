using UnityEngine;
using System.Collections;

public class CutScenePreviousButton : ButtonBehaviour {

	public StoryBook mStoryBook;
	void OnMouseDown(){
		mStoryBook.PreviousFrame();
	}
}
