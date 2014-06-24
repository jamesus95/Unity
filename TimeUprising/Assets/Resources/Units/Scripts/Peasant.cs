using UnityEngine;
using System.Collections;

public class Peasant : Unit
{
    protected override void Awake ()
    {
        mUnitType = UnitType.Peasant;
        base.Awake();
        this.InitializeStats();
        
        mMeleeWeapon = new Pitchfork (mLevel);
        mCurrentWeapon = mMeleeWeapon;
    }
}
