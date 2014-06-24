using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public enum DialogueType {
    Realtime, // played when the game is paused
    Standard, // triggered via events
    Chatter,  // selects randomly from the available chatter dialogue
    Warning,  // high priority messages. these are played immediately
}

public class DialogueManager : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////////////////////////
    // Inspector Presets
    ///////////////////////////////////////////////////////////////////////////////////
    
    public List<SpeakerLocation> SpeakerLocations;
    public List<GUIText> NameTextBoxes;
    public List<GUIText> DialogueTextBoxes;
    
    public List<Speaker> Speakers;
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Public
    ///////////////////////////////////////////////////////////////////////////////////
    
    // Queues the related dialogue for play
    public void TriggerDialogue(string trigger)
    {
        Dictionary<string, Dialogue> triggers = mTriggers[DialogueType.Standard];
        if (triggers.ContainsKey(trigger) && triggers[trigger].IsEnabled) {
            mDialogueQueue[DialogueType.Standard].Enqueue(triggers[trigger].Clone());
            triggers[trigger].IsEnabled = false; // triggers once before requiring re-enabling
        }
    }
    
    // Allows the dialogue to be re-queued by calling TriggerDialogue
    public void ReenableTrigger(string trigger)
    {
        Dictionary<string, Dialogue> triggers = mTriggers[DialogueType.Standard];
        if (triggers.ContainsKey(trigger)) {
            triggers[trigger].IsEnabled = true; 
        }
    }
    
    public void TriggerWarning(string trigger)
    {
        Dictionary<string, Dialogue> triggers = mTriggers[DialogueType.Warning];
        if (triggers.ContainsKey(trigger) && triggers[trigger].IsEnabled) {
            mDialogueQueue[DialogueType.Warning].Enqueue(triggers[trigger].Clone());
            triggers[trigger].IsEnabled = false;
        }
    }
    
    public void TriggerRealtimeDialogue(string trigger)
    {
        Dictionary<string, Dialogue> triggers = mTriggers[DialogueType.Realtime];
        if (triggers.ContainsKey(trigger) && triggers[trigger].IsEnabled) {
            mDialogueQueue[DialogueType.Realtime].Clear(); // only one real-time trigger at a time
            mDialogueQueue[DialogueType.Realtime].Enqueue(triggers[trigger].Clone());
        }
    }
    
    public void TriggerChatter()
    {
    
    }

    ///////////////////////////////////////////////////////////////////////////////////
    // Private
    ///////////////////////////////////////////////////////////////////////////////////

    private Dictionary<SpeakerLocation, GUIText> mTextBoxes;
    private Dictionary<SpeakerLocation, float> mTextWidth;
    private Dictionary<SpeakerLocation, GUIText> mNameBoxes;

    private Dictionary<string, Speaker> mSpeakers;
    
    private Dictionary<DialogueType, Dictionary<string, Dialogue>> mTriggers;    
    private Dictionary<DialogueType, Queue<Dialogue>> mDialogueQueue;
    
    private DialogueType mDialogueType;
    private Dialogue mDialogue;
    
    private bool mIsIdle; // true if there are no messages to display
    private float mRealtimeStamp;
  
    // See src/dialogue_1.txt for formatting the file
    private void LoadDialogueFromFile (string filepath)
    {
        StreamReader file = new StreamReader (filepath);
        char[] delim = { ' ', ',' };
        
        while (!file.EndOfStream) {
            string line = file.ReadLine ();
            
            //string [] values = line.Split (delim, StringSplitOptions.RemoveEmptyEntries);
            string [] values = line.Split(delim, 5, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length == 0 || values[0]== "#") // ignore blank lines and comments
                continue;
            
            if (values[0].Contains(">>>")) { // start trigger
                string trigger = values[1];
                DialogueType type = EnumUtil.FromString<DialogueType>(values[2]);
                
                Dialogue dialogue = GetMessagesFromFile(file);
                mTriggers[type].Add(trigger, dialogue);
            }
        }
        
        file.Close ();
    }
    
    private Dialogue GetMessagesFromFile(StreamReader file)
    {
        Dialogue dialogue = new Dialogue();
        
        char[] delim = { ' ', ',' };
        
        while (true) {
            string line = file.ReadLine ();
            string [] values = line.Split(delim, 6, StringSplitOptions.RemoveEmptyEntries);
            
            if (values[0].Contains ("<<<")) // end trigger
                break;
            
            // read the message
            float duration = float.Parse(values[0]);
            SpeakerState state = EnumUtil.FromString<SpeakerState>(values[1]);
            string speaker = values[2];
            SpeakerLocation location = EnumUtil.FromString<SpeakerLocation>(values[3]);
            // ignore the literal "---"
            string message = values[5];
            
            dialogue.AddMessage(duration, state, speaker, location, message);
        }
        
        return dialogue;
    }
    
    // Resets the GUI and re-enables the relevant portions
    // Updates the dialogue text and name box
    private void DisplayMessage(Message message)
    {
        ResetGui();
        
        string speakerKey = message.Who + message.Location.ToString();
        
        Speaker speaker;
        if (! mSpeakers.ContainsKey(speakerKey))
            speaker = mSpeakers["None" + message.Location.ToString()];
        else 
            speaker = mSpeakers[speakerKey];
            
        speaker.Activate(message.State);
        
        SpeakerLocation location = message.Location;
        
        string messageText = Utility.WordWrappedString(message.Text, mTextBoxes[location], mTextWidth[location]);
        
        mTextBoxes[location].enabled = true;
        mTextBoxes[location].text = messageText;
        
        mNameBoxes[location].enabled = true;
        mNameBoxes[location].text = speaker.DisplayedName;
    }
    
    // Hides all of the Dialogue UI elements
    // DisplayMessage is responsible for enabling the correct ones
    private void ResetGui()
    {            
        foreach (SpeakerLocation l in mTextBoxes.Keys)
            mTextBoxes[l].enabled = false;
        
        foreach (SpeakerLocation l in mNameBoxes.Keys)
            mNameBoxes[l].enabled = false;
        
        foreach (string s in mSpeakers.Keys)
            mSpeakers[s].Deactivate();
    }
    
    private void UpdateDialogueQueue(DialogueType type)
    {            
        if (mDialogueQueue[type].Count == 0) // no messages to display
            return;
            
        if (type != mDialogueType) { // a different type of dialogue needs to be                
            mDialogueType = type;
            mIsIdle = true;
        }
        
        if (mIsIdle) {
            mDialogue = mDialogueQueue[mDialogueType].Peek();
            mIsIdle = false;
        }
        
        float deltaTime;
        if (type == DialogueType.Realtime)
            deltaTime = Time.realtimeSinceStartup - mRealtimeStamp;
        else
            deltaTime = Time.deltaTime;
        
        Message message;
        bool isValid = mDialogue.AdvanceMessage(deltaTime, out message);
        
        if (isValid) {
            DisplayMessage(message);
            mIsIdle = false;
        } else { // indicate that the current dialogue is complete
            mDialogueQueue[mDialogueType].Dequeue();
            mIsIdle = true;
        }
    }
    
    ///////////////////////////////////////////////////////////////////////////////////
    // Unity Overrides
    ///////////////////////////////////////////////////////////////////////////////////

    void Awake ()
    {
        mDialogueQueue = new Dictionary<DialogueType, Queue<Dialogue>>();
        foreach (DialogueType d in EnumUtil.GetValues<DialogueType>())
            mDialogueQueue[d] = new Queue<Dialogue>();
        
        mTriggers = new Dictionary<DialogueType, Dictionary<string, Dialogue>>();
        foreach (DialogueType d in EnumUtil.GetValues<DialogueType>())
            mTriggers[d] = new Dictionary<string, Dialogue>();
    
        mTextBoxes = new Dictionary<SpeakerLocation, GUIText>();
        mNameBoxes = new Dictionary<SpeakerLocation, GUIText>();
        mTextWidth = new Dictionary<SpeakerLocation, float>();
        
        for (int i = 0; i < SpeakerLocations.Count; ++i) {
            SpeakerLocation location = SpeakerLocations[i];
            
            mTextBoxes.Add (location, DialogueTextBoxes[i]);
            mTextWidth[location] = mTextBoxes[location].GetScreenRect ().width;
            mTextBoxes[location].text = "";
            
            mNameBoxes.Add (location, NameTextBoxes[i]);
            mNameBoxes[location].text = "";
        }
        
        mSpeakers = new Dictionary<string, Speaker>();
        
        for (int i = 0; i < Speakers.Count; ++i) {
            string speakerKey = Speakers[i].SpeakerName + Speakers[i].Location.ToString();
            mSpeakers.Add (speakerKey, Speakers[i]);
        }
        
        string dialoguePath = "Data/IngameDialogue/dialogue_" + GameState.GameEra.ToString() + ".txt";
        
        LoadDialogueFromFile(dialoguePath);
        this.TriggerDialogue("ArcherMage");
        this.TriggerDialogue("Tutorial");
        
        mIsIdle = true;
        mDialogueType = DialogueType.Standard;
        mRealtimeStamp = Time.realtimeSinceStartup;
        TriggerRealtimeDialogue("Pause");
        //mDialogue = mDialogueQueue[DialogueType.Standard].Dequeue();
    }
    
    void Update ()
    {
        UpdateDialogueQueue(mDialogueType);
        
        if (Time.timeScale == 0) {// game is paused
            UpdateDialogueQueue(DialogueType.Realtime);
        } else if (mDialogueQueue[DialogueType.Warning].Count != 0) {
            UpdateDialogueQueue(DialogueType.Warning);
        } else {
            UpdateDialogueQueue(DialogueType.Standard);
        }
        
        mRealtimeStamp = Time.realtimeSinceStartup;
    }
}
