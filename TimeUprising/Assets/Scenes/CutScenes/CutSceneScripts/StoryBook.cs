using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public enum Action{
    FadeIn,
    FadeOut,
    Printing,
    ChangingSet,
    Finished,
    Waiting,
}; 
public class StoryBook : MonoBehaviour {



    public GameScenes mScenes;
    public List<SpriteRenderer> mImageArray;
	public string FilePath;
	public GUIText mDialogueText;

    Action mMyAction;
	int mCurrentImageIndex = 0;
	int mDialogueLetterIndex = 0;	
	private SpriteRenderer mCurrentImage;
	private ArrayList mDialogueArray = new ArrayList();
	StreamReader mFile;
	float alpha;
	float mPreviousLetterDisplayTime = 0f;
	string mCurrentDialogueString = null;
	bool mDialogueExists = true;
	bool mWaitedCalled = false;

    const float kFadeRate = 0.01f;
	const float kLetterDisplayDelay = 0.1f;	
	const float kSceneTransitionWaitTime = 2.5f;
    const int kMaxLineLength = 73;


	// Use this for initialization
	void Start () {
		if(File.Exists(FilePath))
			LoadDialogue();
		else{
			Debug.LogError ("Could not find Dialogue text file");
			mDialogueExists = false;
		}

		for (int i = 0; i < mImageArray.Count; i++) {
			setAlphaToZero(mImageArray[i]);
		}

		mMyAction = Action.FadeIn;

		mDialogueText.text = "";
		mCurrentDialogueString = mDialogueArray[mCurrentImageIndex].ToString();
		mCurrentImage = mImageArray[0];
	}
	
	// Update is called once per frame
	void Update () {
		switch(mMyAction){
		case Action.FadeIn:
			alpha = setAlpha ();
			alpha += kFadeRate;
			setImageColor (alpha);
			if (alpha >= 1.0f)
				mMyAction = Action.Printing;
			break;
		case Action.FadeOut:
			alpha = setAlpha ();
			alpha -= kFadeRate;
			setImageColor (alpha);
			if (alpha <= 0.0f)
				mMyAction = Action.ChangingSet;
			break;
		case Action.Printing:
			if(!mDialogueExists){
				mMyAction = Action.Waiting;
                break;
            }
			
            if(mDialogueLetterIndex < mCurrentDialogueString.Length){
				if(Time.time - mPreviousLetterDisplayTime > kLetterDisplayDelay){
					mDialogueText.text += mCurrentDialogueString[mDialogueLetterIndex].ToString();
					mDialogueLetterIndex++;
				}
			}
			else{
				mMyAction = Action.Waiting;
				mDialogueLetterIndex = 0;
			}
			break;
		case Action.ChangingSet:
			if(mCurrentImageIndex < mImageArray.Count && mCurrentImageIndex >= 0)
				setAlphaToZero(mImageArray[mCurrentImageIndex]);

			mCurrentImageIndex++;

			if(mCurrentImageIndex < mImageArray.Count){

				mCurrentDialogueString = mDialogueArray[mCurrentImageIndex].ToString();
				mCurrentImage = mImageArray[mCurrentImageIndex];
                mMyAction = Action.FadeIn;
			}
			else
				mMyAction = Action.Finished;
			break;
		case Action.Finished:
            if(mScenes == null)
                Application.LoadLevel("LevelLoader");
			Application.LoadLevel(mScenes.GetNextLevelAndCutScenes(Application.loadedLevelName));
			break;
		case Action.Waiting:
			if(!mWaitedCalled){
				Invoke("FrameWaited",kSceneTransitionWaitTime);
				mWaitedCalled = true;
			}
			break;
		}
	}
	void FrameWaited(){
		mDialogueText.text = "";
		mMyAction = Action.FadeOut;
		mWaitedCalled = false;	
	}
	void LoadDialogue ()
	{
		mFile = new StreamReader(FilePath);	
		while(!mFile.EndOfStream){
			string FullLine = "";
			string line = mFile.ReadLine();
			string[] Speakers = line.Split(char.Parse(">"));
            for(int i = 0; i < Speakers.Length; i++){
                Speakers[i] = InsertNewLine(Speakers[i]);
                FullLine += Speakers[i] + "\n";
            }

			mDialogueArray.Add(FullLine);
		}
	}
	void setAlphaToZero(SpriteRenderer CurImage){
		CurImage.material.color = new Color (CurImage.material.color.r, 
		                                               CurImage.material.color.g,
		                                               CurImage.material.color.b, 
		                                         		0);
		
	}
	void setImageColor (float alpha)
	{
        if(mCurrentImage == null)
            return;
		mCurrentImage.material.color = new Color (mCurrentImage.material.color.r, 
		                                          mCurrentImage.material.color.g,
		                                          mCurrentImage.material.color.b, 
		                                          alpha);
	}
	 float setAlpha ()
	{
		if(mCurrentImage != null)
			return mCurrentImage.material.color.a;
		else 
			return 1f;
	}
	public void NextFrame(){
		if (mCurrentImageIndex + 1 >= mImageArray.Count){
			return;
		}
		else{
			mDialogueText.text = "";
			mDialogueLetterIndex = 0;
			mMyAction = Action.ChangingSet;
		}
	}
	public void PreviousFrame(){
		if(mCurrentImageIndex -1 < 0)
			return;

		setAlphaToZero(mImageArray[mCurrentImageIndex]);
		mDialogueText.text = "";
		mDialogueLetterIndex = 0;		
		mCurrentImageIndex -= 2;

		mMyAction = Action.ChangingSet;
	}
	public int GetCurrentImage(){
		if(mCurrentImageIndex + 1 <= mImageArray.Count)
			return mCurrentImageIndex + 1;
		else
			return mImageArray.Count;
	}
	public int GetTotalImages(){
		return mImageArray.Count;
	}
	public void QuitNarative(){
		mMyAction = Action.Finished;	
	}
    string InsertNewLine(string lineSegment){
        if(lineSegment.Length < kMaxLineLength)
            return lineSegment;

        int numOfNewLinesNeeded = lineSegment.Length / kMaxLineLength;
        int curNewLineLocation = kMaxLineLength;
        for(int j = 0; j < numOfNewLinesNeeded; j++)
            for(int i = curNewLineLocation; i > 0; i--){
                if(lineSegment[i] == ' '){
                    lineSegment = lineSegment.Insert(i, "\n");
                    curNewLineLocation += kMaxLineLength;
                    break;
                }   
            }
        return lineSegment;
    }
    public Action GetMyAction(){
        return mMyAction;
    }
}
