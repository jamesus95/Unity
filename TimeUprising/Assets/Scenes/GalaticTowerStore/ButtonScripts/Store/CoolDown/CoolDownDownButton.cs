using UnityEngine;
using System.Collections;

public class CoolDownDownButton : GTSButtonBehavior {



	void OnMouseDown(){
		if( mBonusLevel > kBonusMin && mBonusLevel > GetOriginal()){
			GameObject.Find("ShadySeamus").GetComponent<ShadySeamusDialogue>().WriteNegDialogue();
			mStore.mCurCost -= (int)UpgradeCost.CoolDown * mBonusLevel;
			mTotalGoldText.text = mStore.mCurCost.ToString();
			NewValue(-1);
		}
	}
}
