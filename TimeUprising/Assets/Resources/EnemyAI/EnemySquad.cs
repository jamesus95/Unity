using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class EnemySquad
{
    ///////////////////////////////////////////////////////////////////////////////////
    // Public
    ///////////////////////////////////////////////////////////////////////////////////
    public bool IsDead { get; set; }
    
    public EnemySquad (Squad squad, Vector3 destination)
    {
        mWaypoints = new List<Vector3> ();
        mWaypoints.Add(destination);
        mSquad = squad;
        mSquad.SetDestination(mWaypoints[0]);
    }

    public EnemySquad (int size, float spawnTime, 
	                   Vector3 spawnLocation,
                       UnitType unitType,
                       SquadLeaderType leaderType = SquadLeaderType.None)
    {
        if (mSquadPrefab == null) 
            mSquadPrefab = Resources.Load ("Squads/SquadPrefab") as GameObject;
        
        mSpawnLocation = spawnLocation;

        mSquadSize = size;
        mSpawnTime = spawnTime;
        mLeaderType = leaderType;
        mUnitType = unitType;
        
        mWaypoints = new List<Vector3> ();
    }
    
    public void AddWaypoint (Vector3 waypoint)
    {
        mWaypoints.Add (waypoint);
    }
    
    public void Update (float deltaTime)
    {
        if (IsDead = (mSpawnTime < 0 && mSquad == null))
            return;
        
        mSpawnTime -= deltaTime; // update spawn timer
        if (mSquad == null && mSpawnTime < 0)
            this.SpawnSquad (mSquadSize, mSpawnLocation, mLeaderType, mUnitType);
        
        UpdateDestination ();
    }
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Private
    ///////////////////////////////////////////////////////////////////////////////////
    private static GameObject mSquadPrefab;
    private Vector3 mSpawnLocation;
    private Squad mSquad;
    private List<Vector3> mWaypoints;
    private float mSpawnTime; // time before squad spawns
        
    private int mCurrentWaypoint;
    private int mSquadSize;
	private SquadLeaderType mLeaderType;
    private UnitType mUnitType;

    private void UpdateDestination ()
    {
        if (mSquad == null) // squad has not spawned yet
            return;
              
        if (mCurrentWaypoint < mWaypoints.Count - 1 && mSquad != null && mSquad.IsIdle) {
            mCurrentWaypoint ++;
            mSquad.SetDestination (mWaypoints [mCurrentWaypoint]);
        }
    }
    
    private void SpawnSquad (int size, Vector3 spawnLocation, SquadLeaderType leader, UnitType unitType)
    {
        GameObject o = (GameObject)GameObject.Instantiate (mSquadPrefab);
        Squad squad = o.GetComponent<Squad> ();
        
        UnitType squadLeaderType = UnitTypeFromPreset(leader);
        
        if (squadLeaderType == UnitType.None)
            squadLeaderType = unitType; // squad leader is the same type as the unit
        
        squad.AddUnits(squadLeaderType, spawnLocation, 1, Allegiance.AI);
        squad.AddUnits(unitType, spawnLocation, size - 1, Allegiance.AI);
        
        mSquad = squad;
        mSquad.SetDestination (mWaypoints [0]);
    }
    
    private UnitType UnitTypeFromPreset(SquadLeaderType preset)
    {
        switch (preset){
        case (SquadLeaderType.Default):
            return UnitType.Peasant;
        case (SquadLeaderType.Melee):
            return UnitType.Swordsman;
        case (SquadLeaderType.Ranged):
            return UnitType.Archer;
        case (SquadLeaderType.Special):
            return UnitType.Mage;
        case (SquadLeaderType.Elite):
            return UnitType.Elite;
        default:
            return UnitType.None;
        }
    
    }
}
