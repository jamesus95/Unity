using UnityEngine;
using System.Collections;

public class RaptorClaws : Weapon
{
    public RaptorClaws (int level) : base (level)
    {                
        mWeaponType = WeaponType.RaptorClaws;
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