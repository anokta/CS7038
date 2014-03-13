using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Grouping;

public class DialogueInstance
{
    AudioSource voiceOutput;

    [SerializeField]
    List<DialogueEntry> entries;

    DialogueEntry currentEntry;
    int currentEntryIndex;

    float entryStartTime;

    string displayedText;


    private bool isTalking;
    public bool IsTalking
    {
        get { return isTalking; }
    }

    public DialogueInstance(List<DialogueEntry> entries, AudioSource voiceOutput)
    {
        this.entries = entries;
        this.voiceOutput = voiceOutput;
    }

    public void Update(float textSpeed)
    {
        if (currentEntry == null || displayedText.Length == currentEntry.Content.Length)
        {
            if (Pressed())
            {
                if (currentEntry != null || currentEntryIndex != 0)
                    currentEntryIndex++;
                LoadEntry();
            }
            return;
        }

        string previous = displayedText;

        displayedText = currentEntry.Content;
        if (Pressed()) return;

        float chars = (Time.time - entryStartTime) * textSpeed;
        if (chars < displayedText.Length)
        {
            displayedText = displayedText.Substring(0, (int)chars);
        }

        isTalking = (previous != displayedText);
        if (IsTalking)
        {
            if (!voiceOutput.isPlaying)
            {
                voiceOutput.clip = currentEntry.Author.VoiceSample;
                voiceOutput.pitch = UnityEngine.Random.Range(1.25f, 1.75f);//voiceRange.x, voiceRange.y);
                voiceOutput.Play();
            }
        }
    }


    public void OnGUI(GUISkin guiSkin)
    {
        if (entries.Count > 0 && currentEntry != null)
        {
            currentEntry.Display(guiSkin, displayedText);
        }
    }

    public void StartDialogue()
    {
        displayedText = "";
        
        currentEntryIndex = 0;
        LoadEntry();
    }

    bool LoadEntry()
    {
        displayedText = "";

        if (currentEntryIndex >= entries.Count)
        {
            currentEntry = null;

            GroupManager.main.activeGroup = GroupManager.main.group["Game"];

            return false;
        }

        currentEntry = (DialogueEntry)entries[currentEntryIndex];
        entryStartTime = Time.time;

        return true;
    }

    private static bool Pressed()
    {
        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return);
    }
}
