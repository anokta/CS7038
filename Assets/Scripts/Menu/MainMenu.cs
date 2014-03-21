﻿using UnityEngine;
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

        //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), menuBackground);
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height * 0.4f), "Handy MD", GUI.skin.GetStyle("title"));

        // PLAY //
        if (GUI.Button(new Rect(Screen.width / 2.0f - Screen.height / 4.0f, Screen.height / 2.0f - Screen.height / 8.0f, Screen.height / 2.0f, Screen.height / 2.0f), "Play", GUI.skin.GetStyle("play")))
        {
            ScreenFader.StartFade(Color.clear, Color.black, 1.0f, AfterFadeOut);
        }


        // LEFT CORNER //
        GUILayout.BeginArea(new Rect(GUIManager.OffsetX() * 2.0f, Screen.height - GUIManager.OffsetY() * 2.0f - GUIManager.ButtonSize(), 2.0f * GUIManager.ButtonSize(), GUIManager.ButtonSize()));

        GUILayout.BeginHorizontal();

        // Mute
		//TODO: Temporary hack, fix
		string styleOfVolume = AudioListener.volume <= 0.001f ? "volume off" : "volume on";
        if (GUILayout.Button("Mute", GUI.skin.GetStyle(styleOfVolume), GUILayout.Width(GUIManager.ButtonSize()), GUILayout.Height(GUIManager.ButtonSize())))
        {
            AudioListener.volume = 1 - AudioListener.volume;
            PlayerPrefs.SetFloat("Audio Volume", AudioListener.volume);
        }

        // Credits
        if (GUILayout.Button("Credits", GUI.skin.GetStyle("credits"), GUILayout.Width(GUIManager.ButtonSize()), GUILayout.Height(GUIManager.ButtonSize())))
        {
            // TODO : Credits
        }

        GUILayout.EndHorizontal();

        GUILayout.EndArea();


        // RIGHT CORNER //
        GUILayout.BeginArea(new Rect(Screen.width - Screen.height / 10.0f - GUIManager.OffsetX() * 2.0f, Screen.height - Screen.height / 10.0f - GUIManager.OffsetY() * 2.0f, Screen.height / 10.0f, Screen.height / 10.0f));

        GUILayout.BeginHorizontal();

        // Url
        if (GUILayout.Button("i", GUI.skin.GetStyle("info"), GUILayout.Width(Screen.height / 10.0f), GUILayout.Height(Screen.height / 10.0f)))
        {
            Application.OpenURL("http://www.surewash.com/");
        }

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
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
            ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
            {
                GroupManager.main.activeGroup = GroupManager.main.group["Level Select"];
            });
        }
    }
}
