using UnityEngine;
using System.Collections;
using Grouping;

public class GameMenu : MonoBehaviour
{ 
    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Menu"].Add(this);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10.0f, Screen.height / 2 - Screen.height / 5.0f, Screen.width / 5.0f, Screen.height / 10.0f), "Start"))
        {
            LevelManager.Instance.Level = -1;
            ScreenFader.StartFade(Color.clear, Color.black, 1.0f, AfterFadeOut);
        }
        if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10.0f, Screen.height / 2 + Screen.height / 10.0f, Screen.width / 5.0f, Screen.height / 10.0f), "Continue"))
        {
            LevelManager.Instance.Level = PlayerPrefs.GetInt("Level", 0) - 1;
            ScreenFader.StartFade(Color.clear, Color.black, 1.0f, AfterFadeOut);
        }
        if (GUI.Button(new Rect(5, Screen.height - 35, 100, 30), "mute"))
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
