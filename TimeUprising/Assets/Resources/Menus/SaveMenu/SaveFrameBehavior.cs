using UnityEngine;
using System.Collections;

public class SaveFrameBehavior : MonoBehaviour {

	public string stringToEdit = "";
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width/2-200, Screen.height/2, 400 ,100));
		stringToEdit =  GUILayout.TextField(stringToEdit );
		GUILayout.EndArea();
	}
}
