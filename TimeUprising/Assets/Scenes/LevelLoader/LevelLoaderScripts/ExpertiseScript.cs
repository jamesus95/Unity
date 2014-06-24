using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ExpertiseScript : MonoBehaviour {

	public GUIText GoldText;
	public GUIText HealTowerXP;
	public GUIText MeleeXP;
	public GUIText RangedXP;
	public GUIText SpecialXP;


	// Use this for initialization
	void Start () {
		//requires global tower levels
		GoldText.text = "";
		HealTowerXP.text = "";
		MeleeXP.text = "";
		RangedXP.text = "";
		SpecialXP.text = "";

	}
	

}
