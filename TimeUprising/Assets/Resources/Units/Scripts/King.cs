using UnityEngine;
using System.Collections;

public class King : Unit
{
    public Progressbar HealthBar;

    protected override void Awake ()
    {
        mUnitType = UnitType.King;
        base.Awake();
        this.InitializeStats();
        
        mMeleeWeapon = new Sword (mLevel);
        mCurrentWeapon = mMeleeWeapon;
        
        HealthBar.MaxValue = mMaxHealth;
        HealthBar.UpdateValue(mMaxHealth);
        GameState.KingsHealth = mMaxHealth;
    }
    
    public override void Damage (int damage, Weapon weapon = null)
    {            
        base.Damage (damage);
        GameState.KingsHealth = mHealth;
        HealthBar.UpdateValue(mHealth);
        if (GameState.KingsHealth > MaxHealth)
        {
            GameState.KingsHealth = MaxHealth;
        }
    }
    
    public override Vector3 Position {
        get {
            return base.Position + new Vector3(0f, -5f, 0f); // hit box is slightly lower than center
        }
        set {} // doesn't move
    }

    void OnMouseDown()
    {
        GameObject.Find("Towers").GetComponent<MouseManager>().SetAbilityTarget(this);
    }

}
