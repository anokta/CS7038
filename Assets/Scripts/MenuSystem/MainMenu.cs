using UnityEngine;
using System.Collections;
using Grouping;

public class MainMenu : MonoBehaviour
{ 
    // Use this for initialization
    public Texture2D menuBackground;

    void Start()
    {
        GroupManager.main.group["Main Menu"].Add(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Exit the application
            Application.Quit();
        }
    }

    void OnGUI()
    {
        GUI.skin = GUIManager.GetSkin();
        GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),menuBackground);
        // Continue
        GUILayout.BeginArea(new Rect(Screen.width/4, Screen.height/2, Screen.width/2, Screen.height/4));
        GUILayout.BeginHorizontal();
        if (PlayerPrefs.GetInt("Level", 0) > 0 && GUILayout.Button("Continue"))
        {
            LevelManager.Instance.Level = PlayerPrefs.GetInt("Level", 0) - 1;
            ScreenFader.StartFade(Color.clear, Color.black, 1.0f, AfterFadeOut);
        }
        // Start
        if (GUILayout.Button("Start"))
        {
            LevelManager.Instance.Level = -1;
            ScreenFader.StartFade(Color.clear, Color.black, 1.0f, AfterFadeOut);
        }
        // Level Select
        if (GUILayout.Button("Level Select"))
        {
            ScreenFader.StartFade(Color.clear, Color.black, 0.5f, delegate()
            {
                ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
                {
                    GroupManager.main.activeGroup = GroupManager.main.group["Level Select"];
                });
            });
        }
       GUILayout.EndHorizontal();
       GUILayout.EndArea();

        // Mute
        if (GUI.Button(new Rect(Screen.width - Screen.height / 10.0f, Screen.height - Screen.height / 10.0f, Screen.height / 10.0f, Screen.height / 10.0f), "Mute"))
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
