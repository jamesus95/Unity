using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public enum Waypoint
{
    King = 0,
    BottomLeft,
    BottomRight,
    TopLeft,
    TopRight,
    Center,
    CenterLeft,
    CenterRight,
    CenterDown,
    CenterUp,
    SpawnLeft,
    SpawnCenter,
    SpawnRight,
}

public enum SquadLeaderType
{
    None,
    Default, // all peasants
    Melee,   // contains a melee unit
    Ranged,  // contains a ranged unit
    Special, // contains a special unit
    Elite,   // has a raptor!
}

public enum SquadSize
{
    Single = 1,
    Small = 2,
    Medium = 4,
    Large = 7,
}

public enum SquadBehavior
{
    AttackMove,
    ForceMove,
}

public class EnemyAI : MonoBehaviour
{
    public Dictionary<Waypoint, Vector3> Waypoints;
    public int GameLevel;
    public Era GameEra;
    
    // If a wave is finished early, the AI waits this amount of seconds before 
    // spawning the next wave
    private float kWaitTimeBeforeWave = 1f; 
    
    private void ValidatePresets ()
    {
        if (GameLevel == 0) {// || CurrentErra == Era.None
            Debug.LogError("The level or era was not set in the Unity Inspector.");
        }
    }
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Unity Overrides
    /////////////////////////////////////////////////////////////////////////////////// 
    
    void Awake ()
    {
        this.ValidatePresets();
        GameState.GameEra = this.GameEra;
        GameState.GameLevel = this.GameLevel;
        
        // TODO replace with GameState.PauseGame ()
        Time.timeScale = 1; // unpause game
    }
    
    void Update ()
    {
        float timeToNextWave = mWaveSpawnTime - Time.time;
        
        if (timeToNextWave < 0 && mCurrentWave <= mMaxWaves) {
            mCurrentWave ++;
            this.SpawnWave (mCurrentWave);
        }
        
        if (units.Count == 0 && mCurrentWave <= mMaxWaves && timeToNextWave > kWaitTimeBeforeWave) {
            mWaveSpawnTime = Time.time + kWaitTimeBeforeWave;
            GameObject.Find("Dialogue").GetComponent<DialogueManager>().TriggerChatter();
        }
        
        for (int i = units.Count - 1; i >= 0; --i) {
            if (units [i].IsDead)
                units.RemoveAt (i);
            else 
                units [i].Update (Time.deltaTime);
        }
        
        if (mCurrentWave >= mMaxWaves && units.Count == 0) {
            GameState.TriggerWin ();
        }
        
        if (GameState.IsDebug && Input.GetButtonDown ("Fire1")) {
            SpawnWave (3);
        }
    }
    
    void Start ()
    {
        units = new List<EnemySquad> ();
        
        Waypoints = new Dictionary<Waypoint, Vector3> ();
        
        Waypoints.Add (Waypoint.BottomLeft, GameObject.Find("WP_BottomLeft").transform.position);
        Waypoints.Add (Waypoint.BottomRight, GameObject.Find("WP_BottomRight").transform.position);
        Waypoints.Add (Waypoint.TopRight, GameObject.Find("WP_TopRight").transform.position);
        Waypoints.Add (Waypoint.TopLeft, GameObject.Find("WP_TopLeft").transform.position);
        Waypoints.Add (Waypoint.CenterRight, GameObject.Find("WP_CenterRight").transform.position);
        Waypoints.Add (Waypoint.CenterLeft, GameObject.Find("WP_CenterLeft").transform.position);

        Waypoints.Add (Waypoint.CenterDown, GameObject.Find("WP_CenterDown").transform.position);
        Waypoints.Add (Waypoint.CenterUp, GameObject.Find("WP_CenterUp").transform.position);

        Waypoints.Add (Waypoint.King, GameObject.Find("WP_King").transform.position);
        Waypoints.Add (Waypoint.Center, GameObject.Find("WP_Center").transform.position);
        Waypoints.Add (Waypoint.SpawnLeft, GameObject.Find("WP_SpawnLeft").transform.position);
        Waypoints.Add (Waypoint.SpawnCenter, GameObject.Find("WP_SpawnCenter").transform.position);
        Waypoints.Add (Waypoint.SpawnRight, GameObject.Find("WP_SpawnRight").transform.position);
        
        GameObject.Find ("Waypoints").SetActive(false);
                
        string aiDataPath = "Data/AI/AI_";
        aiDataPath += GameEra.ToString ();
        aiDataPath += "_";
        aiDataPath += GameLevel.ToString ();
        aiDataPath += ".txt";
        ReadWavesFromFile (aiDataPath);
        
        mCurrentWave = 1;
        this.SpawnWave (mCurrentWave);
    }

