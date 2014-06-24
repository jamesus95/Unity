using UnityEngine;
using System.Collections;

public class Elite : Unit
{
    protected override void Awake ()
    {
        mUnitType = UnitType.Elite;
        base.Awake();
        this.InitializeStats();
        
        mMeleeWeapon = new RaptorClaws (mLevel);
        mCurrentWeapon = mMeleeWeapon;
    }
}
