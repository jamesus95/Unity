using UnityEngine;
using System.Collections;
using System;

public class TowerStoreBehavior : MonoBehaviour {

	private Upgrades mCurUpgrades; 
	public int[,] DynamicUpgrades;
	public int[,] OriginalUpgrades;

	public BonusSubject mCurBonusSubject{get; set; }
	public int mCurCost{ get; set;}
	public GameObject TheStore;

	int mSubjectMax = Enum.GetNames(typeof(BonusSubject)).Length;
	int mBonusMax = Enum.GetNames(typeof(BonusType)).Length;

	// Use this for initialization
	void Start () {
        //gamestate testing

		mCurBonusSubject = BonusSubject.Melee;
		DynamicUpgrades = new int[mSubjectMax, mBonusMax];
		OriginalUpgrades = new int[mSubjectMax, mBonusMax];
		mCurUpgrades =  GameObject.Find("Store").GetComponent<Upgrades>();
		mCurUpgrades.SetStoreArray(ref DynamicUpgrades);
		mCurUpgrades.SetStoreArray(ref OriginalUpgrades);
	}
	
	public void Purchase(){
		GameState.Gold -= mCurCost;
		mCurUpgrades.SetUpgradeArray(DynamicUpgrades);
		mCurUpgrades.SetStoreArray(ref OriginalUpgrades);
		mCurUpgrades.WriteBonuses();
	}
	public int GetUpgrade(BonusSubject curSub, BonusType curType){
		return DynamicUpgrades[(int)curSub, (int)curType];
	}
	public void SetUpgrade(BonusSubject curSub, BonusType curType, int curValue ){
		DynamicUpgrades[(int)curSub, (int)curType] = curValue;
		//mCurUpgrades.SetBonus(mCurBonusSubject, mCurBonusType, mCurQuantity);
	}
	public void CloseStore(){
		TheStore.SetActive(false);
	}
	public int GetOringalValue(BonusSubject sub, BonusType typ){
		return OriginalUpgrades[(int)sub, (int)typ];
	}

    void Update ()
    {
        if(GameState.IsDebug && Input.GetKeyDown("g")){
            GameState.Gold = 100000;
            GameState.CurrentEra = Era.Future;
            //GameState.CurrentEra = Era.Prehistoric;
        }
    }
}
