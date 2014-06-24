using UnityEngine;
using System.Collections;

public class Crossbow : Weapon
{
    private static GameObject mProjectilePrefab = null;
        
    public Crossbow (int level) : base (level)
    {       
        if (mProjectilePrefab == null)
            mProjectilePrefab = Resources.Load ("Weapons/ArrowPrefab") as GameObject;
          
        mWeaponType = WeaponType.Crossbow;
        this.InitializeStats();
        
        reloadTimer = ReloadTime;
    }
    
    protected override void PerformAttack (Target src, Target target)
    {
        if (target == null)
            return;
        base.PerformAttack (src, target);
        
        /*GameObject o = (GameObject)GameObject.Instantiate (mProjectilePrefab);
        Arrow a = (Arrow)o.GetComponent (typeof(Arrow));
        
        a.transform.position = src.transform.position;      
        a.SetDestination (target.transform.position);*/
    }
}
