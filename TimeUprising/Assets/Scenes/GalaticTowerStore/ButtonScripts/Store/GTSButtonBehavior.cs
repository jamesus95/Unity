using UnityEngine;
using System.Collections;

public class GTSButtonBehavior : ButtonBehaviour {


	protected enum UpgradeCost{
		CoolDown = 1500,
		SpawnRate = 1000,
		SpawnSize = 2000,
		TowerHealth = 1000,
	};
	public TowerStoreBehavior mStore;
	public GUIText mText;
    public GUIText mCurrentLevelText;
	public BonusType mBonusType;

	protected int mBonusLevel;
    protected int mCurrentLevel;
	protected int mCost;
	protected const int kBonusMax = 5;
	protected const int kBonusMin = 0;
	public GUIText mTotalGoldText;


	void Update(){
		mBonusLevel = mStore.GetUpgrade(mStore.mCurBonusSubject, mBonusType);	
		mText.text = mBonusLevel.ToString();
        mCurrentLevel = mStore.GetOringalValue(mStore.mCurBonusSubject,mBonusType);
        mCurrentLevelText.text = mCurrentLevel.ToString();
	}
	public void NewValue(int newLevel){
		mBonusLevel += newLevel;
		mStore.SetUpgrade(mStore.mCurBonusSubject, mBonusType, mBonusLevel);
	}
	public int GetOriginal(){
		return mStore.GetOringalValue(mStore.mCurBonusSubject, mBonusType);
	}
}
