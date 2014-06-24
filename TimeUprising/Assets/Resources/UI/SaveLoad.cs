using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SaveLoad : MonoBehaviour {
    public static readonly string kSaveFileDirectory = "Data/Saves/";

    public static void ResetGameState () 
    {
        Upgrades.ResetTowerUpgrades ();
        UnitStats.ResetLevels ();
        
        if(File.Exists(kLevelPath))
            File.Delete(kLevelPath);
            
        if(File.Exists(kUpgradesPath))
            File.Delete(kUpgradesPath);
            
        GameState.Gold = 0;
        GameState.CurrentEra = Era.Prehistoric;
    }
    
    public static void SaveGameState ()
    {
        SaveLoad s = GameObject.Find("SaveLoad").GetComponent<SaveLoad>();
        
        string savedEra = ((int)GameState.CurrentEra + 1).ToString();
        string savedGold = GameState.Gold.ToString();
        
        s.Clear(SaveLoad.SAVEFILE.Level);
        s.Add(savedEra, SaveLoad.SAVEFILE.Level);
        s.Add(savedGold, SaveLoad.SAVEFILE.Level);
        s.Save();
        
        UnitStats.SaveLevels ();
    }

    public enum SAVEFILE
    {
        Level,
        Upgrade
    }

    private static readonly string kLevelPath = kSaveFileDirectory + "game.save";
    private static readonly string kUpgradesPath = kSaveFileDirectory + "upgrades.save";
    // Level file contains
    // line 1: highest level
    // line 2: gold
    private List<string> levelStrings;
    private List<string> upgradeStrings;

    private bool loadSuccess;

    void Awake()
    {
        Instantiate();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Instantiate()
    {
        levelStrings = new List<string>();
        upgradeStrings = new List<string>();
        loadSuccess = false;
    }

    public void Clear(SAVEFILE file)
    {
        if (file == SAVEFILE.Level)
        {
            levelStrings.Clear();
        }
        else if (file == SAVEFILE.Upgrade)
        {
            upgradeStrings.Clear();
        }
    }

    public void Add(string info, SAVEFILE type)
    {
        if (type == SAVEFILE.Level)
        {
            levelStrings.Add(info);
        }
        else if (type == SAVEFILE.Upgrade)
        {
            upgradeStrings.Add(info);
        }
    }

    public void Save()
    {
        string[] level = new string[levelStrings.Count];
        string[] upgrade = new string[upgradeStrings.Count];
        int i = 0;
        foreach (string s in levelStrings)
        {
            level[i] = s;
            i++;
        }
        i = 0;
        foreach (string s in upgradeStrings)
        {
            upgrade[i] = s;
            i++;
        }
        System.IO.File.WriteAllLines(kUpgradesPath, upgrade);
        System.IO.File.WriteAllLines(kLevelPath, level);
    }

    public void Load(SAVEFILE file)
    {
        if (file == SAVEFILE.Level)
        {
            try
            {
                string[] level = System.IO.File.ReadAllLines(kLevelPath);
                Clear(file);
                foreach (string s in level)
                {
                    levelStrings.Add(s);
                }
                loadSuccess = true;
            }
            catch (System.Exception e) { 
                Debug.Log (e);
                loadSuccess = false;
			}
        }
        else if (file == SAVEFILE.Upgrade)
        {
            try
            {
                string[] upgrades = System.IO.File.ReadAllLines(kUpgradesPath);
                Clear(file);
                foreach (string s in upgrades)
                {
                    upgradeStrings.Add(s);
                }
                loadSuccess = true;
            }
            catch (System.Exception e) {
                Debug.Log (e);
                loadSuccess = false;
			}
        }
    }

    public bool LoadSuccessful()
    {
        return loadSuccess;
    }

    public List<string> GetInfo(SAVEFILE type)
    {
        if (type == SAVEFILE.Level)
        {
            return levelStrings;
        }
        else if (type == SAVEFILE.Upgrade)
        {
            return upgradeStrings;
        }
        return null;
    }

}
