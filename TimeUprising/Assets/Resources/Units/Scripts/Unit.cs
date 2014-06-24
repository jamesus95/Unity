using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UnitType
{
    None,
    Peasant,
    Swordsman,
    Archer,
    Mage,
    King,
    Elite,
}

public class Unit : Target
{    
    public Squad Squad { get; set; }

    public float Range { get { return mCurrentWeapon.Range; } }

    public bool IsIdle { get { return this.mMovementState == MovementState.Idle; } }
    
    public float SightRange = 39f;
    
    // TODO replace with unit stats dictionary
    public float MoveSpeed { // units per second
        get { return mMovementSpeed; } 
        set { mMovementSpeed = value; }
    }
    
    public float ChargeSpeed { // units per second
        get { return mChargeSpeed; } 
        set { mChargeSpeed = value; }
    } 
    
    public UnitType UnitType { 
        get { return mUnitType; }
    }
    
    // do nothing
    public override void SetDestination (Vector3 destination) {}
    
    // do nothing
    public override void UseTargetedAbility (Target target) {}
    
///////////////////////////////////////////////////////////////////////////////////
// Public Methods
///////////////////////////////////////////////////////////////////////////////////
    
    public void BuffMovement (float percent, float duration)
    {
        mMovementSpeed *= percent;
        mChargeSpeed *= percent;
        Invoke("RestoreStat", duration);
    }
    
    public void RestoreStat()
    {
        mMovementSpeed = UnitStats.GetStat(this.mUnitType, UnitStat.MovementSpeed);
        mChargeSpeed = UnitStats.GetStat(this.mUnitType, UnitStat.ChargeSpeed);
    }
    
    /// <summary>
    /// Updates the destination of the unit.
    /// If the unit is not engaged in combat, it will move towards this destination.
    /// </summary>
    /// <param name="target">Destination. The location to move towards. </param>
    public void MoveTo (Vector3 destination)
    {
        this.mDestination = destination;
        
        if (mAttackTarget == null) // can only move if there is no one in range
            mMovementState = MovementState.Moving;
    }
        
    /// <summary>
    /// Engage the specified target from the given firingPosition.
    /// Units will attempt to move towards the firingPosition before attacking enemies.
    /// </summary>
    /// <param name="target">Target. The target to attack.</param>
    /// <param name="firingPosition">Firing position. The position to attack from.</param>
    public void Engage (Target target, Vector3? firingPosition = null)
    {
        mAttackVector = (firingPosition != null) 
            ? firingPosition.Value
            : this.Position;
        
        mMovementState = MovementState.Idle;
        mAttackTarget = target;
        
        mAttackState = AttackState.Engaging;
    }
    
    /// <summary>
    /// Reduces the health of the unit by the given amount.
    /// </summary>
    /// <param name="damage">Damage. The amount of damage taken.</param>
    public override void Damage (int damage, Weapon weapon = null)
    {
        mHealth -= damage;
        if (mHealth <= 0) {
            if (weapon != null && weapon.WeaponType == WeaponType.AoePortal) {
                // do nothing
            } else {
                GameObject o = GameObject.Instantiate(mWarpPrefab) as GameObject;
                o.transform.position = this.Position;
            }
            
            if (this.Allegiance == Allegiance.AI)
                GameState.Gold += mGoldReward;
            
            Despawn ();
        }
    }
    
    public void Despawn ()
    {
        this.Squad.Notify (SquadAction.UnitDied, this);
        
        if (this.GetComponent<IceBlock>() != null)
            this.GetComponent<IceBlock>().Unfreeze();
        
        Destroy (this.gameObject);
    }
    
    /// <summary>
    /// Switchs to weapon stored at the given index. If the index refers to an invalid weapon, no action is taken.
    /// </summary>
    /// <param name="index">Index.</param>
    public void SwitchToMeleeWeapon ()
    {
        if (mRangedWeapon != null)
            mRangedWeapon.Reset();
            
        mCurrentWeapon = mMeleeWeapon;
        mCurrentWeapon.Reset();
        mAttackState = AttackState.Melee;
    }
    
