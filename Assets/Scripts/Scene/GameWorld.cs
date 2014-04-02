using UnityEngine;
using Grouping;

public class GameWorld : MonoBehaviour
{
    public enum LevelOverReason { Success, PatientInfected, LaserKilledPlayer, LaserKilledPatient, ExplosionKilledPlayer, ExplosionKilledPatient }
    public static LevelOverReason levelOverReason;

    public static bool success
    {
        get { return levelOverReason == LevelOverReason.Success; }
        set { levelOverReason = value ? LevelOverReason.Success : LevelOverReason.PatientInfected; }
    }

    // Use this for initialization
    void Start()
    {
        //Application.LoadLevel("MainMenu");

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AudioListener.volume = PlayerPrefs.GetFloat("Audio Volume", 1.0f);

        GroupManager.main.group["Main Menu"].Add(this, new GroupDelegator(null, GameMenu, null));
        GroupManager.main.group["Intro"].Add(this, new GroupDelegator(null, LevelIntro, null));
        GroupManager.main.group["Running"].Add(this);
        GroupManager.main.group["Level Start"].Add(this, new GroupDelegator(null, LevelStart, null));
        GroupManager.main.group["Level Over"].Add(this, new GroupDelegator(null, LevelOver, null));
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
        ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
        {
            DialogueManager.DialogueComplete = FadeToIntroDialogue;
            GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
        });
    }

    void FadeToIntroDialogue()
    {
        ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate()
        {
            ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
            {
                DialogueManager.DialogueComplete = delegate() { GroupManager.main.activeGroup = GroupManager.main.group["Level Start"]; };
                GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
            });
        });
    }

    void LevelStart()
    {
        // Clear resources
        LevelManager.Instance.Clear();

        // Next level
        LevelManager.Instance.Next();

        success = true;

        // TEMPORARY HACK
        DialogueManager.CurrentDialogue = (LevelManager.Instance.Level == 0) ? 1 : (LevelManager.Instance.Level + 5);
        if (LevelManager.Instance.Level > 1)
        {
            ScreenFader.StartFade(Color.black, Color.clear, 1.0f, delegate()
            {
                GoBackToLevel();
            });

            return;
        }
        //

        ScreenFader.StartFade(Color.black, Color.clear, 1.0f, delegate()
        {
            DialogueManager.DialogueComplete = GoBackToLevel;
            GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
        });
    }

    void GameMenu()
    {
        // Clear resources
        LevelManager.Instance.Clear();
    }

    public static void GoBackToLevel()
    {
        GroupManager.main.activeGroup = GroupManager.main.group["Running"];
    }

    void LevelOver()
    {
        if (levelOverReason == LevelOverReason.Success && PlayerPrefs.GetInt("Level", 0) <= LevelManager.Instance.Level)
        {
            PlayerPrefs.SetInt("Level", LevelManager.Instance.Level + 1);
            PlayerPrefs.Save();
        }
    }
}
