using UnityEngine;
using Grouping;

public class GameWorld : MonoBehaviour
{
    public enum LevelOverReason {
		Squashed,

 Success, PatientInfected, PlayerInfected, LaserKilledPlayer, LaserKilledPatient, ExplosionKilledPlayer, ExplosionKilledPatient }
    public static LevelOverReason levelOverReason;

    public static bool success
    {
        get { return levelOverReason == LevelOverReason.Success; }
        set { levelOverReason = value ? LevelOverReason.Success : LevelOverReason.PatientInfected; }
    }

	private static bool _dialogueOff;
	public static bool dialogueOff {
		get { return _dialogueOff; }
		set {
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
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: remove this when releasing
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LevelManager.Instance.Level -= 2;
            GroupManager.main.activeGroup = GroupManager.main.group["Level Over"];
            GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
        }

        //TODO: remove this when releasing
        if (Input.GetKeyDown(KeyCode.E))
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Level Over"];
            GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
        }

        //TODO: remove this when releasing
        if (Input.GetKeyDown(KeyCode.R))
        {
            LevelManager.Instance.Level--;
            success = false;
            GroupManager.main.activeGroup = GroupManager.main.group["Level Over"];
            GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
        }

        //TODO: remove this when releasing
        var patients = FindObjectsOfType<Patient>();
        var isOver = true;
        foreach (var patient in patients)
        {
            isOver &= patient.GetComponent<Patient>().IsTreated();
        }

        if (!success || isOver)
        {
            if (LevelManager.Instance.Level == 0 && success)
            {
                DialogueManager.DialogueComplete = ToLevelOver;
                GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
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
                GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
            });
        });
    }

    void FadeToIntroDialogue()
    {
        if (LevelManager.Instance.Level == -1)
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
                    GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
                });
            });
        }
    }

    void LevelStart()
    {
        // Clear resources
        LevelManager.Instance.Clear();

        // Next level
        LevelManager.Instance.Next();

        dialogueOff = !success;

        if (dialogueOff || LevelManager.Instance.Level >= DialogueManager.dialogueIndex.Length)
        {
            ScreenFader.StartFade(Color.black, Color.clear, 1.0f, delegate()
            {
                GoBackToLevel();
            });
        }
        else
        {
            DialogueManager.CurrentDialogue = DialogueManager.dialogueIndex[LevelManager.Instance.Level];

            ScreenFader.StartFade(Color.black, Color.clear, 1.0f, delegate()
            {
                DialogueManager.DialogueComplete = GoBackToLevel;
                GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
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
        if (success && PlayerPrefs.GetInt("Level", 0) <= LevelManager.Instance.Level)
        {
            PlayerPrefs.SetInt("Level", LevelManager.Instance.Level + 1);
            PlayerPrefs.Save();
        }
    }
}
