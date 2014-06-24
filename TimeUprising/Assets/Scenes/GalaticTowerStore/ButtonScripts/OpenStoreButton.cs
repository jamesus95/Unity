using UnityEngine;
using System.Collections;

public class OpenStoreButton : ButtonBehaviour {

	public GameObject mStore;
	public GameObject mStoreFront;
	void OnMouseDown(){
		ChangeScreen();
		mStoreFront.SetActive(false);
		mStore.SetActive(true);
	}
}
