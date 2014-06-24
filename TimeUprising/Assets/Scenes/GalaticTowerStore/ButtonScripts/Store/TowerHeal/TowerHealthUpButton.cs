using UnityEngine;
using System.Collections;

public class TowerHealthUpButton : GTSButtonBehavior {


	void OnMouseDown(){
		if( mBonusLevel < kBonusMax ){
			NewValue(1);
			GameObject.Find("ShadySeamus").GetComponent<ShadySeamusDialogue>().WritePosDialogue();
			mStore.mCurCost += (int)UpgradeCost.TowerHealth * mBonusLevel;
			mTotalGoldText.text = mStore.mCurCost.ToString();
		}
	}
}
