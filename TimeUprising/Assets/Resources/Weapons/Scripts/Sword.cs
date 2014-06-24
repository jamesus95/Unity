using UnityEngine;
using System.Collections;

public class Sword : Weapon
{    
    public Sword (int level) : base (level)
    {                
        mWeaponType = WeaponType.Sword;
        this.InitializeStats();
        
        reloadTimer = ReloadTime;
    }
    
    protected override void PerformAttack (Target src, Target target)
    {
        if (target == null)
            return;
        base.PerformAttack (src, target);
    }
}
