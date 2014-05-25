using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Grouping;

public class DialogueManager : MonoBehaviour
{
   // GUISkin guiSkin;

    public float textSpeed = 40.0f;

    public Author[] authors;
    //public List<DialogueInstance> dialogues;

	private Dictionary<string, Author> authorDict;

    //private static int currentDialogue;
    //public static int CurrentDialogue { get { return currentDialogue; } set { currentDialogue = value; } }
  //  public static int[] dialogueIndex = { 1, 6, 7, 8, 9, 11, 13, 14 }; 

    public static Action DialogueComplete;

    Timer waitTimer;
    public Texture2D[] nextButtonImages;

	DialogueMap defMap;

	public Author GetAuthor(string author) {
		return authorDict[author];
	}

	public static DialogueManager instance {
		get; private set;
	}

	public DialogueMap defaultMap {
		get { return defMap;
		}
	}

	void SetData() {
		authorDict = new Dictionary<string, Author>();
		foreach (var author in authors) {
			authorDict[author.Key] = author;
		}
		defMap = new DialogueMap(this);
	}

	public static DialogueInstance ActiveInstance
	{
		get; private set;
	}

	public static void ActivateDialogue(DialogueInstance instance)
	{
		ActiveInstance = instance;
		instance.StartDialogue();
		GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
	}

    void Start()
    {
		instance = this;
		SetData();
        // Audio
        AudioSource audioOutput = GetComponent<AudioSource>();

        // Dialogues
        //dialogues = new List<DialogueInstance>();

        /*Prologue*/
        List<DialogueEntry> entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[0], "In an increasingly fast-paced world, humanity has forgotten correct sanitary practices. Washing one’s hands is dismissed as slow and old fashioned, a waste of water."));
        entries.Add(new DialogueEntry(authors[0], "In the wake of poor hand hygiene, disease is thriving! The number of ill grows day by day, and no one seems to understand why."));
        entries.Add(new DialogueEntry(authors[0], "However! Two outstanding doctors, Dr. Handrew and Glovia Johnson, are on the case. To counteract the imminent spread of a viral epidemic, they decide to take matters into their own clean hands!"));
        entries.Add(new DialogueEntry(authors[0], "Their aim: to set a global example of good health, treating patients the way they ought to be treated, and thereby single-handedly save the world."));
        entries.Add(new DialogueEntry(authors[0], "Can you handle the task?"));
    
        //dialogues.Add(new DialogueInstance(entries, audioOutput));
		defMap.data["Intro1"] = new DialogueInstance(entries, audioOutput);

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

        //dialogues.Add(new DialogueInstance(entries, audioOutput));
		defMap.data["Intro2"] = new DialogueInstance(entries, audioOutput);

        /*Tutorial*/
        entries = new List<DialogueEntry>();
		if (SystemInfo.deviceType == DeviceType.Handheld) {
			entries.Add(new DialogueEntry(authors[1], "Move in a direction by dragging your finger anywhere across the screen."));
			entries.Add(new DialogueEntry(authors[1], "Hold your finger to keep moving towards that direction."));
		} else {
			entries.Add(new DialogueEntry(authors[1], "Move around by pressing the arrow keys."));
		}
		defMap.data["Input"] = new DialogueInstance(entries, audioOutput);
        //dialogues.Add(new DialogueInstance(entries, audioOutput));

        /*entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[1], "Clean your hands by walking to the sink!"));

        //dialogues.Add(new DialogueInstance(entries, audioOutput));

        entries = new List<DialogueEntry>();
		entries.Add(new DialogueEntry(authors[1], "What was that? You need to spend at least 20 seconds to clean your hands properly!"));
		entries.Add(new DialogueEntry(authors[1], "In game-time, however, we only have to wait for the washing bar to fill up.  Now try washing again, but this time wait until your hands are clean."));

        dialogues.Add(new DialogueInstance(entries, audioOutput));
        
        entries = new List<DialogueEntry>();
		entries.Add(new DialogueEntry(authors[1], "Now your hands are clean. Walk towards the patient to treat him."));

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

        // File cabinet hint
        entries = new List<DialogueEntry>();
		entries.Add(new DialogueEntry(authors[1], "Push a file cabinet or a plant by walking against it. But beware! Your hands will get dirty .."));
		entries.Add(new DialogueEntry(authors[1], ".. make sure you clean your hands before treating the patient!"));

        dialogues.Add(new DialogueInstance(entries, audioOutput));

        // Lasers
        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[1], "Lasers are dangerous! Don’t touch them, or we’ll die!"));
        entries.Add(new DialogueEntry(authors[1], "Anything can block its beam, but some might as well catch on fire."));
        entries.Add(new DialogueEntry(authors[2], "And why are active lethal laser emitters lying around so casually?"));
		entries.Add(new DialogueEntry(authors[1], "That is a good question, one that I do not know the answer to."));
		entries.Add(new DialogueEntry(authors[2], "..."));
		//entries.Add(new DialogueEntry(authors[1], "Exactly!"));

        dialogues.Add(new DialogueInstance(entries, audioOutput));
        
        // Mirrors
        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[1], "Mirrors can deflect lasers. Push on a mirror to change its direction."));
        entries.Add(new DialogueEntry(authors[1], "Make sure you don’t deflect the beam towards us though!"));

        dialogues.Add(new DialogueInstance(entries, audioOutput));

        // Doors & Gates
        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[2], "Doors! My sworn enemy!"));
		entries.Add(new DialogueEntry(authors[1], "Don’t worry, doctor. We can pass through them easily, but make sure you wash your hands afterwards .."));
        entries.Add(new DialogueEntry(authors[1], ".. we don’t know who touched those handles before!"));

        dialogues.Add(new DialogueInstance(entries, audioOutput));

        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[1], "Levers can make the impossible, possible."));

        dialogues.Add(new DialogueInstance(entries, audioOutput));
        
        // Trolley
        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[1], "A trolley acts like a file cabinet or a plant, except it cannot be stopped until it hits something."));

        dialogues.Add(new DialogueInstance(entries, audioOutput));

        entries = new List<DialogueEntry>();
		entries.Add(new DialogueEntry(authors[1], "Indeed, you are not the only one here. They can interact with the objects they hit."));

        dialogues.Add(new DialogueInstance(entries, audioOutput));

        // Explosive
        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[1], "Those chemical crates are safe to push around, but beware: They explode when struck by a laser!"));
        entries.Add(new DialogueEntry(authors[1], "An explosion can please everything, and everyone."));
        entries.Add(new DialogueEntry(authors[2], "Why are you encouraging the destruction of chemical supplies again?"));
        entries.Add(new DialogueEntry(authors[1], "It will be worth it if it means you can save a life!"));

        dialogues.Add(new DialogueInstance(entries, audioOutput));

        // Last
        entries = new List<DialogueEntry>();
        entries.Add(new DialogueEntry(authors[2], "What's next?"));
        entries.Add(new DialogueEntry(authors[1], "We haven't even started yet."));
        entries.Add(new DialogueEntry(authors[2], "..."));
        entries.Add(new DialogueEntry(authors[1], "..."));
        
        dialogues.Add(new DialogueInstance(entries, audioOutput));
*/
        GroupManager.main.group["Dialogue"].Add(this);
        GroupManager.main.group["Dialogue"].Add(this, new GroupDelegator(null, TriggerDialogue, null));
        GroupManager.main.group["Main Menu"].Add(this, new GroupDelegator(null, GameMenu, null));

