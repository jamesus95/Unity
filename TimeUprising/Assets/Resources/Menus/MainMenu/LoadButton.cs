using UnityEngine;
using System.Collections;

public class LoadButton : MonoBehaviour {

    public Sprite mButton;
    public Sprite mButtonOver;
	public GameObject mLoadFrameObject;
	public GameObject mMenuFrameObject;
    private bool isActive = true;
    public Sprite noLoad;

	// Use this for initialization
	void Start () {
	}

	void OnMouseDown(){
        //SaveLoad s = GameObject.Find("SaveLoad").GetComponent<SaveLoad>();
		
        //s.Clear(SaveLoad.SAVEFILE.Level);
        //s.Load(SaveLoad.SAVEFILE.Level);
        //ChangeScreen();
		//mLoadMenuFrame.SetActive(false);
		//mNarativeContinueMenuFrame.SetActive(true);
        if (isActive)
        {
            Application.LoadLevel("LevelLoader"); 
        }
        
		//mLoadFrameObject.SetActive(true);
		//mMenuFrameObject.SetActive(false);
		//ChangeScreen();
	}

    public void setInactive()
    {
        isActive = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = noLoad;
    }

    void OnMouseOver()
    {
        if (isActive)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = mButtonOver;
        }
        
    }
    void OnMouseExit()
    {
        if (isActive)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = mButton;
        }
    }

    public void ChangeScreen()
    {
        if (isActive)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = mButton;
        }
    }

}