    public void SwitchToRangedWeapon ()
    {
        mMeleeWeapon.Reset();
        mCurrentWeapon = mRangedWeapon != null ? mRangedWeapon : mMeleeWeapon;
        mCurrentWeapon.Reset();
    }
    
    /// <summary>
    /// Disenganges the unit from combat and starts them moving towards their original destination.
    /// The unit switches to their longest ranged weapon.
    /// </summary>
    public void Disengage ()
    {
        mAttackState = AttackState.Idle;
        mMovementState = MovementState.Moving;
        mAttackTarget = null;
        
        SwitchToRangedWeapon (); // longest range weapon
    } 
    
///////////////////////////////////////////////////////////////////////////////////
// Private Methods
///////////////////////////////////////////////////////////////////////////////////
    
    protected enum MovementState
    {
        Moving,
        Idle
    }
    
    protected enum AttackState
    {
        Idle,
        Engaging,
        Ranged,
        Melee
    }
    
    protected UnitAnimation mUnitAnimator;
    protected float mMovementSpeed; // units per second
    protected float mChargeSpeed;   // speed used to engage enemies
    protected int mLevel;
    protected int mGoldReward;
    
    protected Weapon mCurrentWeapon = null;
    protected Weapon mMeleeWeapon;
    protected Weapon mRangedWeapon = null;
    
    protected Target mAttackTarget;
    protected Vector3 mAttackVector; // direction to attack target from
    protected Vector3 mDestination;  // location to move to when no other actions are taking place
    protected Vector3 mPreviousLocation;
    
    protected MovementState mMovementState;
    protected AttackState mAttackState;
    
    protected UnitType mUnitType;
    
    protected static GameObject mWarpPrefab;
    
    private void UpdateTargetState ()
    {
        if (mAttackState == AttackState.Idle) // no target present
            return; 
            
        if (mAttackTarget == null) { // target's been destroyed
            mCurrentWeapon.Reset ();
            this.Squad.Notify (SquadAction.UnitDestroyed, this);
            return;
        }
        
        if (mAttackTarget.Allegiance == mAllegiance) {
            this.Squad.Notify (SquadAction.TargetDestroyed);
            return;
        }
    }
    
    private void EngageTarget (Target target)
    {
        if (target == null)
            return;
        
        Vector3 targetLocation = target.Position;   
        Vector3 firingPosition = targetLocation + mAttackVector;
        
        float targetDistance = Vector3.SqrMagnitude (targetLocation - this.Position);
        float firingPositionDistance = Vector3.SqrMagnitude (targetLocation - firingPosition);
        
        // if in range, start firing!
        // do not move away from target
        if (Vector3.Distance (this.Position, targetLocation) <= mCurrentWeapon.Range ||
            firingPositionDistance > targetDistance) {
            mAttackState = AttackState.Ranged;
            return;
        }
        
        // Second priority is to move into the firing position
        if (Vector3.Distance (this.Position, firingPosition) > 1.0f) {
            UpdateMovement (firingPosition, mChargeSpeed);
        } else { // firing position reached, attack!
            mAttackState = AttackState.Ranged;
        }
    }
    
    private void UpdateAttack (Target target)
    {
        if (target == null)
            return;
        
        Vector3 targetLocation = target.Position;
        float targetDistanceSquared = Vector3.SqrMagnitude (targetLocation - this.Position);
                
        // switch to melee range if target is close enough
        if (mAttackState != AttackState.Melee && targetDistanceSquared < 15f * 15f) 
            Squad.Notify(SquadAction.EngagedInMelee);
            
        // Move into range of the target
        if (targetDistanceSquared > mCurrentWeapon.Range * mCurrentWeapon.Range) {
            UpdateMovement (targetLocation, mChargeSpeed);
            return;
        }
        
        bool attackSuccess = mCurrentWeapon.Attack (this, target);
        if (attackSuccess)
        {
            AudioSource sfx = gameObject.GetComponent<AudioSource>();
            if (sfx == null) { return; }
            sfx.volume = 0f;
            sfx.volume = GameObject.Find("GameManager").GetComponent<ControlPanel>().mSFXVolume;
            sfx.Play();
        }
    }
    
