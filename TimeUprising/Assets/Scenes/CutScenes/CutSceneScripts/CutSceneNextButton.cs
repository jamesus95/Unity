using UnityEngine;
using System.Collections;

public class CutSceneNextButton : ButtonBehaviour {

	public StoryBook mStoryBook;
	void OnMouseDown(){
        if(mStoryBook.GetMyAction() != Action.ChangingSet || 
           mStoryBook.GetMyAction() != Action.Waiting)
		        mStoryBook.NextFrame();
	}
}
