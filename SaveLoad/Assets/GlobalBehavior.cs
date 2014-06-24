using UnityEngine;
using System.Collections;

public class GlobalBehavior : MonoBehaviour {

    private const string path = "save.game";

	// Use this for initialization
	void Start () {
        Load();
	}
	
	// Update is called once per frame
	void Update () {
        Quit();
	}

    private void Quit()
    {
        if (Input.GetKeyDown("escape"))
        {
            Save();
            Application.Quit();
        }
    }

    private void Save()
    {
        Transform player = GameObject.Find("Baddie2").transform;
        float x = player.position.x;
        float y = player.position.y;
        string X = "" + x;
        string Y = "" + y;
        string[] save = {X, Y};
        System.IO.File.WriteAllLines(path, save);
    }

    private void Load()
    {
        try
        {
            string[] load = System.IO.File.ReadAllLines(path);
            GameObject.Find("Baddie2").transform.position = new Vector3(float.Parse(load[0]), float.Parse(load[1]), 0f);
        }
        catch (System.Exception e)
        {
        }
    }

}
