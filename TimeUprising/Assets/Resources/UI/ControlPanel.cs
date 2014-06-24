using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlPanel : MonoBehaviour
{
    public Progressbar KingsHealthBar;
    public Progressbar SwordsmanExperienceBar;
    public Progressbar ArcherExperienceBar;
    public Progressbar MageExperienceBar;
    
    public Progressbar HealCooldownBar;
    public Progressbar AoeCooldownBar;
    public Progressbar BoostCooldownBar;
    
    public GUIText SwordsmanLevel;
    public GUIText ArcherLevel;
    public GUIText MageLevel;
    
    public GUIText GoldCounterText;
    public GUIText WaveCounter;
    public AudioSource Music;
       
    public AbilityTower HealTower;
    public AbilityTower AoeTower;
    public AbilityTower BoostTower;

	public float mSFXVolume{get; set;}
    
    private Dictionary<UnitType, Progressbar> mExpBars;
    private Dictionary<UnitType, GUIText> mLevelText;
    
    private Dictionary<BonusSubject, GUIText> mCooldownText;
    private Dictionary<BonusSubject, Progressbar> mCooldownBars;
    private Dictionary<BonusSubject, AbilityTower> mAbilityTowers;

    void Awake ()
    {
        mSFXVolume = 1f;
        if (HealTower == null)
            Destroy (HealCooldownBar);
            
        if (AoeTower == null)
            Destroy (AoeCooldownBar);
            
        if (BoostTower == null)
            Destroy (BoostCooldownBar);
            
        mCooldownText = new Dictionary<BonusSubject, GUIText>();
        mCooldownText.Add(BonusSubject.HealTower, GameObject.Find("txtHealCooldown").GetComponent<GUIText>());
        mCooldownText.Add(BonusSubject.AOETower, GameObject.Find("txtAoeCooldown").GetComponent<GUIText>());
        mCooldownText.Add(BonusSubject.BuffTower, GameObject.Find("txtBoostCooldown").GetComponent<GUIText>());
        
        mCooldownBars = new Dictionary<BonusSubject, Progressbar>();
        mCooldownBars.Add(BonusSubject.HealTower, GameObject.Find("HealCooldownBar").GetComponent<Progressbar>());
        mCooldownBars.Add(BonusSubject.AOETower, GameObject.Find("AoeCooldownBar").GetComponent<Progressbar>());
        mCooldownBars.Add(BonusSubject.BuffTower, GameObject.Find("BoostCooldownBar").GetComponent<Progressbar>());
    }
    
    void Start ()
    {
        if (KingsHealthBar == null || SwordsmanExperienceBar == null || 
            MageExperienceBar == null || ArcherExperienceBar == null) {
            Debug.LogError ("Experience and health bars need to be attached to the game manager!");
        }
        
        mExpBars = new Dictionary<UnitType, Progressbar> ();
        mExpBars.Add (UnitType.Archer, ArcherExperienceBar);
        mExpBars.Add (UnitType.Swordsman, SwordsmanExperienceBar);
        mExpBars.Add (UnitType.Mage, MageExperienceBar);
        
        mLevelText = new Dictionary<UnitType, GUIText>();
        mLevelText.Add(UnitType.Archer, ArcherLevel);
        mLevelText.Add(UnitType.Swordsman, SwordsmanLevel);
        mLevelText.Add(UnitType.Mage, MageLevel);
        
        KingsHealthBar.MaxValue = GameState.KingsHealth;
        
        // TODO refactor these hard coded values
        SpriteRenderer sr = GameObject.Find ("ControlPanel").GetComponent<SpriteRenderer>();
        Era era = GameState.GameEra;
        string controlPanelSpritePath = "UI/Textures/" + era.ToString() + "UI";
        sr.sprite = Resources.Load<Sprite>(controlPanelSpritePath);
    }
    
    void Update ()
    {
        foreach (UnitType u in mExpBars.Keys) {
            mExpBars [u].MaxValue = UnitStats.GetExpToNextLevel(u);
            mExpBars [u].UpdateValue ((int)UnitStats.GetStat(u, UnitStat.Experience));
            mLevelText [u].text = ((int)UnitStats.GetStat(u, UnitStat.Level)).ToString();
        }
        
        // There is no special unit in the prehistoric era
        if (GameState.GameEra == Era.Prehistoric)
            mLevelText [UnitType.Mage].text = "";
        
        UpdateCooldownBar(HealCooldownBar, HealTower);
        UpdateCooldownTimer(mCooldownText[BonusSubject.HealTower], HealTower);
        
        UpdateCooldownBar(AoeCooldownBar, AoeTower);
        UpdateCooldownTimer(mCooldownText[BonusSubject.AOETower], AoeTower);
        
        UpdateCooldownBar(BoostCooldownBar, BoostTower);
        UpdateCooldownTimer(mCooldownText[BonusSubject.BuffTower], BoostTower);
        
        KingsHealthBar.UpdateValue (GameState.KingsHealth);
        GoldCounterText.text = GameState.Gold.ToString ();
        WaveCounter.text = GameState.RemainingWaves.ToString ();
        
        if (GameState.IsDebug && Input.GetKeyDown ("a"))
            Time.timeScale += 0.5f;
            
        if (GameState.IsDebug && Input.GetKeyDown ("s"))
            Time.timeScale -= 0.5f;
            
        if (GameState.IsDebug && Input.GetKeyDown ("b"))
            UnitStats.SaveLevels ();
            
        if (GameState.IsDebug && Input.GetKeyDown ("x"))
            UnitStats.ResetLevels ();
	}
    
    private void UpdateCooldownTimer(GUIText text, AbilityTower tower)
    {        
        if (text == null || tower == null) 
            return;
        
        float cooldown = tower.ability.CooldownTimer - tower.ability.CoolDown;
        cooldown = Mathf.Min(0, cooldown) * -1; // convert to a countdown
        
        string cooldownDisplay;
        
        if (cooldown <= 0) {
            cooldownDisplay = '\u2605'.ToString();
            // PlayGlowAnimation(); // TODO add this glow
        }

        else 
            cooldownDisplay = ((int)cooldown + 1).ToString();
            
        text.text = cooldownDisplay;
    }
    
    private void UpdateCooldownBar(Progressbar cooldownBar, AbilityTower tower)
    {
        if (tower != null) {
            cooldownBar.MaxValue = (int)(tower.ability.CoolDown * 100);
            cooldownBar.UpdateValue((int)(tower.ability.CooldownTimer * 100));
        }
    }
    
	public void SetMusicVolume(float v)
    {
        // Possibly don't need
		Music.volume = v;
        GameObject.Find("Background").GetComponent<AudioSource>().volume = v;
	}

}
