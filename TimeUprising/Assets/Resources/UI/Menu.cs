using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public enum MenuButton
{
    NewGame,
    LoadGame,
    About
}

public class Menu : MonoBehaviour
{
    private Dictionary<MenuButton, Rect> mButtons;
    
    void OnGUI ()
    {
        //GUI.color = Color.clear;
        foreach (MenuButton button in mButtons.Keys) {
            if (GUI.Button (mButtons [button], ""))
                HandleClick (button);
        }
    }
    
    private void HandleClick (MenuButton button)
    {
        switch (button) {
        case(MenuButton.NewGame):
            Application.LoadLevel ("LevelLoader");
            break;
        case(MenuButton.LoadGame):
            break;
        case(MenuButton.About):
            Application.Quit ();
            break;
        }
    }
    
    void Awake ()
    {
        mButtons = new Dictionary<MenuButton, Rect> ();

        Rect buttonDimensions = new Rect (313, 310, 400, 95);
        mButtons.Add (MenuButton.NewGame, ScaleButton (buttonDimensions));

        buttonDimensions.y += 110; // vertical offset between buttons
        mButtons.Add (MenuButton.LoadGame, ScaleButton (buttonDimensions));

        buttonDimensions.y += 110;
        mButtons.Add (MenuButton.About, ScaleButton (buttonDimensions));
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
