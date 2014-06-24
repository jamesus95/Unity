using UnityEngine;
using System.Collections;

public class CutSceneSkipButton : ButtonBehaviour {

	public StoryBook mStoryBook;
	void OnMouseDown(){
		    mStoryBook.QuitNarative();
	}
}
