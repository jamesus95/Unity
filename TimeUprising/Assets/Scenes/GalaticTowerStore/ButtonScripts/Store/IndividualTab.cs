using UnityEngine;
using System.Collections;

public class IndividualTab : MonoBehaviour {

	public SelectBar mSelectBar;
	public GameObject TabButtons;
	public BonusSubject mBonusSubject;


	void OnMouseDown(){ 
    		mSelectBar.SetTab(this.gameObject, TabButtons, mBonusSubject);
	}
}
