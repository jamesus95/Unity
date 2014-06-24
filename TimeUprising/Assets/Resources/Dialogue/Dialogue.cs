using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// who is saying what, and where
public struct Message
{
    public float Duration;
    public string Who;
    public string Text;
    public SpeakerLocation Location;
    public SpeakerState State;
}

public class Dialogue
{   
    public bool IsEnabled = true;
    private const float kLetterDisplayTime = .05f;
    public bool IsDone = false;
    Queue<Message> mMessages;
    
    public Dialogue ()
    {
        mMessages = new Queue<Message> ();
        
        // dummy object that is removed on the first call to AdvanceMessage
        mMessages.Enqueue (new Message ()); 
        mDialogueTimer = 0;
    }
    
    /// <summary>
    /// Advances the dialogue
    /// </summary>
    /// <returns><c>false</c>, if there are no more messages in the dialogue, 
    /// <c>true</c>, if the message was successfully updated.</returns>
    /// <param name="deltaTime">Delta time. The time since the last AdvanceMessage call.</param>
    /// <param name="message">Message. Returned containing information on how to display the message.</param>
    public bool AdvanceMessage (float deltaTime, out Message message)
    {
        if (IsDone) {
            message = new Message ();
            return false;
        }
        
        mDialogueTimer += deltaTime;
        
        // The current message is finished, advance to the next one
        if (mDialogueTimer > mTimeUntilNextMessage) { 
            mMessages.Dequeue (); // the current message is finished
            
            if (mMessages.Count > 0) {
                SetupNextMessage ();
            } else {
                IsDone = true;
                message = new Message ();
                return false;
            }
        }
                
        while (AdvanceLetterAnimation (deltaTime)) {};
        
        message = mCurrentMessage;
        return true;
    }
    
    private Message mCurrentMessage; // information about who the current speaker and what is being said
    private float mDialogueTimer; // the total duration this dialogue has been running
    private float mTimeUntilNextMessage = 0; // the duration of the current message
    private string mCurrentText; // the current text being manipulated
    private int mStringLength; // used to display the message one character at a time
    private float mPreviousLetterTime;
    
    private void SetupNextMessage ()
    {
        mCurrentMessage = mMessages.Peek ();
        mTimeUntilNextMessage = mCurrentMessage.Duration;
        mCurrentText = mCurrentMessage.Text;
        mStringLength = 1;
        mDialogueTimer = 0;
        mPreviousLetterTime = -kLetterDisplayTime;
    }
    
    private bool AdvanceLetterAnimation (float deltaTime)
    {
        if (mCurrentText == null)
            return false;
            
        if (mStringLength >= mCurrentText.Length || mDialogueTimer - mPreviousLetterTime < kLetterDisplayTime) 
            return false;
        
        mPreviousLetterTime += kLetterDisplayTime;
        mStringLength++;
        
        mCurrentMessage.Text = mCurrentText.Substring (0, mStringLength);
        return true;
    }
        
    // when, how, who, where, what
    // ex. for <5> seconds, <Nervous> <King> (<Left> side), says <"Hello world!">, 
    //     for <3.2> seconds, <Normal> <Peasant> (<Right> side), says <"....blargh.">
    public void AddMessage (float duration, SpeakerState state, string speaker, SpeakerLocation location, string text)
    {
        Message message = new Message ();
        message.Duration = duration;
        message.Who = speaker;
        message.Text = text;
        message.Location = location;
        message.State = state;
        
        mMessages.Enqueue (message);
    }
    
    public Dialogue Clone()
    {
        Dialogue dialogue = new Dialogue();
        
        foreach (Message m in mMessages) {
            if (m.Duration == 0)
                continue;
            dialogue.AddMessage(m.Duration, m.State, m.Who, m.Location, m.Text);
        }

        return dialogue;
    }
}
