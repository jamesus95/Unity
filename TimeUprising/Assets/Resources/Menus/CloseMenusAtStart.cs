using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloseMenusAtStart : MonoBehaviour
{
	public List<GameObject> mMenuList;
	// Use this for initialization
	void Start ()
	{
		for(int i = 0; i < mMenuList.Count; i++){
			mMenuList[i].SetActive(false);
		}
	}
}
