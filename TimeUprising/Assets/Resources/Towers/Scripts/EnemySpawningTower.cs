using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawningTower : Tower
{
    ///////////////////////////////////////////////////////////////////////////////////
    // Unity Inspector Presets
    ///////////////////////////////////////////////////////////////////////////////////
    public List<GameObject> SpawnPoints;
    public List<GameObject> SpawnWaypoint;
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Public
    ///////////////////////////////////////////////////////////////////////////////////

    public override void SetDestination (Vector3 destination)
    {
        // does nothing
    }
    
    public override void UseTargetedAbility (Target Target)
    {
        // does nothing
    }

    private void SpawnEnemyUnit()
    {
        if (mGarrisonedPeasants <= 0)
            return;
        
        int squadSize = Mathf.Min (mGarrisonedPeasants, 3);
        
        mGarrisonedPeasants -= (squadSize + 1); // lose a peasant when being armed
        
        UnitType unitType = RandomUnitType();
        
        // random spawn point and waypoint
        int r = Random.Range (0, SpawnPoints.Count);
        Vector3 spawnLocation = SpawnPoints[r].transform.position;
        Vector3 destination = SpawnWaypoint[r].transform.position;
        
        GameObject.Find ("AI").GetComponent<EnemyAI> ().AddSquad (
            squadSize, spawnLocation, unitType, destination);
    }
    
    private UnitType RandomUnitType()
    {
        int type = Random.Range(0, 3);
        
        switch (type) {
        case (0):
            return UnitType.Archer;
        case (1):
            return UnitType.Mage;
        case (2):
            return UnitType.Swordsman;
        default:
            return UnitType.Swordsman;
        }
    }
   
    ///////////////////////////////////////////////////////////////////////////////////
    // Private
    /////////////////////////////////////////////////////////////////////////////////// 
    
    private float mEnemySpawnTime = 3; // 3 seconds for peasants to arm themselves
    private float mEnemySpawnTimer;
    private int mGarrisonedPeasants;
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Unity Overrides
    ///////////////////////////////////////////////////////////////////////////////////
    
    void Update ()
    {        
        // Only spawns squads if the enemy owns the tower
        if (this.Allegiance == Allegiance.Rodelle) {
            // reset the spawn timer and number of peasants
            mGarrisonedPeasants = 0;
            mEnemySpawnTimer = mEnemySpawnTime;
            return;
        }
        
        mEnemySpawnTimer -= Time.deltaTime;
        
        if (mEnemySpawnTimer < 0) {
            mEnemySpawnTimer = mEnemySpawnTime;
            this.SpawnEnemyUnit ();
        }
    }
    
    void OnTriggerStay2D (Collider2D other)
    {
        if (this.Allegiance == Allegiance.Rodelle)
            return;
        
        Unit unit = other.gameObject.GetComponent<Unit> ();
        
        if (unit != null && unit.UnitType == UnitType.Peasant) {
            unit.Despawn ();
            mGarrisonedPeasants ++;
        }
    }
    
    protected override void Start ()
    {
        base.Start ();
        this.Allegiance = Allegiance.Rodelle;
        mEnemySpawnTimer = mEnemySpawnTime;
    }
}
