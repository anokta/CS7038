using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Grouping;

public class DialogueManager : MonoBehaviour
{
    public GUISkin guiSkin;

    public float textSpeed = 40.0f;

    public Author[] authors;
    public List<DialogueInstance> dialogues;

    private static int currentDialogue;
    public static int CurrentDialogue { get { return currentDialogue; } set { currentDialogue = value; } }

    public static Action DialogueComplete;

    Timer waitTimer;
    public Texture2D[] nextButtonImages;

    void Start()
    {
        // Audio
        AudioSource audioOutput = GetComponent<AudioSource>();

        // Dialogues
        dialogues = new List<DialogueInstance>();

        /*Prologue*/
        List<DialogueEntry> entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[0], "In an increasingly fast-paced world, humanity has forgotten correct sanitary practices. Washing one’s hands is dismissed as slow and old fashioned, a waste of water."));
        entries.Add(new DialogueEntry(authors[0], "In the wake of poor hand hygiene, disease is thriving! The number of ill grows day by day, and no one seems to understand why."));
        entries.Add(new DialogueEntry(authors[0], "However! Two outstanding doctors, Dr. Handrew and Glovia Johnson, are on the case. To counteract the imminent spread of a viral epidemic, they decide to take matters into their own clean hands!"));
        entries.Add(new DialogueEntry(authors[0], "Their aim: to set a global example of good health, treating patients the way they ought to be treated, and thereby single-handedly save the world."));
        entries.Add(new DialogueEntry(authors[0], "Can you handle the task?"));
    
        dialogues.Add(new DialogueInstance(entries, audioOutput));

        /*Intro*/
        entries = new List<DialogueEntry>();
		entries.Add(new DialogueEntry(authors[1], "Hello, I am Professor Onionghost, and I am the embodiment of your conscience."));
        entries.Add(new DialogueEntry(authors[2], "You cannot be an embodiment of anything if you don’t have a physical form."));
        entries.Add(new DialogueEntry(authors[1], "That is correct. I am inside your head, and I’m here to guide you through this lab."));
        entries.Add(new DialogueEntry(authors[2], "What is the situation again?"));
        entries.Add(new DialogueEntry(authors[1], "This lab is being quarantined for the Handurian Flu: A highly dangerous and potent virus which attaches itself .."));
        entries.Add(new DialogueEntry(authors[1], ".. to human hands and infects individuals when in contact with more sensitive areas."));
        entries.Add(new DialogueEntry(authors[2], "..."));
        entries.Add(new DialogueEntry(authors[2], "When will people ever learn to clean their hands properly?"));

        dialogues.Add(new DialogueInstance(entries, audioOutput));

        /*Tutorial*/
        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[1], "Move in a direction by dragging your finger across the screen."));
        entries.Add(new DialogueEntry(authors[1], "Hold your finger to keep moving towards that direction."));

        dialogues.Add(new DialogueInstance(entries, audioOutput));

        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[1], "Clean your hands by walking to the sink!"));

        dialogues.Add(new DialogueInstance(entries, audioOutput));

        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[1], "What was that? We need to spend at least 20 seconds to clean our hands properly!"));
        entries.Add(new DialogueEntry(authors[1], "In game-time, however, we only have to wait for the washing bar to fill up.  Now try washing again, but this time hold your finger until our hands are clean."));

        dialogues.Add(new DialogueInstance(entries, audioOutput));
        
        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[1], "Now our hands are clean. Walk towards the patient to treat him."));

        dialogues.Add(new DialogueInstance(entries, audioOutput));

        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[3], "Thank you for saving me!"));
        entries.Add(new DialogueEntry(authors[2], "I’m only doing my duty, sir."));
        entries.Add(new DialogueEntry(authors[2], "Just a follow-up question.. Did you wash your hands after lunch today?"));
        entries.Add(new DialogueEntry(authors[3], "..."));
        entries.Add(new DialogueEntry(authors[3], "No, I forgot."));
        entries.Add(new DialogueEntry(authors[2], "Make sure you remember next time. The Handurian Flu is not a joke!"));
        entries.Add(new DialogueEntry(authors[3], "Will do!"));

        dialogues.Add(new DialogueInstance(entries, audioOutput));

        // Levels
        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[2], "What's next?"));
        entries.Add(new DialogueEntry(authors[1], "We haven't even started yet."));
        entries.Add(new DialogueEntry(authors[2], "..."));
        entries.Add(new DialogueEntry(authors[1], "..."));
        
        dialogues.Add(new DialogueInstance(entries, audioOutput));

        
        GroupManager.main.group["Dialogue"].Add(this);
        GroupManager.main.group["Dialogue"].Add(this, new GroupDelegator(null, TriggerDialogue, null));

        currentDialogue = -1;

        waitTimer = new Timer(0.6f, delegate()
        {
            guiSkin.GetStyle("next").normal.background = (guiSkin.GetStyle("next").normal.background == nextButtonImages[0]) ? nextButtonImages[1] : nextButtonImages[0];
        });
        waitTimer.repeating = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDialogue >= 0)
        {
            dialogues[currentDialogue].Update(textSpeed);
        }

        waitTimer.Update();
    }

	static readonly float bigSize = 0.1f;
	static readonly float relSize = 0.075f;
		
    void OnGUI()
    {
        GUI.skin = guiSkin;

        if (GUI.Button(new Rect(Screen.width * (1 - bigSize), Screen.width * relSize / 4.0f, Screen.width * relSize, Screen.width * relSize), GUIContent.none, guiSkin.GetStyle("skip")))
        {
            DialogueComplete();

            return;
        }

        if (currentDialogue >= 0)
        {
            dialogues[currentDialogue].OnGUI(GUI.skin);
        }
    }

    void TriggerDialogue()
    {
        currentDialogue++;

        currentDialogue = Mathf.Min(dialogues.Count - 1, currentDialogue);

        dialogues[currentDialogue].StartDialogue();

        waitTimer.Reset();
    }
}