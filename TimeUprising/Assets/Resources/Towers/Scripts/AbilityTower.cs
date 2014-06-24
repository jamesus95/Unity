using UnityEngine;
using System.Collections;

public class AbilityTower: Tower
{
    public Ability ability;
    public SpriteRenderer Glow;

    public override void UseTargetedAbility (Target target)
    {
        if (this.Allegiance != Allegiance.Rodelle)
            return;
            
        if (target == this) 
            return;
            
        ability.UseAbility (target);
    }

    public override void SetDestination (Vector3 destination)
    {
        if (this.Allegiance != Allegiance.Rodelle)
            return;
            
        ability.UsePositionalAbility(destination);
    }
    
    void Update()
    {
        if (Glow == null)
            return;
        
        Glow.enabled = (ability.CountdownTimer <= 0);
    }
    
    protected override void Start ()
    {
        if (ability is TowerBomb)
            mTowerType = BonusSubject.AOETower;
        else if (ability is Heal)
            mTowerType = BonusSubject.HealTower;
        
        base.Start ();
        
        Upgrades upgrades = GameObject.Find ("UI").GetComponent<Upgrades> ();
        float cooldownBonus = upgrades.GetBonus(this.mTowerType, BonusType.CoolDown);
       
        ability.CoolDown /= cooldownBonus;
    }
}
