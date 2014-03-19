using UnityEngine;
using System.Collections;
using Grouping;

public class GUIManager : MonoBehaviour
{
    static string[] winTitles =
    {
        "Handsomely done!",
        "High five!",
        "Hands down, you rock!",
        "Well handled!",
        "Cleanly done!",
        "I’ve got to hand it to you."
    };
    static string[] winMessages =
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

    static string[] loseTitles =
    {
        "Hand on a minute.",
        "Caught you red handed!"
    };
    static string[] loseMessages = 
    {
       "I saw what you did there, treating that patient with filthy hands! That was a bad idea.",
        "In retrospect, treating a patient with filthy hands was more harmful than helpful",
    "I am appalled at your unsanitary medical practices. Are you a real doctor?"
    };


    // Menu items
    public GameObject menuLayout, over;
    public GUIText start, overTitle, overMessage;

    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Menu"].Add(this, new GroupDelegator(null, GameMenu, null));
        GroupManager.main.group["Intro"].Add(this, new GroupDelegator(null, LevelStart, null));
        GroupManager.main.group["Level Start"].Add(this, new GroupDelegator(null, LevelStart, null));
        GroupManager.main.group["Level Over"].Add(this, new GroupDelegator(null, LevelOver, null));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        // For testing only //
        // TODO: Replace this with Game Menu script
        if (GroupManager.main.activeGroup == GroupManager.main.group["Level Over"])
        {
            if (GameWorld.success)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 50, 2 * Screen.height / 3, 100, 50), "Next Level"))
                {
                    GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
                }
            }
            else
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 50, 2 * Screen.height / 3, 100, 50), "Restart"))
                {
                    LevelManager.Instance.Level--;
                    GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
                }
            }
        }

        if (GUI.Button(new Rect(Screen.width - 110, 30, 100, 20), "Toggle Audio"))
        {
            AudioListener.volume = 1 - AudioListener.volume;
        }
    }

    void GameMenu()
    {
        over.SetActive(false);

        menuLayout.SetActive(true);
    }

    void LevelStart()
    {
        over.SetActive(false);

        menuLayout.SetActive(false);
    }

    void LevelOver()
    {
        over.SetActive(true);

        overTitle.text = GameWorld.success ? winTitles[Random.Range(0, winTitles.Length)] : loseTitles[Random.Range(0, loseTitles.Length)];
        overMessage.text = GameWorld.success ? winMessages[Random.Range(0, winMessages.Length)] : loseMessages[Random.Range(0, loseMessages.Length)];
    }
}