    ///////////////////////////////////////////////////////////////////////////////////
    // Public Methods and Variables
    /////////////////////////////////////////////////////////////////////////////////// 
    
    public void AddSquad(int size, Vector3 location, UnitType unitType, Vector3? destination = null)
    {
        EnemySquad es = new EnemySquad(size, 0, location, unitType);
        
        if (destination != null)
            es.AddWaypoint(destination.Value);
            
        es.AddWaypoint(Waypoints[Waypoint.King]);
        units.Add(es);
    }
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Private Methods and Variables
    /////////////////////////////////////////////////////////////////////////////////// 
    private int kMaxNumWaves = 20; // Cannot create more than 20 waves
    
    private float mWaveSpawnTime;
    private int mCurrentWave = 0;
    private int mMaxWaves;
    List<string>[] mEnemyWaves;
    float[] mWaveTimers;
    private List<EnemySquad> units;
    private Queue<Vector3> mSpawnPoint;
        
    private void SpawnWave (int waveNumber)
    {   
        if (waveNumber > mMaxWaves)
            return;
        
        GameState.RemainingWaves = mMaxWaves - mCurrentWave;
        
        mWaveSpawnTime = Time.time + mWaveTimers [waveNumber];
        
        foreach (string s in mEnemyWaves[waveNumber])
            this.AddSquad (s);
    }
    
    // Format: <Spawn Time> <Size> <Preset> <Action Type> <Waypoint>,<Waypoint>,...
    // "2.5 Large Default AttackMove ArcherTower,SwordsmanTower"
    // "1 Individual Elite ForcedMove AbilityTower"
    private void AddSquad (string input)
    {        
        float spawnTime;
        Vector3 spawnLocation;
        
        char[] delimiters = { ' ', ',' };
        string[] param = input.Split (delimiters, StringSplitOptions.RemoveEmptyEntries);
        
        spawnTime = float.Parse (param [0]);
        SquadSize size = EnumUtil.FromString<SquadSize> (param [1]);
        Waypoint wp = EnumUtil.FromString<Waypoint> (param [4]);
        spawnLocation = Waypoints[wp];
        SquadLeaderType preset = EnumUtil.FromString<SquadLeaderType> (param [2]);
        //SquadBehavior behavior = EnumHelper.FromString<SquadBehavior> (param [3]);
        
        EnemySquad es = new EnemySquad ((int)size, spawnTime, spawnLocation, UnitType.Peasant, preset);
        
        for (int i = 5; i < param.Length; ++i) {
            wp = EnumUtil.FromString<Waypoint> (param [i]);
            es.AddWaypoint (Waypoints [wp]);
        }
        es.AddWaypoint (Waypoints [Waypoint.King]);
        
        units.Add (es);
    }
    
    private void ReadWavesFromFile (string filepath)
    {
        StreamReader file = null;
        string input;
        try {
            file = new StreamReader (filepath);
            mEnemyWaves = new List<string>[kMaxNumWaves + 1]; // 0 based array
            mWaveTimers = new float[kMaxNumWaves + 1];
            
        } catch (System.Exception e) {
            Debug.LogError (e.ToString ());
        }
        
        int waveNumber = 0;
        char[] delimiters = { ' ', ',' };
        
        string[] waveData;
        while (true) {
            input = file.ReadLine ();
            if (file.EndOfStream || input == "ENDWAVES")
                break;
            
            if (input == "" || input [0] == '#') // ignore comments and blank lines
                continue;
                
            // @EnemyWave <WaveNumber> <WaveTimer>
            if (input [0] == '@') { // start of new enemy wave
                waveNumber ++;
                waveData = input.Split (delimiters);
                mWaveTimers [waveNumber] = float.Parse (waveData [1]);
                mEnemyWaves [waveNumber] = new List<string> ();
            } else { // information about the enemy wave
                mEnemyWaves [waveNumber].Add (input);
            }
        }
        
        mMaxWaves = waveNumber;
    }
}