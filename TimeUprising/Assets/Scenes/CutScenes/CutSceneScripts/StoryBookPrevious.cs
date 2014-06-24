using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class StoryBookPrevious : MonoBehaviour
{	
    public List<Camera> CameraViews;
    public List<SpriteRenderer> Image;
    public float SceneInterval = 3f;
	public string NextScene;
	public string FilePath;
    
    const float kFadeRate = 0.01f;
    
    private float mCameraInterval = 3f;
    private List<Camera> mCameraArray = new List<Camera> ();
    private float mCameraStartTime;
    private int mCurrentCamera = 1;
    private bool mIsFadingIn = false;
	private SpriteRenderer mCurrentImage;
	private ArrayList mDialogueArray = new ArrayList();
	StreamReader mFile;
	string line; //used to read line from mfile and arrays

    
    void Awake ()
    {
		if(File.Exists(FilePath))
			LoadDialogue();
		else
            Debug.LogError ("Could not find Dialogue text file");
			

        if (CameraViews == null || CameraViews.Count == 0)
            Debug.LogError ("Cameras need to be added to this script in the Unity inspector");
        
        mCameraInterval = SceneInterval;
        mCameraStartTime = Time.time;
        mCameraArray = CameraViews;
        mCameraArray [0].enabled = true;
        for (int i = 1; i < mCameraArray.Count; i++) {
            mCameraArray [i].enabled = false;
			setAlphaToZero(i);
		}


		mCurrentImage = Image[0];
		
        // Have the first scene fade in
        setImageColor(0f);
        mIsFadingIn = true;
    }
    
    // Update is called once per frame
    void Update ()
    {
        if (mIsFadingIn)
            FadeIn ();
        


        if (Time.time - mCameraStartTime > mCameraInterval && mCurrentCamera <= mCameraArray.Count) {
            if (FadeOut ()) {

				if (mCurrentCamera == mCameraArray.Count)
					StartGame ();

				mCameraArray [mCurrentCamera].enabled = true;
				mCameraArray[mCurrentCamera-1].enabled = false;
				mCurrentImage = Image[mCurrentCamera];
                mCurrentCamera ++;
                mCameraStartTime = Time.time;
                mIsFadingIn = true;
            }
        }
    }

    bool FadeIn ()
    {
        float alpha = setAlpha ();
        alpha += kFadeRate;
        setImageColor (alpha);
        if (alpha >= 1.0f) {
            mIsFadingIn = false;
            return true;
        } else
            return false;

    }

    bool FadeOut ()
    {
        float alpha = setAlpha ();
        alpha -= kFadeRate;
        setImageColor (alpha);
        if (alpha <= 0.0f)
            return true;
        else
            return false;
    }

    float setAlpha ()
    {
        return mCurrentImage.material.color.a;
    }

	void setAlphaToZero(int index){
		Image[index].material.color = new Color (Image[index].material.color.r, 
		                                         Image[index].material.color.g,
		                                         Image[index].material.color.b, 
	                                          	0);

	}
    void setImageColor (float alpha)
    {
		mCurrentImage.material.color = new Color (mCurrentImage.material.color.r, 
		                                          mCurrentImage.material.color.g,
		                                          mCurrentImage.material.color.b, 
		                                          alpha);
	}
	void StartGame(){
		Application.LoadLevel(NextScene);
	}
	public void SkipScene(){
		if (mCurrentCamera >= mCameraArray.Count)
			StartGame ();
		mCameraArray [mCurrentCamera].enabled = true;
		mCurrentImage = Image[mCurrentCamera];
		mCurrentCamera ++;
		mCameraStartTime = Time.time;
		mIsFadingIn = true;
	}
	public void PreviousScene(){
		if(mCurrentCamera <= 1)
			return;

		setAlphaToZero(mCurrentCamera-1);
		int prevCam = mCurrentCamera-1;
		if(mCurrentCamera <= 2){
			mCurrentCamera = 0;
		}
		else{
			mCurrentCamera -= 2;
		}
		mCameraArray [mCurrentCamera].enabled = true;
		mCameraArray[prevCam].enabled = false;
		setAlphaToZero(mCurrentCamera);
		mCurrentImage = Image[mCurrentCamera];
		mCurrentCamera ++;
		mCameraStartTime = Time.time;
		mIsFadingIn = true;
	}
	public int GetCurrentCamera(){
		return mCurrentCamera;
	}

	void LoadDialogue ()
	{
		mFile = new StreamReader(FilePath);	
		while(!mFile.EndOfStream){
			mDialogueArray.Add(mFile.ReadLine());
		}
	}
	public string GetDialogueText(int index){
		if(index > 0 && index < mDialogueArray.Count)
			return mDialogueArray[index].ToString();
		else 
			return null;
	}

}
