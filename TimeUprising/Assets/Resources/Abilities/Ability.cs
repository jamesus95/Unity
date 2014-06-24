using UnityEngine;
using System.Collections;

public abstract class Ability : MonoBehaviour
{
    public float CoolDown {
        get { return mCooldown; }
        set { mCooldown = value;} }
    public float CooldownTimer { // >= mCooldown if the ability is ready to use
        get { return Time.time - mUseTimer; } }
    public float CountdownTimer { // <= 0 if the ability is ready to use
        get { return mCooldown - (Time.time - mUseTimer); } }
    
    protected float mCooldown;
    protected float mUseTimer;

    public abstract void UseAbility (Target target);
    public abstract void UsePositionalAbility (Vector3 location);
}
