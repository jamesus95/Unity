using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Swordsman : Unit
{
    protected override void Awake ()
    {
        mUnitType = UnitType.Swordsman;
        base.Awake();
        this.InitializeStats();
                              
        mMeleeWeapon = new Sword (mLevel);
        mCurrentWeapon = mMeleeWeapon;
    }
}
