using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SpeakerState
{
    Normal,
    Nervous,
    Happy,
    Angry,
    Dying,
}

public enum SpeakerLocation
{
    Left,
    Right,
}

public class Speaker : MonoBehaviour 
{
    public SpeakerLocation Location;
    public string SpeakerName; // The name referenced by the dialogue data
    public string DisplayedName;

    public List<SpeakerState> SpeakerStates;
    public List<Sprite> Sprites;
    
    public void Activate (SpeakerState state)
    {
        mRenderer.sprite = GetSprite(state);
        mRenderer.enabled = true;
    }
    
    public void Deactivate ()
    {
        mRenderer.enabled = false;
    }
    
    public Sprite GetSprite (SpeakerState state)
    {
        if (mSprites.ContainsKey(state))
            return mSprites[state];
        else 
            return mSprites[SpeakerState.Normal];
    }    
    
    private Dictionary<SpeakerState, Sprite> mSprites;
    private SpriteRenderer mRenderer;
    //private Dictionary<SpeakerState, Sprite> mSprites;
    
    void Awake ()
    {
        mSprites = new Dictionary<SpeakerState, Sprite>();
        mSprites = new Dictionary<SpeakerState, Sprite>();
        
        for (int i = 0; i < SpeakerStates.Count; ++i) {
            mSprites.Add (SpeakerStates[i], Sprites[i]);
        }
        
        mRenderer = this.GetComponent<SpriteRenderer>();
        mRenderer.enabled = false;
    }
}
