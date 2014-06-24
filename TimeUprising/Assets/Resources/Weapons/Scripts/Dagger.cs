using UnityEngine;
using System.Collections;

public class Dagger : Weapon
{       
    public Dagger (int level) : base (level)
    {      
        mWeaponType = WeaponType.Dagger;
        this.InitializeStats();
        
        reloadTimer = ReloadTime;
    }
}
