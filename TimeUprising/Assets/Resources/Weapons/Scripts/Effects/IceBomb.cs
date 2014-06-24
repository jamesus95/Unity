using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceBomb : MonoBehaviour
{   
    public IceBlock IceBlockPrefab;
    // TODO replace with states
    bool explosionDone = false;
        
    float kLiveTimer = 1.8f;
    float kExplosionDuration = 0.1f;
    float kStartTime;
    
    void Update ()
    {
        if (Time.time - kStartTime > kLiveTimer) {
            Destroy (this.gameObject);
        }
        
        if (Time.time - kStartTime > kExplosionDuration)
            explosionDone = true;
    }
    
   void OnTriggerEnter2D (Collider2D other)
    {
        OnTriggerStay2D (other);
    }
    
    void OnTriggerStay2D (Collider2D other)
    {
        if (explosionDone)
            return;
            
        Target target = other.gameObject.GetComponent<Target> ();
        
        if (target == null || target.Allegiance == mSource.Allegiance)
            return;
        
        int damage = (int)WeaponStats.GetStat(WeaponType.IceWand, WeaponStat.Damage);
        if (target is Tower) {
            return;
        }
        
        if (! (target is Unit))
            return;
            
        // assuming that the target is a Unit
        
        IceBlock f = other.gameObject.GetComponent<IceBlock> ();
        
        // do not retarget frozen units
        if (f != null) 
            return;
            
        Unit unit = (Unit) target;

        for (int i = unit.Squad.SquadMembers.Count - 1; i >= 0; --i) {
            Unit u = unit.Squad.SquadMembers[i];
            
            u.gameObject.AddComponent ("IceBlock");
            f = u.gameObject.GetComponent<IceBlock> ();
            f.Freeze (u);
                                                                                                                   
            u.Damage (damage);
            if (u.IsDead)
                f.Unfreeze ();
            
            if (u.IsDead && mSource.Allegiance == Allegiance.Rodelle)
                UnitStats.AddToExperience (mSource.UnitType, 1);
        }
    }
    
    Unit mSource;
    
    public void SetParameters (Unit src, Target target)
    {
        mSource = src;
        transform.position = target.Position;
        kStartTime = Time.time;
    }   
}
