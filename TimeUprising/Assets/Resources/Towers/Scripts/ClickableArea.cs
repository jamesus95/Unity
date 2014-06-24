using UnityEngine;
using System.Collections;

public class ClickableArea : MonoBehaviour
{
    public Tower ParentTower;
    private MouseManager mMouseManager;
    
    // Use this for initialization
    void Start ()
    {
        if (this.GetComponent<Renderer> () != null)
            this.GetComponent<Renderer> ().enabled = false;
            
        mMouseManager = GameObject.Find ("Towers").GetComponent<MouseManager> ();
        if (mMouseManager == null)
            Debug.LogError ("The Mouse Manager could not be found");
            
        this.transform.position = ParentTower.transform.position;
    }
    
    void OnMouseDown ()
    {
        if (ParentTower.Allegiance == Allegiance.Rodelle) {
            mMouseManager.SetAbilityTarget (ParentTower);
            mMouseManager.Select (ParentTower);
        }   
    }
}
