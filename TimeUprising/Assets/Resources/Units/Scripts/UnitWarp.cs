using UnityEngine;
using System.Collections;

public class UnitWarp : MonoBehaviour {
    
    public void DestroyThis()
    {
        Destroy (this.gameObject);
        Destroy (this);
    }
    
	// Use this for initialization
	void Start () {
        int sortingOrder = (int)(4 * (-this.transform.position.y + Camera.main.orthographicSize));
        GetComponent<SpriteRenderer> ().sortingOrder = (int)(sortingOrder);
        
        Invoke("DestroyThis", 1f);
	}
}
