using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseManager : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////////////////////////
    // Public Methods and Variables
    ///////////////////////////////////////////////////////////////////////////////////
        
    public void SetAbilityTarget (Target target)
    {
        UseTargetedAbility (target);
    }
    
    /*public void SelectMultiple (List<Selectable> selectable)
    {
        foreach (Selectable s in mSelected)
            s.Deselect();
            
        mSelected = selectable;
        UseTargetedAbility (mTarget);
        
        mWasJustSelected = true;
    }*/
    
    public void Select (Selectable selectable)
    {          
        mWasJustSelected = true;
        
        if (mSelected.Contains(selectable)) {
            this.Deselect(selectable);
            return;
        }
    
        foreach (Selectable s in mSelected) {
            if (s != null)
                s.Deselect();
        }
        
        mSelected.Clear ();
        
        mSelected.Add (selectable);
        mSelected[0].Select ();
    }
    
    public void Deselect (Selectable selectable)
    {
        if (selectable == null)
            return;
            
        if (mSelected.Contains (selectable)) 
            mSelected.Remove (selectable);
        
        selectable.Deselect ();
    }
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Private Methods and Variables
    ///////////////////////////////////////////////////////////////////////////////////
    private List<Selectable> mSelected;
    private bool mWasJustSelected = false;
    
    private void UseTargetedAbility (Target target)
    {
        if (target == null)
            return;
        
        for (int i = 0; i < mSelected.Count; ++i){
            if (mSelected[i] == null)
                continue;
            
            mSelected[i].UseTargetedAbility (target);
        }
    }
    
    private void SetDestination (Vector3 destination)
    {
        for (int i = 0; i < mSelected.Count; ++i){
            if (mSelected[i] == null)
                continue;
            
            mSelected[i].SetDestination (destination);
        }
    }
    
    private void CheckTowerHotkey(string hotkey, string towerName) 
    {
        if (Input.GetButtonDown (hotkey)) {
            GameObject tower = GameObject.Find(towerName);
            if (tower == null)
                return;
        
            Select (tower.GetComponent<Tower> ());
        }
    }
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Unity Overrides
    /////////////////////////////////////////////////////////////////////////////////// 
    
    void Awake()
    {
        mSelected = new List<Selectable>();
    }
    
    void LateUpdate ()
    {
        if (Input.GetMouseButtonDown (0) && ! mWasJustSelected) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            mousePos.z = 0f;
            SetDestination (mousePos);
        }
        
        if (GameState.IsDebug && Input.GetButtonDown ("Fire2") && mSelected[0] is UnitSpawningTower) {
            ((UnitSpawningTower)mSelected[0]).SpawnUnit ();
        }
        
        CheckTowerHotkey("SelectRanged", "ArcherTower");
        CheckTowerHotkey("SelectMelee", "SwordsmanTower");
        CheckTowerHotkey("SelectSpecial", "MageTower");
        CheckTowerHotkey("SelectHeal", "HealTower");
        CheckTowerHotkey("SelectAoe", "AoeTower");
        CheckTowerHotkey("SelectBoost", "BoostTower");
            
        mWasJustSelected = false;
    }
}
