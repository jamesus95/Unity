using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerBombEffect : MonoBehaviour
{
    public void ExplodeAt (Vector3 location, int damage)
    {
        mDamage = damage;
        this.transform.position = location;
        
        mStartTime = Time.time;
    }

    private enum ExplosionState {
        Idle,
        Started,
        Finished,
    }
    
    private ExplosionState mExplosionState = ExplosionState.Idle;
    
    float kLiveTimer = 1.8f;
    float mStartTime;
    
    private int mDamage;
    
    void Update ()
    {
        if (Time.time - mStartTime > kLiveTimer) {
            Destroy (this.gameObject);
        }
        
        this.transform.Rotate(new Vector3(0, 0, 100f * Time.smoothDeltaTime));
    }
    
    void OnTriggerEnter2D (Collider2D other)
    {
        OnTriggerStay2D (other);
    }
    
    void OnTriggerStay2D (Collider2D other)
    {
        if (mExplosionState == ExplosionState.Finished)
            return;
            
        mExplosionState = ExplosionState.Started;
        
        Target target = other.gameObject.GetComponent<Target> ();
        
        if (target is Unit)
            target.Damage (mDamage, new AoePortal(0));
    }
   
    void FixedUpdate()
    {
        if (mExplosionState == ExplosionState.Started)
            mExplosionState = ExplosionState.Finished;
    }
}

public class AoePortal : Weapon 
{
    public AoePortal (int level) : base (level)
    {                
        mWeaponType = WeaponType.AoePortal;
    }
}