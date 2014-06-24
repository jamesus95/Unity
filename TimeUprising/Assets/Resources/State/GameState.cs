using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState
{
    public static bool IsDebug = true;

    
    public static Era CurrentEra { get; set; }

    public static int GameLevel;
    public static Era GameEra { get; set; }
    
    public static int RemainingWaves;
	public static bool WonGame = false;
	public static bool LostGame = false;

    public static int Gold;
        
    private static int[] numEnemies;

    public static int KingsHealth { 
        get { return mKingsHealth; }
        set { UpdateKingsHealth(value); }
    }

    public static Dictionary<UnitType, int> UnitSquadCount { get; set; }

    public static Dictionary<UnitType, int> RequiredUnitExperience { get; set; }

    public static Dictionary<UnitType, float> SpawnTimes { get; set; }
    
    static GameState ()
    {
        // TODO pull these values from a file
        UnitSquadCount = new Dictionary<UnitType, int> ();
        UnitSquadCount.Add (UnitType.Archer, 3);
        UnitSquadCount.Add (UnitType.Swordsman, 3);
        UnitSquadCount.Add (UnitType.Mage, 1);
        
        SpawnTimes = new Dictionary<UnitType, float> ();
        SpawnTimes.Add (UnitType.Archer, 15f);
        SpawnTimes.Add (UnitType.Swordsman, 15f);
        SpawnTimes.Add (UnitType.Mage, 20f);
        
        Gold = 0;
    }
    
    public static void LoadLevel (int level)
    {
        //mGameLevel = level;
        Application.LoadLevel ("Level" + level.ToString ());
    }
    
    public static void TriggerLoss ()
    {
        GameObject.Find("Dialogue").GetComponent<DialogueManager>().TriggerRealtimeDialogue("GameLost");
        Time.timeScale = 0;
    
		LostGame = true;
        SaveLoad.SaveGameState ();
    }
    
    public static void TriggerWin ()
    {
        GameObject.Find("Dialogue").GetComponent<DialogueManager>().TriggerRealtimeDialogue("GameWon");
        Time.timeScale = 0;
        
		WonGame = true;
        if ((int)CurrentEra == 6 || (int)CurrentEra == (int)GameEra)
        {
            CurrentEra = (Era)((int)GameEra + 1);
        }
        SaveLoad.SaveGameState ();
    }
    
    public static void UpdateKingsHealth (int value)
    {
        mKingsHealth = value;
        float maxHealth = UnitStats.GetStat(UnitType.King, UnitStat.Health);
        
        if (mKingsHealth < (int)(0.75 * maxHealth))
            GameObject.Find("Dialogue").GetComponent<DialogueManager>().TriggerWarning("KingDamaged");
        
        if (mKingsHealth < (int)(0.35 * maxHealth))
            GameObject.Find("Dialogue").GetComponent<DialogueManager>().TriggerWarning("KingInjured");
        
        if (mKingsHealth <= 0)
            TriggerLoss ();
    }
    
    private static int mKingsHealth;
}
