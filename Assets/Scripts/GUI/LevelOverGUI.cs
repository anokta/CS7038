using UnityEngine;
using System.Collections;
using Grouping;

public class LevelOverGUI : MonoBehaviour
{
    public float windowSize = 0.9f;
    public float buttonSize = 0.2f;

    Rect guiWindow;

    string overTitle, overMessage;

    string[] winTitles =
    {
        "Handsomely done!",
        "High five!",
        "Hands down, you rock!",
        "Well handled!",
        "Cleanly done!",
        "I’ve got to hand it to you."
    };
    string[] winMessages =
    {
        "You did a great job. Mankind will forever be grateful of your honorable exploits.",
        "Excellent work there. Thanks to you, the patient you saved will one day discover a cure for cancer, diabetes, pessimism and dislocative shoulder disorder.",
        "You deserve a handful of medals for bravery beyond the call of duty.",
        "You are a brilliant doctor, well deserving your M.D status.",
        "The Handurian Flu trembles and fears at your sight. It has no chance against your mighty ways!",
        "You get an A for style, because there is no letter that precedes A in the Latin alphabet.",
        "If you assembled an army from all the people you have saved, you’d be able to overthrow the government. Just saying.",
        "The patient you saved will one day become a charismatic leader who will lead Mankind into victory against the imminent alien invasion.",
        "The patient you saved will one day invent a time machine, which he will use to go back in time and save your life, thereby ensuring the streamlined continuity of the universe and preventing a time-paradox from causing it to implode."
    };

    string[] loseTitles =
    {
        "Hand on a minute.",
        "Caught you red handed!"
    };
    string[] loseMessages = 
    {
        "I saw what you did there, treating that patient with filthy hands! That was a bad idea.",
        "In retrospect, treating a patient with filthy hands was more harmful than helpful",
        "I am appalled at your unsanitary medical practices. Are you a real doctor?"
    };

    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Level Over"].Add(this);
        GroupManager.main.group["Level Over"].Add(this, new GroupDelegator(null, Enter, null));

        buttonSize *= Screen.height;
        windowSize *= Screen.height;
        guiWindow = new Rect(Screen.width / 2.0f - windowSize / 2.0f, Screen.height / 2.0f - windowSize / 2.0f, windowSize, windowSize);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameWorld.success)
                LevelManager.Instance.Level--;

            FadeToMainMenu();
        }
    }

    // Update is called once per frame
    void OnGUI()
    {
        GUI.skin = GUIManager.GetSkin();

        GUI.Window(2, guiWindow, DoGameOverWindow, GUIContent.none, GUI.skin.GetStyle("over window"));
    }

    void DoGameOverWindow(int windowID)
    {
        // Info
        GUILayout.BeginVertical();
        GUILayout.Label(overTitle, GUI.skin.GetStyle("over title"));
        GUILayout.Label(overMessage, GUI.skin.GetStyle("over message"));

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        // Restart
        if (GUILayout.Button("Restart", GUI.skin.GetStyle("restart"), GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
        {
            LevelManager.Instance.Level--;

            FadeToLevelStart();
        }
        GUILayout.FlexibleSpace();
        // Next Level
        if (GameWorld.success)
        {
            if (GUILayout.Button("Next Level", GUI.skin.GetStyle("continue"), GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
            {
                FadeToLevelStart();
            }

            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        // Go Back To Menu
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Menu", GUILayout.Width(buttonSize / 2.0f), GUILayout.Height(buttonSize / 2.0f)))
        {
            if (!GameWorld.success)
                LevelManager.Instance.Level--;

            FadeToMainMenu();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.EndVertical();
    }

    void Enter()
    {
        overTitle = GameWorld.success ? winTitles[Random.Range(0, winTitles.Length)] : loseTitles[Random.Range(0, loseTitles.Length)];
        overMessage = GameWorld.success ? winMessages[Random.Range(0, winMessages.Length)] : loseMessages[Random.Range(0, loseMessages.Length)];
    }

    void FadeToLevelStart()
    {
        ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate()
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
        });
    }

    void FadeToMainMenu()
    {
        ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate()
        {
            // Clear resources
            LevelManager.Instance.Clear();

            ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
            {
                GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
            });
        });
    }
}