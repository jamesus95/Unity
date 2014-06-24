using UnityEngine;
using System.Collections;

public class Progressbar : MonoBehaviour
{
    public SpriteRenderer Shape;

    public int MaxValue { 
        get { return mMaxValue; }
        set { 
            mMaxValue = value; 
            if (mMaxValue <= 0) 
                mMaxValue = 1; 
        }
    }

    public Color ForegroundColor;
    public Color BackgroundColor;
    public Color BorderColor;
    public float BorderWidth = 1f;
    
    public int Value { 
        get { return mCurrentValue; }
        set { UpdateValue (value); }
    }
    
    private int mMaxValue;
    private int mCurrentValue;
    private Rect mGuiBox;
    private Texture2D background;
    private Texture2D foreground;
    private Texture2D border;
    
    void Awake ()
    {
        Shape.enabled = false;
    }
    
    void Start ()
    {
        Bounds b = Shape.bounds;
        mGuiBox = new Rect (b.min.x, b.min.y, b.size.x, b.size.y);
        
        Vector3 topLeft = Camera.main.WorldToScreenPoint (new Vector3 (b.min.x, b.min.y, 0f));
        Vector3 bottomRight = Camera.main.WorldToScreenPoint (new Vector3 (b.max.x, b.max.y, 0f));
        
        mGuiBox.xMax = bottomRight.x + 1f;
        mGuiBox.yMin = Screen.height - bottomRight.y + 1f;
        
        mGuiBox.xMin = topLeft.x;
        mGuiBox.yMax = Screen.height - topLeft.y;
        
        background = new Texture2D (1, 1, TextureFormat.RGB24, false);
        background.SetPixel (0, 0, BackgroundColor);
        background.Apply ();
        
        foreground = new Texture2D (1, 1, TextureFormat.RGB24, false);
        foreground.SetPixel (0, 0, ForegroundColor);
        foreground.Apply ();
        
        border = new Texture2D (1, 1, TextureFormat.RGB24, false);
        border.SetPixel (0, 0, BorderColor);
        border.Apply ();
    }
    
    public void UpdateValue (int value)
    {
        mCurrentValue = value;
        if (mCurrentValue > MaxValue)
            mCurrentValue = MaxValue;
        if (mCurrentValue < 0)
            mCurrentValue = 0;
    }
    
    void OnGUI ()
    {
		if (Time.timeScale == 0)
			return; 

        GUI.BeginGroup (mGuiBox);
        {
            GUI.DrawTexture (new Rect (0, 0, mGuiBox.width, mGuiBox.height), 
                             border, ScaleMode.StretchToFill);
                             
            GUI.DrawTexture (new Rect (BorderWidth, BorderWidth, mGuiBox.width - BorderWidth * 2, mGuiBox.height - BorderWidth * 2), 
                             background, ScaleMode.StretchToFill);
                             
            GUI.DrawTexture (new Rect (BorderWidth, BorderWidth, 
                                       (mGuiBox.width - BorderWidth * 2) * mCurrentValue / MaxValue, 
                                       (mGuiBox.height) - BorderWidth * 2), 
                             foreground, ScaleMode.StretchToFill);
        }
        GUI.EndGroup ();
    }
}
