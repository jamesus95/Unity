using UnityEngine;
using System.Collections;

public enum Allegiance
{
    Rodelle, 
    AI,
}

public abstract class Target : MonoBehaviour, Selectable
{
    public Allegiance Allegiance {
        get { return mAllegiance; }
        set { mAllegiance = value; }
    }
    
    public virtual Vector3 Position {
        get { return this.transform.position; }
        set { this.transform.position = value; }
    }
    
    public virtual int MaxHealth {
        get { return mMaxHealth; }
        set { mMaxHealth = value; }
    }
    
    public virtual int Health {
        get { return mHealth; }
    }
    
    public bool IsDead {
        get { return mHealth <= 0; }
    }

    public abstract void Damage (int damage, Weapon weapon = null);
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Interface: Selectable
    ///////////////////////////////////////////////////////////////////////////////////
    public Renderer Selector;
    
    public abstract void SetDestination (Vector3 destination);
    
    public abstract void UseTargetedAbility (Target target);
    
    public virtual void Select ()
    {
        ShowSelector (true);
    }
    
    public virtual void Deselect ()
    {
        ShowSelector (false);
    }
    
    protected virtual void ShowSelector (bool doSelect)
    {
        if (Selector != null)
            Selector.enabled = doSelect;
    }
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Protected/Private
    ///////////////////////////////////////////////////////////////////////////////////
    
    protected int mMaxHealth;
    protected int mHealth;
    protected Allegiance mAllegiance;
}
