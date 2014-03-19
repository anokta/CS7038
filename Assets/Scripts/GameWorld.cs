using UnityEngine;
using Grouping;

public class GameWorld : MonoBehaviour
{
    public static bool success;

    // Use this for initialization
    void Start()
    {
        //Application.LoadLevel("MainMenu");

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AudioListener.volume = PlayerPrefs.GetFloat("Audio Volume", 1.0f);

        GroupManager.main.group["Intro"].Add(this, new GroupDelegator(null, LevelIntro, null));
        GroupManager.main.group["Running"].Add(this);
        GroupManager.main.group["Running"].Add(this, new GroupDelegator(null, LevelRunning, null));
        GroupManager.main.group["Level Start"].Add(this, new GroupDelegator(null, LevelStart, null));
        GroupManager.main.group["Level Over"].Add(this, new GroupDelegator(null, LevelOver, null));

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Exit the application
            Application.Quit();
        }

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

        if (isOver)
        {
            if (LevelManager.Instance.Level == 0 && success)
            {
                DialogueManager.nextState = GroupManager.main.group["Level Over"];
                GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
            }
            else
            {
                GroupManager.main.activeGroup = GroupManager.main.group["Level Over"];
            }
        }
    }

    void GameMenu()
    {
        //TODO
    }

    void LevelIntro()
    {
        DialogueManager.nextState = GroupManager.main.group["Running"];
        GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
    }

    void LevelRunning()
    {
        // Intro ?
        if (LevelManager.Instance.Level == -1)
        {
            DialogueManager.nextState = GroupManager.main.group["Level Start"];
            GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
        }
    }

    void LevelStart()
    {
        // Next level
        LevelManager.Instance.Next();

        // TEMPORARY HACK
        DialogueManager.CurrentDialogue = (LevelManager.Instance.Level == 0) ? 1 : 6;
        //

        DialogueManager.nextState = GroupManager.main.group["Running"];
        GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];

        success = true;
    }

    void LevelOver()
    {
        // Clear resources
        LevelManager.Instance.Clear();
    }
}
