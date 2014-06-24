using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Tower : Target
{
    ///////////////////////////////////////////////////////////////////////////////////
    // Inspector Presets
    ///////////////////////////////////////////////////////////////////////////////////
    
    public Progressbar TowermHealthBar;
    public List<Sprite> DamagedSprites;
    public Sprite CapturedSprite;
    public bool canTargetTowers;
    public GameObject SpawnPoint;
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Public Methods and Variables
    ///////////////////////////////////////////////////////////////////////////////////
    
    public override Vector3 Position {
        get {
            return SpawnPoint.transform.position;
        }
        set {
            base.Position = value;
        }
    }
    
    /// <summary>
    /// Action performed when the user left-clicks on a location after
    /// selecting the tower
    /// </summary>
    /// <param name="location">Location. The game coodinate clicked on.</param>
    public abstract override void SetDestination(Vector3 destination);
    public abstract override void UseTargetedAbility(Target Target);

    public override void Damage (int damage, Weapon weapon = null)
    {
        if (damage > 1) // makes towers stronger
            damage = 1;
            
        mHealth -= damage;
        
        if (mHealth <= 0) {
            mHealth = (int)(mMaxHealth * 0.25);
            mAllegiance = this.Allegiance == Allegiance.Rodelle
                ? Allegiance.AI
                : Allegiance.Rodelle;
                
            if (this.Allegiance != Allegiance.Rodelle)
                this.ShowSelector (false);
        }
        
        if (mHealth > mMaxHealth)
            mHealth = mMaxHealth;
        TowermHealthBar.Value = mHealth;
        UpdateAnimation ();
    }
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Private Methods and Variables
    ///////////////////////////////////////////////////////////////////////////////////
    
    protected const int kDefaultHealth = 45;
    protected BonusSubject mTowerType;
    
    private void UpdateAnimation ()
    {
        SpriteRenderer sr = this.GetComponent<SpriteRenderer> ();
        if (sr == null || DamagedSprites.Count == 0)
            return;
        
        if (this.Allegiance == Allegiance.AI)
            sr.sprite = CapturedSprite;
        else { 
            float percentDamaged = (float)(mMaxHealth - mHealth) / (float)mMaxHealth;
            int spriteIndex = (int)(percentDamaged * DamagedSprites.Count);
            if (spriteIndex < DamagedSprites.Count)
                sr.sprite = DamagedSprites [spriteIndex];
        }
    }
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Unity Overrides
    /////////////////////////////////////////////////////////////////////////////////// 
    
    protected virtual void Start ()
    {
        mAllegiance = Allegiance.Rodelle;
        
        Upgrades upgrades = GameObject.Find ("UI").GetComponent<Upgrades> ();
        float healthBonus = upgrades.GetBonus(this.mTowerType, BonusType.Health);
        
        mHealth = (int)(kDefaultHealth * healthBonus);
        mMaxHealth = mHealth;
        
        TowermHealthBar.MaxValue = mMaxHealth;
        TowermHealthBar.Value = mHealth;
        this.Deselect();
        UpdateAnimation ();
    }
}
