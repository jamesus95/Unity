using UnityEngine;
using System.Collections;

public class KingSpawner : MonoBehaviour 
{
    public SpriteRenderer KingImage;

	void Start () 
    {
        this.KingImage.enabled = false;
	}
}
