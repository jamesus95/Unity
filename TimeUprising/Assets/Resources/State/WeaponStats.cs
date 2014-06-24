using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public enum WeaponStat
{
    Damage = 0,
    Range = 1,
    ReloadTime = 2,
    ReloadVariance = 3,
    Accuracy = 4,
}

public static class WeaponStats
{   
    ///////////////////////////////////////////////////////////////////////////////////
    // Public
    ///////////////////////////////////////////////////////////////////////////////////
    
    static WeaponStats ()
    {
        int numWeaponTypes = Enum.GetValues(typeof(WeaponType)).Length;
        int numStats = Enum.GetValues(typeof(WeaponStat)).Length;
        mWeaponStats = new float[numWeaponTypes, numStats];      
        
        for (int i = 0; i < numWeaponTypes; i ++)
            for (int j = 0; j < numStats; j++)
                mWeaponStats [i, j] = 0f;
        
        LoadStatsFromFile ("Data/default_weaponstats.txt");
    }
    
    public static float GetStat (WeaponType weapon, WeaponStat stat)
    {
        return mWeaponStats [(int)weapon, (int)stat];
    }
    
    public static void SetStat (WeaponType weapon, WeaponStat stat, float value)
    {
        mWeaponStats [(int)weapon, (int)stat] = value;
    }
    
    /*
    public static void WriteStats ()
    {
        StreamWriter writer = new StreamWriter (filepath);
        
        foreach (string sub in Enum.GetNames(typeof(GameType))) {
            foreach (string bon in Enum.GetNames(typeof(weaponStat))) {
                writer.WriteLine (sub + "," + bon + "," +
                                  mstatArray [(int)Enum.Parse (typeof(GameType), sub), 
                             (int)Enum.Parse (typeof(weaponStat), bon)].ToString ());
            }
        }
        writer.Close ();
    }*/
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Private
    ///////////////////////////////////////////////////////////////////////////////////
    
    static float[,] mWeaponStats;
    
    private static void LoadStatsFromFile (string filepath)
    {
        StreamReader file = new StreamReader (filepath);
        char[] delim = { ' ', ',' };
        
        while (!file.EndOfStream) {
            string line = file.ReadLine ();
            
            string [] values = line.Split (delim, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length == 0 || values[0]== "#")
                continue;
            
            WeaponType WeaponType = EnumUtil.FromString<WeaponType>(values[0]);
            
            int statIndex = 1;
            foreach (WeaponStat s in EnumUtil.GetValues<WeaponStat>()) {
                mWeaponStats [(int)WeaponType, (int)s] = float.Parse (values [statIndex]);
                statIndex ++;
            }
        }
        file.Close ();
    }  
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Unity Overrides
    ///////////////////////////////////////////////////////////////////////////////////
    
}