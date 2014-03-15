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

    private int currentDialogueIndex;

    void Start()
    {
        // Audio
        AudioSource audioOutput = GetComponent<AudioSource>();

        // Dialogues
        dialogues = new List<DialogueInstance>();

        /*1*/
        List<DialogueEntry> entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[0], "Hey Glovia, I have an AWESOME idea. What would happen if we destroyed the world in one hour. Wouldn't you join?"));
        entries.Add(new DialogueEntry(authors[1], "Indeed. What about Gustav? \nI know, i know . . ."));
        entries.Add(new DialogueEntry(authors[0], "Let's get this party started then! Shall we?"));
        entries.Add(new DialogueEntry(authors[1], "Ok!"));
        entries.Add(new DialogueEntry(authors[0], "I need to go on talking crap to test this thing though. Any suggestions yet? Also, I need to keep this a bit longer to see how it works, right?"));
        entries.Add(new DialogueEntry(authors[1], "Try writing long words like wunschpunsch, then. Did it work?"));
        entries.Add(new DialogueEntry(authors[0], "Yea, I guess. Anyway, enough for today."));
        entries.Add(new DialogueEntry(authors[1], "Cool, bye!"));

        dialogues.Add(new DialogueInstance(entries, audioOutput));

        /*2*/
        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[0], "I need to test it once more. You ready?"));
        entries.Add(new DialogueEntry(authors[1], "Sure. Something wrong? . . ."));
        entries.Add(new DialogueEntry(authors[0], "Not at all. I just wanted to make sure that it works properly when creating multiple dialogues. Also for long sentences which contains loads and loads of words .."));
        entries.Add(new DialogueEntry(authors[0], ".. and more weird stuff like this one. Guess it's alright. What do you say?"));
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

        if (GUI.Button(new Rect(Screen.width * 0.925f, Screen.height * 0.925f, Screen.width * 0.05f, Screen.height * 0.05f), ""))
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Game"];

            return;
        }

        if (currentDialogueIndex >= 0)
        {
            dialogues[currentDialogueIndex].OnGUI(GUI.skin);
        }
    }

    public void LoadDialogue(int index)
    {
        currentDialogueIndex = index;

        dialogues[currentDialogueIndex].StartDialogue();
    }

    void TriggerDialogue()
    {
        LoadDialogue(0);
    }
}