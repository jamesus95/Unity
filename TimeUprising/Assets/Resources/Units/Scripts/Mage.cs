using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mage : Unit
{   
    protected override void Awake ()
    {
        mUnitType = UnitType.Mage;
        base.Awake();
        this.InitializeStats();
        
        mMeleeWeapon = new Dagger (mLevel);
        mRangedWeapon = new IceWand (mLevel);
        mCurrentWeapon = mRangedWeapon;
        
        ((IceWand)mRangedWeapon).src = this;
    }
}
