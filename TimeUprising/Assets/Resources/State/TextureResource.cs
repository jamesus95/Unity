using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TextureResource 
{
    public static GameObject GetUnitPrefab(UnitType unitType, bool isFriendly = true)
    {
        if (GameState.GameEra != mLoadedEra)
            InitializeResources(GameState.GameEra);
            
        if (isFriendly)
            return mFriendlyUnitPrefabs[unitType];
        else 
            return mEnemyUnitPrefabs[unitType];
    }

    public static void InitializeResources(Era era)
    {
        if (era == mLoadedEra)
            return;
            
        mLoadedEra = era;
        InitializeUnitPrefabs(mLoadedEra);
    }

    private static Era mLoadedEra; // textures for this era are currently loaded
    
    private static Dictionary<BonusSubject, Renderer> mTowerTextures;
    private static Dictionary<UnitType, GameObject> mFriendlyUnitPrefabs;
    private static Dictionary<UnitType, GameObject> mEnemyUnitPrefabs;    

    private static void InitializeUnitPrefabs(Era era)
    {
        mFriendlyUnitPrefabs = new Dictionary<UnitType, GameObject> ();
        mFriendlyUnitPrefabs.Add (UnitType.Swordsman, UnitPrefab(era, "SwordsmanPrefab"));
        mFriendlyUnitPrefabs.Add (UnitType.Archer, UnitPrefab(era, "ArcherPrefab"));
        mFriendlyUnitPrefabs.Add (UnitType.Mage, UnitPrefab(era, "MagePrefab"));
        mFriendlyUnitPrefabs.Add (UnitType.King, UnitPrefab(era, "KingPrefab"));
        
        mEnemyUnitPrefabs = new Dictionary<UnitType, GameObject> ();
        mEnemyUnitPrefabs.Add (UnitType.Swordsman, UnitPrefab(era, "EnemySwordsmanPrefab"));
        mEnemyUnitPrefabs.Add (UnitType.Archer, UnitPrefab(era, "EnemyArcherPrefab"));
        mEnemyUnitPrefabs.Add (UnitType.Peasant, UnitPrefab(era, "EnemyPeasantPrefab"));
        mEnemyUnitPrefabs.Add (UnitType.Mage, UnitPrefab(era, "EnemyMagePrefab"));
        mEnemyUnitPrefabs.Add (UnitType.Elite, UnitPrefab(era, "EnemyElitePrefab"));
    }
    
    private static GameObject UnitPrefab(Era era, string name)
    {
        string prefabPath = "Units/Prefabs/";
        
        prefabPath += era.ToString();
        prefabPath += "/";
        prefabPath += name;
        
        return Resources.Load (prefabPath) as GameObject;
    }
    
    static TextureResource()
    {
        mLoadedEra = Era.None;
    }
}
