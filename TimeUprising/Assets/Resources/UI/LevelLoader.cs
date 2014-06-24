using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour 
{
	protected enum Button {
		TowerStore,
		Menu
	}
	
	List<Rect> levelButtons;
	Dictionary<Button, ButtonData> buttons;
	
	protected struct ButtonData
	{
		public Rect rect;
		public string text;
		
		public ButtonData(Rect r, string s) 
		{
			rect = r;
			text = s;
		}
	}
	
	private void HandleClick(Button button)
	{
		switch(button) 
		{
		case(Button.TowerStore):
			Application.LoadLevel("TowerStore");
			break;
		case(Button.Menu):
			Application.LoadLevel("Menu");
			break;
		}
	}
	
	private void LoadLevel(int level)
	{
		GameState.LoadLevel(level);
	}
	
	void OnGUI () 
	{
		foreach(Button button in buttons.Keys) {
			if (GUI.Button(buttons[button].rect, buttons[button].text))
				HandleClick(button);
		}
		
		for (int i = 0; i < levelButtons.Count; ++i)
			if (GUI.Button(levelButtons[i], i.ToString()))
				LoadLevel(i);
	}
	
	void Awake()
	{
		buttons = new Dictionary<Button, ButtonData>();

		Rect buttonRect = new Rect (313, 310, 400, 95);
		buttons.Add (Button.TowerStore, 
		              new ButtonData(ScaleButton(buttonRect), "Galactic Tower Store"));

		buttonRect.y += 110; // vertical offset between buttons
		buttons.Add (Button.Menu, 
		              new ButtonData(ScaleButton(buttonRect), "Back to Menu"));

		Rect levelButton = new Rect (233, 550, 75, 75);
		levelButtons = new List<Rect>();
		for (int i = 0; i < 5; ++i) {
			levelButton.x += 80; // offset between buttons
			levelButtons.Add (ScaleButton(levelButton));
		}
	}

	// TODO make this a general utility function
	// TODO add these to global game state
	float kScreenWidth = 1024;
	float kScreenHeight = 768;

	Rect ScaleButton(Rect button)
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
