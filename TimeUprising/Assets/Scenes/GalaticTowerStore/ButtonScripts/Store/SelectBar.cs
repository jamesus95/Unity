using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectBar : MonoBehaviour {

	public List<GameObject> mAllTabs;
	public List<GameObject> mAllTabButtons;
	GameObject mCurrentTab;
	GameObject mCurrentTabButtons;
	public TowerStoreBehavior mStore;

	void Start(){

        SetLockedTabs();


        for(int i = 0; i < mAllTabs.Count; i++){
			GameObject tempTab = (GameObject)mAllTabs[i];
			GameObject tempTabButtons = (GameObject)mAllTabButtons[i];
            if(tempTab.name.Equals("TabsMelee")){
				mCurrentTab = tempTab;
				mCurrentTab.renderer.sortingOrder = 10;
                mStore.mCurBonusSubject = BonusSubject.Melee;
			}
			else
				tempTab.renderer.sortingOrder = 1;
            if(tempTabButtons.name.Equals("MeleeTabButtons")){
				mCurrentTabButtons = tempTabButtons;
				mCurrentTabButtons.SetActive(true);
			}
			else
				tempTabButtons.SetActive(false);
		}

	}


	public void SetTab(GameObject CurTab, GameObject TabButtons, BonusSubject curSub){

		mCurrentTab.renderer.sortingOrder = 1;
		mCurrentTabButtons.SetActive(false);

		mCurrentTab	= CurTab;
		mCurrentTabButtons = TabButtons;

		mCurrentTab.renderer.sortingOrder = 10;
		mCurrentTabButtons.SetActive(true);
	
		mStore.mCurBonusSubject = curSub;
	}

    void SetLockedTabs(){
        if((int)GameState.CurrentEra >= (int)Era.Medieval){
            GameObject.Find("TabsSpecialLocked").SetActive(false);
            GameObject.Find("TabsSpecial").SetActive(true);
        }
        else{
            GameObject.Find("TabsSpecialLocked").SetActive(true);
            GameObject.Find("TabsSpecial").SetActive(false);
        }
        if((int)GameState.CurrentEra >= (int)Era.Japanese){
            GameObject.Find("TabsAOELocked").SetActive(false);
            GameObject.Find("TabsAOE").SetActive(true);
        }
        else{
            GameObject.Find("TabsAOELocked").SetActive(true);
            GameObject.Find("TabsAOE").SetActive(false);
        }
    }

}
