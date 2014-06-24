using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public static class EnumUtil 
{
    public static IEnumerable<T> GetValues<T>() 
    {
        return (T[])Enum.GetValues(typeof(T));
    }
    
    public static T FromString<T> (string value)
    {
        return (T)Enum.Parse (typeof(T), value);
    }
}

public static class Utility
{
    public static void Perspectivize(GameObject o)
    {
        int sortingOrder = (int)(4 * (-o.transform.position.y + Camera.main.orthographicSize));
        o.GetComponent<SpriteRenderer> ().sortingOrder = (int)(sortingOrder);
    }
    
    /// <summary>
    /// Breaks a word into multiple lines so that it fits into the given GUIText.
    /// </summary>
    /// <returns>The wrapped string.</returns>
    /// <param name="text">Text. The text to wrap.</param>
    /// <param name="guiText">GUI text. The guiText to wordwrap on.</param>
    /// <param name="maxTextWidth">Max text width. The width of the guiTextBox.</param>
    public static string WordWrappedString (string text, GUIText guiText, float maxTextWidth)
    {
        char[] delim = { ' ' };
        string[] words = text.Split(delim); //Split the string into seperate words
        
        if (words.Length == 0) // empty string
            return "";
        
        string result = "";
        
        result = words[0];
        
        for (var index = 1; index < words.Length; ++index) {
            string word = words[index].Trim();
            
            result += " " + word;
            guiText.text = result;
            
            Rect TextSize = guiText.GetScreenRect();
            
            if (TextSize.width > maxTextWidth)
            {
                result = result.Substring(0, result.Length - (word.Length));
                result += "\n" + word;
            }
        }
        
        return result;
    }
}