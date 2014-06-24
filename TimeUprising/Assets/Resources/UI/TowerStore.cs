using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerStore : MonoBehaviour
{
    public GUISkin GUISkin;
    
    protected enum Button
    {
        SwordsmanUp,
        SwordsmanDown,
        ArcherUp,
        ArcherDown,
        MageUp,
        MageDown,
        
        NextLevel,
        LevelSelector,
    }
    
    protected enum Label
    {
        SwordsmanSquadCount,
        ArcherSquadCount,
        MageSquadCount,
    }
    
    Dictionary<Button, ButtonData> buttons;
    Dictionary<Label, LabelData> labels;
    
    protected delegate int LabelValue ();
    
    protected struct LabelData
    {
        public Rect Rect;

        public string Text { get { return labelValue.Invoke ().ToString (); } }
        
        private LabelValue labelValue;
        
        // Pass in a function that is used to fill the text of the label
        public LabelData (Rect r, LabelValue l)
        {
            Rect = r;
            labelValue = l;
        }
    }
    
    protected struct ButtonData
    {
        public Rect rect;
        public string text;
        
        public ButtonData (Rect r, string s)
        {
            rect = r;
            text = s;
        }
    }
    
    private void HandleClick (Button button)
    {
        switch (button) {
        case (Button.SwordsmanUp):
            GameState.UnitSquadCount [UnitType.Swordsman] ++;
            break;
        case (Button.SwordsmanDown):
            GameState.UnitSquadCount [UnitType.Swordsman] --;
            break;
        case (Button.ArcherUp):
            GameState.UnitSquadCount [UnitType.Archer]  ++;
            break;
        case (Button.ArcherDown):
            GameState.UnitSquadCount [UnitType.Archer]  --;
            break;
        case (Button.MageUp):
            GameState.UnitSquadCount [UnitType.Mage]  ++;
            break;
        case (Button.MageDown):
            GameState.UnitSquadCount [UnitType.Mage]  --;
            break;
            
        case (Button.LevelSelector):
            Application.LoadLevel ("LevelLoader");
            break;
            
        case (Button.NextLevel):
            break;
        }
    }
    
    private void LoadLevel (int level)
    {
        GameState.LoadLevel (level);
    }
    
    void OnGUI ()
    {
        foreach (Button button in buttons.Keys) {
            if (GUI.Button (buttons [button].rect, buttons [button].text))
                HandleClick (button);
        }
        
        GUI.skin = this.GUISkin;
        foreach (Label label in labels.Keys) 
            GUI.Label (labels [label].Rect, labels [label].Text);
    }
    
    void Awake ()
    {
        buttons = new Dictionary<Button, ButtonData> ();
        labels = new Dictionary<Label, LabelData> ();
        
        buttons.Add (Button.SwordsmanUp, 
                      new ButtonData (ScaleButton (new Rect (500, 230, 50, 25)), "+"));
        buttons.Add (Button.SwordsmanDown, 
                      new ButtonData (ScaleButton (new Rect (500, 260, 50, 25)), "-"));
        labels.Add (Label.SwordsmanSquadCount, new LabelData (
            ScaleButton (new Rect (450, 245, 40, 40)),
            delegate {
            return GameState.UnitSquadCount [UnitType.Swordsman];
        }));
                      
        buttons.Add (Button.ArcherUp, 
                      new ButtonData (ScaleButton (new Rect (500, 350, 50, 25)), "+"));
        buttons.Add (Button.ArcherDown, 
                      new ButtonData (ScaleButton (new Rect (500, 380, 50, 25)), "-"));              
        labels.Add (Label.ArcherSquadCount, new LabelData (
            ScaleButton (new Rect (450, 365, 40, 40)),
            delegate {
            return GameState.UnitSquadCount [UnitType.Archer];
        }));
            
        buttons.Add (Button.MageUp, 
                      new ButtonData (ScaleButton (new Rect (500, 470, 50, 25)), "+"));
        buttons.Add (Button.MageDown, 
                      new ButtonData (ScaleButton (new Rect (500, 500, 50, 25)), "-"));
        labels.Add (Label.MageSquadCount, new LabelData (
            ScaleButton (new Rect (450, 485, 40, 40)),
            delegate {
            return GameState.UnitSquadCount [UnitType.Mage];
        }));
            
        buttons.Add (Button.LevelSelector,
                      new ButtonData (ScaleButton (new Rect (200, 650, 250, 60)), "Back to Level Selection"));
        buttons.Add (Button.NextLevel, 
                      new ButtonData (ScaleButton (new Rect (500, 650, 250, 60)), "Next Level"));
    }

    // TODO make this a general utility function
    // TODO add these to global game state
    float kScreenWidth = 1024;
    float kScreenHeight = 768;
    
    Rect ScaleButton (Rect button)
    {
        float widthRatio = Screen.width / kScreenWidth;
        float heightRatio = Screen.height / kScreenHeight;
        
        button.x *= widthRatio;
        button.width *= widthRatio;
        button.y *= heightRatio;
        button.height *= heightRatio;
        
        return button;
    }
}
