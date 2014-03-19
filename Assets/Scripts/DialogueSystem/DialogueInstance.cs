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
        if (displayedText.Length != currentEntry.Content.Length)
        {
            string previous = displayedText;

            float chars = (Time.time - entryStartTime) * textSpeed;
            if (chars < currentEntry.Content.Length)
            {
                displayedText = currentEntry.Content.Substring(0, (int)chars);
            }
            else
            {
                displayedText = currentEntry.Content;
                return;
            }

            isTalking = (previous != displayedText);
            if (IsTalking)
            {
                if (!voiceOutput.isPlaying)
                {
                    voiceOutput.clip = currentEntry.Author.VoiceSample;
                    voiceOutput.pitch = UnityEngine.Random.Range(1.0f, 1.5f);
                    voiceOutput.Play();
                }
            }
        }
    }


    public void OnGUI(GUISkin guiSkin)
    {
        if (entries.Count > 0 && currentEntry != null)
        {
            bool fastforward = currentEntry.DisplayEntry(guiSkin, displayedText);
            
            if (displayedText.Length == currentEntry.Content.Length)
            {
                fastforward |= currentEntry.DisplayContinueButton(guiSkin.GetStyle("next"));

                if (fastforward)
                {
                    currentEntryIndex++;
                    LoadEntry();
                }
            }
            else if (fastforward)
            {
                displayedText = currentEntry.Content;
            }
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

            DialogueManager.DialogueComplete();

            return false;
        }

        currentEntry = (DialogueEntry)entries[currentEntryIndex];
        entryStartTime = Time.time;

        return true;
    }
}
