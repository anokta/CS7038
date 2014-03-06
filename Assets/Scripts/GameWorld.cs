using UnityEngine;
using Grouping;

public class GameWorld : MonoBehaviour
{
    private LevelManager levelManager;

    // Use this for initialization
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        levelManager = new LevelManager();

        GroupManager.main.group["Game"].Add(this);
        GroupManager.main.group["Game"].Add(this, new GroupDelegator(null, LevelStart, null));
        GroupManager.main.group["Over"].Add(this, new GroupDelegator(null, LevelOver, null));

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Exit the application
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Over"];
            GroupManager.main.activeGroup = GroupManager.main.group["Game"];
        }

        // Testing purposes only //
        var patients = FindObjectsOfType<Patient>();
        var isOver = true;
        foreach (var patient in patients)
        {
            isOver &= patient.GetComponent<Patient>().IsTreated();
        }

        if (isOver)
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Over"];
        }

    }

    void GameMenu()
    {
        //TODO
    }

    void LevelStart()
    {
        // Next level
        levelManager.Next();
    }

    void LevelOver()
    {
        // Clear resources
        levelManager.Clear();

        Grouping.GroupManager.main.activeGroup = Grouping.GroupManager.main.group["Dialogue"];
    }
}
    