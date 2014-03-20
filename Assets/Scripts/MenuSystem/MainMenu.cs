using UnityEngine;
using System.Collections;
using Grouping;

public class MainMenu : MonoBehaviour
{ 
    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Main Menu"].Add(this);
    }

    void OnGUI()
    {
        // Start
        if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10.0f, Screen.height / 2 - Screen.height / 5.0f, Screen.width / 5.0f, Screen.height / 10.0f), "Start"))
        {
            LevelManager.Instance.Level = -1;
            ScreenFader.StartFade(Color.clear, Color.black, 1.0f, AfterFadeOut);
        }
        // Continue
        if (PlayerPrefs.GetInt("Level", 0) > 0 && GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10.0f, Screen.height / 2, Screen.width / 5.0f, Screen.height / 10.0f), "Continue"))
        {
            LevelManager.Instance.Level = PlayerPrefs.GetInt("Level", 0) - 1;
            ScreenFader.StartFade(Color.clear, Color.black, 1.0f, AfterFadeOut);
        }
        // Level Select
        if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10.0f, Screen.height / 2 + Screen.height / 5.0f, Screen.width / 5.0f, Screen.height / 10.0f), "Level Select"))
        {
            ScreenFader.StartFade(Color.clear, Color.black, 0.5f, delegate()
            {
                ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
                {
                    GroupManager.main.activeGroup = GroupManager.main.group["Level Select"];
                });
            });
        }

        // Mute
        if (GUI.Button(new Rect(Screen.width - 120, Screen.height - 70, 100, 50), "Mute"))
        {
            AudioListener.volume = 1 - AudioListener.volume;
            PlayerPrefs.SetFloat("Audio Volume", AudioListener.volume);
        }
    }

    void AfterFadeOut()
    {
        // Start the level
        if (LevelManager.Instance.Level == -1)
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Intro"];
        }
        else
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
        }
    }
}
