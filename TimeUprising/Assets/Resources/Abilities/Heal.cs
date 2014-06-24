using UnityEngine;
using System.Collections;

public class Heal : Ability
{
    protected float mHealRate = 0.5f;

    void Awake()
    {
        mCooldown = 10f;
        mUseTimer = -mCooldown;
    }

    public override void UseAbility (Target target)
    {
        if (target.Health == target.MaxHealth)
            return; // do not heal full health targets
            
        if (Time.time - mUseTimer > mCooldown) {
            target.Damage((int)(-mHealRate * target.MaxHealth));
            mUseTimer = Time.time;
        }
    }
    
    // does nothing
    public override void UsePositionalAbility (Vector3 location) {}
}
