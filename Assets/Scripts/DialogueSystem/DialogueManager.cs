using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Grouping;

public class DialogueManager : MonoBehaviour
{
    public GUISkin guiSkin;

    public float textSpeed = 40.0f;

    public Author[] authors;
    public List<DialogueInstance> dialogues;

    private static int currentDialogueIndex;
    public static int CurrentDialogue { get { return currentDialogueIndex; } set { currentDialogueIndex = value; } }

    public static GroupManager.Group nextState;

    void Start()
    {
        // Audio
        AudioSource audioOutput = GetComponent<AudioSource>();

        // Dialogues
        dialogues = new List<DialogueInstance>();

        /*Intro*/
        List<DialogueEntry> entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[0], "In an increasingly fast-paced world, humanity has forgotten correct sanitary practices. Washing one’s hands is dismissed as slow and old fashioned, a waste of water."));
        entries.Add(new DialogueEntry(authors[0], "In the wake of poor hand hygiene, disease is thriving! The number of ill grows day by day, and no one seems to understand why."));
        entries.Add(new DialogueEntry(authors[0], "However! Two outstanding doctors, Dr. Handrew and Glovia Johnson, are on the case. To counteract the imminent spread of a viral epidemic, they decide to take matters into their own clean hands!"));
        entries.Add(new DialogueEntry(authors[0], "Their aim: to set a global example of good health, treating patients the way they ought to be treated, and thereby single-handedly save the world."));
        entries.Add(new DialogueEntry(authors[0], "Can you handle the task?"));
    
        dialogues.Add(new DialogueInstance(entries, audioOutput));

        /*1*/
        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[1], "Hello, I am Dr. Teddy, and I am the embodiment of your conscience."));
        entries.Add(new DialogueEntry(authors[2], "You cannot be an embodiment of anything if you don’t have a physical form."));
        entries.Add(new DialogueEntry(authors[1], "That is correct. I am inside your head, and I’m here to guide you through this lab."));
        entries.Add(new DialogueEntry(authors[2], "What is the situation again?"));
        entries.Add(new DialogueEntry(authors[1], "This lab is being quarantined for the Handurian Flu: A highly dangerous and potent virus which attaches itself .."));
        entries.Add(new DialogueEntry(authors[1], ".. to human hands and infects individuals when in contact with more sensitive areas."));
        entries.Add(new DialogueEntry(authors[2], "..."));
        entries.Add(new DialogueEntry(authors[2], "When will people ever learn to clean their hands properly?"));

        dialogues.Add(new DialogueInstance(entries, audioOutput));

        /*2*/
        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[1], "Hey Glovia, I have an AWESOME idea. What would happen if we destroyed the world in one hour. Wouldn't you join?"));
        entries.Add(new DialogueEntry(authors[2], "Indeed. What about Gustav? \nI know, i know . . ."));
        entries.Add(new DialogueEntry(authors[1], "Let's get this party started then! Shall we?"));
        entries.Add(new DialogueEntry(authors[2], "Ok!"));
        entries.Add(new DialogueEntry(authors[1], "I need to go on talking crap to test this thing though. Any suggestions yet? Also, I need to keep this a bit longer to see how it works, right?"));
        entries.Add(new DialogueEntry(authors[2], "Try writing long words like wunschpunsch, then. Did it work?"));
        entries.Add(new DialogueEntry(authors[1], "Yea, I guess. Anyway, enough for today."));
        entries.Add(new DialogueEntry(authors[2], "Cool, bye!"));

        dialogues.Add(new DialogueInstance(entries, audioOutput));

        /*3*/
        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[1], "I need to test it once more. You ready?"));
        entries.Add(new DialogueEntry(authors[2], "Sure. Something wrong? . . ."));
        entries.Add(new DialogueEntry(authors[1], "Not at all. I just wanted to make sure that it works properly when creating multiple dialogues. Also for long sentences which contains loads and loads of words .."));
        entries.Add(new DialogueEntry(authors[2], ".. and more weird stuff like this one. Guess it's alright. What do you say?"));
        entries.Add(new DialogueEntry(authors[1], "Cool story, bro!"));

        dialogues.Add(new DialogueInstance(entries, audioOutput));

        currentDialogueIndex = -1;
        
        GroupManager.main.group["Dialogue"].Add(this);
        GroupManager.main.group["Dialogue"].Add(this, new GroupDelegator(null, TriggerDialogue, null));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            LoadDialogue((currentDialogueIndex + 1) % dialogues.Count);
        }

        if (currentDialogueIndex >= 0)
        {
            dialogues[currentDialogueIndex].Update(textSpeed);
        }
    }


    void OnGUI()
    {
        GUI.skin = guiSkin;

        if (GUI.Button(new Rect(Screen.width * 0.925f, Screen.height * 0.925f, Screen.width * 0.05f, Screen.height * 0.05f), GUIContent.none, guiSkin.GetStyle("skip")))
        {
            GroupManager.main.activeGroup = nextState;

            return;
        }

        if (currentDialogueIndex >= 0)
        {
            dialogues[currentDialogueIndex].OnGUI(GUI.skin);
        }
    }

    public void LoadDialogue(int index)
    {
        //currentDialogueIndex = index;

        dialogues[currentDialogueIndex].StartDialogue();
    }

    void TriggerDialogue()
    {
        LoadDialogue(currentDialogueIndex);
    }
}