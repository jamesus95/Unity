using UnityEngine;
using System.Collections;

public class Pitchfork : Weapon
{
    private static GameObject projectilePrefab = null;
    
    public Pitchfork (int level) : base (level)
    {        
        if (projectilePrefab == null)
            projectilePrefab = Resources.Load ("Weapons/ArrowPrefab") as GameObject;
            
        mWeaponType = WeaponType.Pitchfork;
        this.InitializeStats();
        
        reloadTimer = ReloadTime;
    }
    
    protected override void PerformAttack (Target src, Target target)
    {
        if (target == null)
            return;
        base.PerformAttack (src, target);
                
        /*GameObject o = (GameObject)GameObject.Instantiate (projectilePrefab);
        Arrow a = (Arrow)o.GetComponent (typeof(Arrow));
        
        a.transform.position = src.transform.position;      
        a.SetDestination (target.transform.position);*/
    }
}