    private void UpdateMovement (Vector3 targetLocation, float speed)
    {
        if (Vector3.SqrMagnitude (this.Position - targetLocation) < 1.0f) {
            mMovementState = MovementState.Idle;
            this.Squad.Notify (SquadAction.DestinationReached);
            return;
        }
        
        Vector3 targetDir = targetLocation - Position;
        targetDir.Normalize ();
     
        mPreviousLocation = Position;      
        Position += speed * Time.deltaTime * targetDir;
    }
    
    // Update sprite set to match current health
    private void UpdateAnimation ()
    {
        if (mAttackState == AttackState.Idle && mMovementState == MovementState.Idle) {
            mUnitAnimator.Idle ();
            return;
        }
                
        Vector3 direction;
        if (mAttackTarget != null)
            direction = mAttackTarget.Position - this.Position;
        else 
            direction = mDestination - this.Position;

        if (mAttackState == AttackState.Idle || mAttackState == AttackState.Engaging) {
            if (direction.x >= 0) 
                mUnitAnimator.WalkRight ();
            else 
                mUnitAnimator.WalkLeft ();
        } else {
            if (direction.x >= 0)
                mUnitAnimator.AttackRight ();
            else 
                mUnitAnimator.AttackLeft ();
        }
            
        int sortingOrder = (int)(4 * (-Position.y + Camera.main.orthographicSize));
        GetComponent<SpriteRenderer> ().sortingOrder = (int)(sortingOrder);
    }
    
    // pre: this.UnitType is set to the unit's type
    // Pulls the unit stat information from the Global UnitUpgrades class
    protected virtual void InitializeStats()
    {
        mMaxHealth = (int)UnitStats.GetStat(mUnitType, UnitStat.Health);
        mHealth = mMaxHealth;
        mMovementSpeed = (int)UnitStats.GetStat(mUnitType, UnitStat.MovementSpeed);
        mChargeSpeed = (int)UnitStats.GetStat(mUnitType, UnitStat.ChargeSpeed);
        mLevel = (int)UnitStats.GetStat(mUnitType, UnitStat.Level);
        mGoldReward = (int)UnitStats.GetStat(mUnitType, UnitStat.GoldReward);
        this.SightRange = UnitStats.GetStat(mUnitType, UnitStat.SightRange);
    }
    
///////////////////////////////////////////////////////////////////////////////////
// Unity Overrides
///////////////////////////////////////////////////////////////////////////////////
    
    void OnMouseDown()
    {
        if (this.Allegiance == Allegiance.Rodelle) {
            // uncomment this line if you want abilities to be able to target squads
            // GameObject.Find ("Towers").GetComponent<MouseManager> ().SetAbilityTarget (this.Squad);
            GameObject.Find ("Towers").GetComponent<MouseManager> ().Select (this.Squad);
        }
    }
    
    // Update is called once per frame
    void FixedUpdate ()
    {
        UpdateTargetState ();
        UpdateAnimation();
        
        switch (mAttackState) {
        case (AttackState.Idle):
            break;
            
        case (AttackState.Engaging):
            EngageTarget (mAttackTarget);
            break;
            
        case (AttackState.Ranged):
        case (AttackState.Melee):
            UpdateAttack (mAttackTarget);
            break;
        }
        
        switch (mMovementState) {
        case (MovementState.Idle):
            break;
            
        case (MovementState.Moving):
            UpdateMovement (mDestination, mMovementSpeed);
            break;
        }
    }

    // Initialize variables
    protected virtual void Awake ()
    {        
        mAttackState = AttackState.Idle;
        mAttackTarget = null;
        
        mMovementState = MovementState.Idle;
        mDestination = new Vector3 (0, 0, 0);
        
        mUnitAnimator = this.GetComponent<UnitAnimation> ();
        if (mUnitAnimator == null)
            Debug.LogError ("UnitAnimation script was not attached to this Unit!");
            
        if (Selector != null)
            Selector.enabled = false;
            
        if (mWarpPrefab == null)
            mWarpPrefab = Resources.Load("Units/Prefabs/UnitWarpPrefab") as GameObject;
    }
}