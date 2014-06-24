using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

enum UnitAnimationState
{
    AttackLeft,
    AttackRight,
    WalkLeft,
    WalkRight,
    IdleLeft,
    IdleRight,
}

public class UnitAnimation : MonoBehaviour
{   
///////////////////////////////////////////////////////////////////////////////////
// Public
/////////////////////////////////////////////////////////////////////////////////// 
    public void WalkLeft ()
    {
        SetProperty (UnitAnimationState.WalkLeft);
    }
    
    public void WalkRight ()
    {
        SetProperty (UnitAnimationState.WalkRight);
    }
    
    public void AttackRight ()
    {
        SetProperty (UnitAnimationState.AttackRight);
    }
    
    public void AttackLeft ()
    {
        SetProperty (UnitAnimationState.AttackLeft);
    }
    
    public void Idle ()
    {           
        if (IsFacingLeft ())
            SetProperty (UnitAnimationState.IdleLeft);
        else 
            SetProperty (UnitAnimationState.IdleRight);
    }
    
///////////////////////////////////////////////////////////////////////////////////
// Private
/////////////////////////////////////////////////////////////////////////////////// 
    private Animator mAnimator;
    Dictionary<UnitAnimationState, bool> mAnimationStates;
    
    void Awake ()
    {
        mAnimator = this.GetComponent<Animator> ();
        
        mAnimationStates = new Dictionary<UnitAnimationState, bool> ();
        foreach (UnitAnimationState state in Enum.GetValues(typeof(UnitAnimationState)))
            mAnimationStates.Add (state, false);
    }
    
    // Sets the property to true and other properties to false
    private void SetProperty (UnitAnimationState state)
    {
        if (mAnimator == null)
            return;
        
        // state is already set, no need to continue
        if (mAnimationStates[state])
            return;
        
        string property = state.ToString ();
        
        foreach (UnitAnimationState s in mAnimationStates.Keys.ToList()) {
            mAnimator.SetBool (s.ToString (), false);
            mAnimationStates [s] = false;
        }
        
        mAnimator.SetBool (property, true);
        mAnimationStates [state] = true;
    }
    
    private bool IsFacingLeft ()
    {
        return
        mAnimationStates [UnitAnimationState.WalkLeft] 
            || mAnimationStates [UnitAnimationState.IdleLeft]
            || mAnimationStates [UnitAnimationState.AttackLeft];
    }
}
