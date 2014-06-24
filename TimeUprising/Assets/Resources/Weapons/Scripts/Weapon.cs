using UnityEngine;
using System.Collections;

public enum WeaponType
{
    Crossbow,
    Dagger,
    IceWand,
    Sword,
    Pitchfork,
    RaptorClaws,
    AoePortal,
}

public abstract class Weapon
{
    public int Damage { get; set; }

    public float Range { get; set; }

    public float ReloadTime { get; set; }
    
    public float ReloadVariance { get; set; }

    public float Accuracy { get; set; }
    
    public WeaponType WeaponType { 
        get { return mWeaponType; }
    }
    
    protected float reloadTimer;
    protected int mLevel;
    
    public Weapon(int level)
    {
        mLevel = level;
    }
    
    public virtual bool Attack (Target src, Target target)
    {
        if (target == null || src == null)
            return false;
        
        reloadTimer -= Time.deltaTime;
        if (reloadTimer < 0) {
            PerformAttack (src, target);
            return true;
        }
        return false;
    }
    
    public virtual void Reset ()
    {
        reloadTimer = ReloadTime * 0.5f;
        //reloadTimer = Random.Range (ReloadTime * (1f - ReloadVariance), ReloadTime * (1f + ReloadVariance));
    }
    
    protected virtual void PerformAttack (Target src, Target target)
    {
        if (target == null)
            return;
        
        reloadTimer = Random.Range (ReloadTime * (1f - ReloadVariance), ReloadTime * (1f + ReloadVariance));
        
        if (Random.value < this.Accuracy)
            target.Damage (this.Damage);
    }
    
    protected WeaponType mWeaponType;
    
    protected void InitializeStats()
    {
        this.Damage = (int)WeaponStats.GetStat(mWeaponType, WeaponStat.Damage);
        this.Range = WeaponStats.GetStat(mWeaponType, WeaponStat.Range);
        this.ReloadTime = WeaponStats.GetStat(mWeaponType, WeaponStat.ReloadTime);
        this.ReloadVariance = WeaponStats.GetStat(mWeaponType, WeaponStat.ReloadVariance);
        this.Accuracy = WeaponStats.GetStat(mWeaponType, WeaponStat.Accuracy);
        
        this.UpgradeWeapon(mLevel);
        Reset ();
    }
    
    protected virtual void UpgradeWeapon(int level)
    {
        float newDamage = (this.Damage * (1 + (float)level / 10f)); // 10% increase every level
        this.Damage = (int)(newDamage); 
        
        float newAccuracy = this.Accuracy + ((level-1) / 5) / 5f; // 5% increase every 5 levels
        this.Accuracy = newAccuracy; 
    }
}