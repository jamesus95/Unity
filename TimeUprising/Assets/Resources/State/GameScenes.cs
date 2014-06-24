using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScenes : MonoBehaviour{


    Dictionary<string, string> LevelsAndCutScenes = new Dictionary<string,string >();
    Dictionary<string, string> LevelsOnly = new Dictionary<string,string >();
    
	// Use this for initialization
	void Start () {
        LevelsOnly.Add("Prehistoric_1","Prehistoric_2");
        LevelsOnly.Add("Prehistoric_2","Medieval_1");
        LevelsOnly.Add("Medieval_1","Medieval_2");
        LevelsOnly.Add("Medieval_2","Japanese_1");
        LevelsOnly.Add("Japanese_1","Japanese_2");

        LevelsAndCutScenes.Add("TutorialCutScene","Prehistoric_1");
        LevelsAndCutScenes.Add("Prehistoric_1","Prehistoric_2");
        LevelsAndCutScenes.Add("Prehistoric_2","Prologue");
        LevelsAndCutScenes.Add("Prologue","Medieval_1");
        LevelsAndCutScenes.Add("Medieval_1","Medieval_1_CutScene");
        LevelsAndCutScenes.Add("Medieval_1_CutScene","TowerStore");
        LevelsAndCutScenes.Add("Medieval_2","Japanese_1_CutScene");
        LevelsAndCutScenes.Add("Japanese_1_CutScene","Japanese_1");
        LevelsAndCutScenes.Add("Japanese_1","Japanese_2");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public string GetNextLevelOnly(string currentLevel){
        return LevelsOnly[currentLevel];
    }
    public string GetNextLevelAndCutScenes(string currentScene){
        return LevelsAndCutScenes[currentScene];
    }
}
