using UnityEngine;
using System.Collections;

public class SpawnSizeDownButton : GTSButtonBehavior {


	void OnMouseDown(){
		if( mBonusLevel > kBonusMin && mBonusLevel > GetOriginal())  {
			GameObject.Find("ShadySeamus").GetComponent<ShadySeamusDialogue>().WriteNegDialogue();
			mStore.mCurCost -= (int)UpgradeCost.SpawnSize * mBonusLevel;
			mTotalGoldText.text = mStore.mCurCost.ToString();
			NewValue(-1);
		}
	}

}