//        currentDialogue = -1;

        waitTimer = new Timer(0.6f, delegate()
        {
			GUIManager.Style.next.normal.background = (GUIManager.Style.next.normal.background == nextButtonImages[0]) ? nextButtonImages[1] : nextButtonImages[0];
           // guiSkin.GetStyle("next").normal.background = (guiSkin.GetStyle("next").normal.background == nextButtonImages[0]) ? nextButtonImages[1] : nextButtonImages[0];
        });
        waitTimer.repeating = true;

//        guiSkin = GUIManager.GetSkin();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DialogueComplete();

            return;
        }

       /* if (currentDialogue >= 0)
        {
            dialogues[currentDialogue].Update(textSpeed);
        }*/
		if (ActiveInstance != null) {
			ActiveInstance.Update(textSpeed);
		}

        waitTimer.Update();
    }
		
    void OnGUI()
    {
        //GUI.skin = guiSkin;

        if (GUI.Button(new Rect(Screen.width - GUIManager.OffsetX() - GUIManager.ButtonSize(), GUIManager.OffsetY(), GUIManager.ButtonSize(), GUIManager.ButtonSize()), GUIContent.none, GUIManager.Style.skip))
        {
            DialogueComplete();

            return;
        }

		if (ActiveInstance != null) {
			ActiveInstance.OnGUI(GUIManager.skin);
		}
        //if (currentDialogue >= 0)
       // {
       //     dialogues[currentDialogue].OnGUI(GUIManager.skin);
       // }
    }

    void TriggerDialogue()
    {
        //currentDialogue++;

        //currentDialogue = Mathf.Min(dialogues.Count - 1, currentDialogue);

        //dialogues[currentDialogue].StartDialogue();

        //waitTimer.Reset();
    }

    void GameMenu()
    {
      //  currentDialogue = -1;
    }
}