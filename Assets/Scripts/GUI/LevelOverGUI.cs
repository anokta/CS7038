using UnityEngine;
using System.Collections;
using Grouping;

public class LevelOverGUI : MonoBehaviour
{
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

    float buttonSize;
    Rect guiWindow;

    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Level Over"].Add(this);
        GroupManager.main.group["Level Over"].Add(this, new GroupDelegator(null, Enter, Exit));
        
        buttonSize = Screen.height / 5.0f;
        guiWindow = new Rect(Screen.width / 2.0f - Screen.width / 3.0f, Screen.height / 2.0f - Screen.height / 3.0f, 2.0f * Screen.width / 3.0f, 2.0f * Screen.height / 3.0f - buttonSize);
    }

    // Update is called once per frame
    void OnGUI()
    {
        GUI.skin = GUIManager.GetSkin();

        GUILayout.BeginArea(guiWindow);

        GUILayout.Label(overTitle, GUI.skin.GetStyle("over title"));
        GUILayout.Label(overMessage, GUI.skin.GetStyle("over message"));

        GUILayout.EndArea();

        Rect buttonRect = new Rect(guiWindow.x + guiWindow.width / 2.0f - buttonSize / 2.0f, guiWindow.y + guiWindow.height, buttonSize, buttonSize);

        if (GameWorld.success)
        {
            buttonRect.x += buttonSize * 0.75f;
            if (GUI.Button(buttonRect, "Next Level", GUI.skin.GetStyle("continue")))
            {
                GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
            }
            buttonRect.x -= buttonSize * 1.5f;
        }

        if (GUI.Button(buttonRect, "Restart", GUI.skin.GetStyle("restart")))
        {
            LevelManager.Instance.Level--;
            GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];

            GUILayout.Space(Screen.height / 30.0f);
        }

        if (GUI.Button(new Rect(0, Screen.height - Screen.height / 8, Screen.height / 6, Screen.height / 8), "Menu"))
        {
            ScreenFader.StartFade(Color.clear, Color.black, 0.5f, delegate()
            {
                ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
                {
                    GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
                });
            });
        }
    }

    void Enter()
    {
        overTitle = GameWorld.success ? winTitles[Random.Range(0, winTitles.Length)] : loseTitles[Random.Range(0, loseTitles.Length)];
        overMessage = GameWorld.success ? winMessages[Random.Range(0, winMessages.Length)] : loseMessages[Random.Range(0, loseMessages.Length)];
    }

    void Exit()
    {
    }
}
