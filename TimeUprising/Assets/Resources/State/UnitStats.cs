using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public enum UnitStat
{
    Health = 0,
    MovementSpeed,
    ChargeSpeed,
    SightRange,
    Level,
    Experience,
    GoldReward,
}

public static class UnitStats
{   
    private static int kLevelSoftCap = 10; // levels become harder to earn after this level
    public static int kLevelHardCap = 20;

    private static string kUnitDataPath = "Data/default_unitstats.txt";
    
    private static string kBaseUnitLevelPath = "Data/default_unitlevels.txt";
    private static string kUnitLevelPath = SaveLoad.kSaveFileDirectory + "SavedUnitLevels.save";
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Public
    ///////////////////////////////////////////////////////////////////////////////////
    
    static UnitStats ()
    {
        int numUnitTypes = Enum.GetValues(typeof(UnitType)).Length;
        int numStats = Enum.GetValues(typeof(UnitStat)).Length;
        mUnitStats = new float[numUnitTypes, numStats];      
        
        int numEras = Enum.GetValues(typeof(UnitStat)).Length;
        mUnitLevels = new int[numUnitTypes, numEras];
        mUnitExperience = new int[numUnitTypes, numEras];
        
        LoadStatsFromFile (kUnitDataPath);

        if (File.Exists(kUnitLevelPath))
            LoadLevelsFromFile (kUnitLevelPath);
        else
            LoadLevelsFromFile (kBaseUnitLevelPath);
    }
    
    public static float GetStat (UnitType unit, UnitStat stat)
    {
        Era currentEra = GameState.GameEra;
        
        if (stat == UnitStat.Level)
            return mUnitLevels[(int) unit, (int) currentEra];
        
        if (stat == UnitStat.Experience)
            return mUnitExperience[(int) unit, (int) currentEra];
                    
        return mUnitStats [(int)unit, (int)stat];
    }
    
    public static void SetStat (UnitType subject, UnitStat stat, float value)
    {
        if (stat == UnitStat.Level || stat == UnitStat.Experience)
            return;
            
        mUnitStats [(int)subject, (int)stat] = value;
    }
    
    public static void AddToExperience (UnitType subject, int value)
    {
        if (subject == UnitType.Peasant)
            return;
     
        Era currentEra = GameState.GameEra;
        int unitLevel = (int)mUnitLevels[(int) subject, (int) currentEra];
        if (unitLevel >= kLevelHardCap)
            return;      
        
        mUnitExperience [(int)subject, (int)currentEra] += value;
        if (mUnitExperience [(int)subject, (int)currentEra] > GetExpToNextLevel(subject)) {
            IncreaseUnitLevel(subject);
        }
    }
    
    public static int GetExpToNextLevel(UnitType unit)
    {
        Era currentEra = GameState.GameEra;
        int unitLevel = (int)mUnitLevels[(int) unit, (int) currentEra];

        // exp = 30 + 2x + (max(x,10) - 9)^2
        // levels become exponentially more difficult to attain after reaching the level soft cap
        // exp from  5 to  6 is 41
        // exp from 10 to 11 is 51
        // exp from 15 to 16 is 96
        
        int softCapPenalty = Math.Max (unitLevel, kLevelSoftCap) - kLevelSoftCap - 1;
        return softCapPenalty * softCapPenalty + unitLevel * 2 + 30;
    }
    
    public static void ResetLevels()
    {
        int numUnitTypes = Enum.GetValues(typeof(UnitType)).Length;
        int numEras = Enum.GetValues(typeof(UnitStat)).Length;
        mUnitLevels = new int[numUnitTypes, numEras];
        mUnitExperience = new int[numUnitTypes, numEras];
                
        LoadLevelsFromFile (kBaseUnitLevelPath);
        SaveLevels();
    }
    
    public static void SaveLevels()
    {
        // TODO create temporary backup before writing to file
        WriteLevels(kUnitLevelPath);
    }
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Private
    ///////////////////////////////////////////////////////////////////////////////////
    
    static float[,] mUnitStats;
    static int[,] mUnitLevels;
    static int[,] mUnitExperience;
    
    private static void IncreaseUnitLevel(UnitType unit)
    {
        Era currentEra = GameState.GameEra;
        
        mUnitStats[(int) unit, (int) UnitStat.Health] += 1;
        
        mUnitLevels[(int) unit, (int) currentEra] += 1;
        mUnitExperience[(int) unit, (int) currentEra] = 0;
    }
    
    private static void WriteLevels (string filepath)
    {
        StreamWriter writer = new StreamWriter (filepath);
        
        foreach (Era era in EnumUtil.GetValues<Era>()) {
            if (era == Era.None)
                continue;
            
            writer.WriteLine(FormatLevelData(UnitType.Swordsman, era));
            writer.WriteLine(FormatLevelData(UnitType.Archer, era));
            writer.WriteLine(FormatLevelData(UnitType.Mage, era));
            writer.WriteLine();
        }
        
        writer.Close ();
    }
    
    private static string FormatLevelData(UnitType unitType, Era era)
    {
        string data = "";
        data += String.Format("{0,-20}", unitType.ToString());
        data += " ";
        data += String.Format("{0,-20}", era.ToString());
        data += " ";
        data += String.Format("{0,5}", mUnitLevels[(int) unitType, (int) era]);
        data += " ";
        data += String.Format("{0,5}", mUnitExperience[(int) unitType, (int) era]);
        
        return data;
    }
    
    private static void LoadStatsFromFile (string filepath)
    {
        StreamReader file = new StreamReader (filepath);
        char[] delim = { ' ', ',' };
        
        while (!file.EndOfStream) {
            string line = file.ReadLine ();
            
            string [] values = line.Split (delim, StringSplitOptions.RemoveEmptyEntries);
            if (values[0]== "#")
                continue;
                
            UnitType unitType = EnumUtil.FromString<UnitType>(values[0]);
            
            int statIndex = 1;
            foreach (UnitStat s in EnumUtil.GetValues<UnitStat>()) {
                if (s == UnitStat.Level || s == UnitStat.Experience)
                    continue;
                mUnitStats [(int)unitType, (int)s] = float.Parse (values [statIndex]);
                statIndex ++;
            }
        }
        
        file.Close ();
    }  
    
    private static void LoadLevelsFromFile (string filepath)
    {
        StreamReader file = new StreamReader (filepath);
        char[] delim = { ' ', ',' };
        
        while (!file.EndOfStream) {
            string line = file.ReadLine ();
            
            string [] values = line.Split (delim, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length == 0 || values[0]== "#") // Ignore comments and blank lines
                continue;
            
            // FileFormat: "<UnitType>  <Era>   <Level>   <Experience>"
            UnitType unitType = EnumUtil.FromString<UnitType>(values[0]);
            Era era = EnumUtil.FromString<Era>(values[1]);
            
            mUnitLevels[(int)unitType, (int)era] = int.Parse(values[2]);
            mUnitExperience[(int)unitType, (int)era] = int.Parse(values[3]);
        }

        file.Close ();
    }
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Unity Overrides
    ///////////////////////////////////////////////////////////////////////////////////
    
}