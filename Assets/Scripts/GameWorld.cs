using UnityEngine;

public class GameWorld : MonoBehaviour
{
    private LevelManager levelManager;

    // Use this for initialization
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        levelManager = new LevelManager();

        GameEventManager.GameMenu += GameMenu;
        GameEventManager.LevelStart += LevelStart;
        GameEventManager.LevelOver += LevelOver;

        GameEventManager.TriggerGameMenu();

        Grouping.GroupManager.main.group["Game"].Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Exit the application
            GameEventManager.TriggerGameQuit();
        }

        switch (GameEventManager.CurrentState)
        {
            case GameEventManager.GameState.InMenu:
                if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    // Start the level
                    GameEventManager.TriggerLevelStart();
                }

                break;

            case GameEventManager.GameState.Running:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    // Restart the level (for testing purposes)
                    GameEventManager.TriggerLevelOver();
                    levelManager.Level--;
                    GameEventManager.TriggerLevelStart();
                }

                #region TO_BE_DELETED

                // Testing purposes only //
                var patients = FindObjectsOfType<Patient>();
                var isOver = true;
                foreach (var patient in patients)
                {
                    isOver &= patient.GetComponent<Patient>().IsTreated();
                }

                if (isOver)
                {
                    GameEventManager.TriggerLevelOver();
                }

                break;

            case GameEventManager.GameState.Over:
                if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    // Start the level
                    GameEventManager.TriggerLevelStart();
                }
                #endregion

                break;
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
