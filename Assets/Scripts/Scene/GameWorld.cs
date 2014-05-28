using UnityEngine;
using Grouping;

public class GameWorld : MonoBehaviour
{
    public enum LevelOverReason
    {
       Undefined, Squashed,
        Success, PatientInfected, PlayerInfected, LaserKilledPlayer, LaserKilledPatient, ExplosionKilledPlayer, ExplosionKilledPatient
    }

    public static LevelOverReason levelOverReason;

    public static bool success
    {
        get { return levelOverReason == LevelOverReason.Success; }
       set { levelOverReason = value ? LevelOverReason.Success : LevelOverReason.Undefined; }
    }

	//public static bool 

	//static bool LockedReason { get; set; }
  /*  static int _score;
    public static int score
    {
        get { return _score; }
        set
        {
            _score = value;
            Debug.Log("Current score: " + _score);
        }
    }*/

    private static bool _dialogueOff;
    public static bool dialogueOff
    {
        get { return _dialogueOff; }
        set
        {
            //Debug.Log("DialogueOff was set to " + value + " from " + _dialogueOff);
            _dialogueOff = value;

        }
    }

    // Use this for initialization
    void Start()
    {
        //Application.LoadLevel("MainMenu");

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AudioListener.volume = PlayerPrefs.GetFloat("Audio Volume", 1.0f);

        GroupManager.main.group["Intro"].Add(this, new GroupDelegator(null, LevelIntro, null));
        GroupManager.main.group["Running"].Add(this);
        GroupManager.main.group["Level Start"].Add(this, new GroupDelegator(null, LevelStart, null));
        GroupManager.main.group["Level Over"].Add(this, new GroupDelegator(null, LevelOver, null));
        GroupManager.main.group["Level Select"].Add(this, new GroupDelegator(null, delegate() { success = true; }, null));

        dialogueOff = false;

        CheatManager.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        CheatManager.Instance.Update();

        var patients = FindObjectsOfType<Patient>();
        var isOver = true;
        foreach (var patient in patients)
        {
            isOver &= patient.GetComponent<Patient>().IsTreated();
        }

        if (!success || isOver)
        {
            if (success && LevelManager.instance.settings.outro != null)
            {
                //GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
				if (!LevelManager.instance.settings.HasDialogueFlag(LevelManager.instance.settings.outro)) {
				//Debug.Log("Whaaaa");
					DialogueManager.DialogueComplete = ToLevelOver;
					DialogueManager.ActivateDialogue(LevelManager.instance.settings.outro);
					LevelManager.instance.settings.StoreDialogueFlag(LevelManager.instance.settings.outro);
				}
				else {
					ToLevelOver();
				}
            }
            else
            {
                ToLevelOver();
            }
        }
    }

    void ToLevelOver()
    {
        GroupManager.main.activeGroup = GroupManager.main.group["To Level Over"];
    }

    void LevelIntro()
    {
        // Fade In To Prologue
        ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate()
        {
            ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
            {
                DialogueManager.DialogueComplete = FadeToIntroDialogue;
				DialogueManager.ActivateDialogue(DialogueManager.instance.defaultMap["Intro1"]);
                //GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
            });
        });
    }

    void FadeToIntroDialogue()
    {
        if (LevelManager.instance.Level == -1)
        {
            ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate()
            {
                ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
                {
                    DialogueManager.DialogueComplete = delegate()
                    {
                        ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate()
                        {
                            GameWorld.dialogueOff = false;
                            GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
                        });
                    };
					DialogueManager.ActivateDialogue(DialogueManager.instance.defaultMap["Intro2"]);
                    //GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
                });
            });
        }
    }

    void LevelStart()
    {
        // Clear resources
		var instance = LevelManager.instance;

        instance.Clear();

        // Next level
		instance.Next();

        dialogueOff = !success;
		var intro = instance.settings.intro;
		/*if (intro != null) {
			if (!instance.settings.HasDialogueFlag(intro)) {
				
				DialogueManager.DialogueComplete = GoBackToLevel;
				DialogueManager.ActivateDialogue(intro);
				instance.settings.StoreDialogueFlag(intro);
			}
		}*/


       // if (dialogueOff || LevelManager.instance.Level >= DialogueManager.dialogueIndex.Length)
		if (dialogueOff || intro == null)
        {
            ScreenFader.StartFade(Color.black, Color.clear, 1.0f, delegate()
            {
                GoBackToLevel();
            });
        }
        else 
        {
            //DialogueManager.CurrentDialogue = DialogueManager.dialogueIndex[LevelManager.instance.Level];
			//if (LevelManager.instance.settings.intro != null) {
				//DialogueManager.ActivateDialogue(LevelManager.instance.settings.intro);
			//}
			//Debug.Log("Resetting dialogues...");
			instance.settings.ResetStoredDialogues();
            ScreenFader.StartFade(Color.black, Color.clear, 1.0f, delegate()
            {
				//
				if (intro != null) {
					if (!instance.settings.HasDialogueFlag(intro)) {
						
						DialogueManager.DialogueComplete = GoBackToLevel;
						DialogueManager.ActivateDialogue(intro);
						instance.settings.StoreDialogueFlag(intro);
					}
				}
				//DialogueManager.
                //GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
            });
        }

        success = true;
    }

    public static void GoBackToLevel()
    {
        GroupManager.main.activeGroup = GroupManager.main.group["Running"];
    }

    void LevelOver()
    {
        if (success && PlayerPrefs.GetInt("Level", 0) <= LevelManager.instance.Level)
        {
            PlayerPrefs.SetInt("Level", LevelManager.instance.Level + 1);
            PlayerPrefs.Save();
        }
    }
}
