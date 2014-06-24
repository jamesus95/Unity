using UnityEngine;
using System.Collections;

public class HideLevelSelectButtons : MonoBehaviour {

	public GameObject mLevelSelectButtons;
	public GameObject mEraSelectButtons;
	// Use this for initialization
	void Start () {
		mLevelSelectButtons.SetActive(false);
		mEraSelectButtons.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
