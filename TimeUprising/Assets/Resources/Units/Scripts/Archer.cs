using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Archer : Unit
{
    protected override void Awake ()
    {
        mUnitType = UnitType.Archer;
        base.Awake();
        this.InitializeStats();
        
        mMeleeWeapon = new Dagger (mLevel);
        mRangedWeapon = new Crossbow (mLevel);
        mCurrentWeapon = mRangedWeapon;
    }
}
