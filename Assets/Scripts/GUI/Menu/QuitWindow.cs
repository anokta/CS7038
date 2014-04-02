﻿using UnityEngine;
using System.Collections;
using Grouping;
using System;

public class QuitWindow : MonoBehaviour
{
    public float windowSize = 0.5f;
    public float buttonSize = 0.2f;

    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Exiting"].Add(this);

        windowSize *= Screen.height;
        buttonSize *= Screen.height;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
        }
    }

    void OnGUI()
    {
            GUI.skin = GUIManager.GetSkin();

            GUI.Window(1, new Rect(Screen.width / 2.0f - windowSize / 2.0f, Screen.height / 2.0f - windowSize / 4.0f, windowSize, windowSize / 2.0f), DoMenuWindow, "Quit Game?", GUI.skin.GetStyle("ingame window"));
    }

    void DoMenuWindow(int windowID)
    {
        GUILayout.BeginVertical();

        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();

        GUILayout.FlexibleSpace();
        GUILayout.Label("Make sure you clean your hands after playing this game!", GUI.skin.GetStyle("over message"));
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("I will", GUI.skin.GetStyle("button gray"), GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
        {
            Application.Quit();
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("I won't", GUI.skin.GetStyle("button gray"), GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.FlexibleSpace();

        GUILayout.EndVertical();
    }

    void FadeToMainMenu()
    {
        ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate()
        {
            LevelManager.Instance.Level--;
            GroupManager.main.activeGroup = GroupManager.main.group["Level Over"];

            // Clear resources
            LevelManager.Instance.Clear();

            ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
            {
                GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
            });
        });
    }

    void RestartLevel()
    {
        ScreenFader.StartFade(Color.clear, Color.black, 0.5f, delegate()
        {
            LevelManager.Instance.Level--;
            GroupManager.main.activeGroup = GroupManager.main.group["Level Over"];

            // Clear resources
            LevelManager.Instance.Clear();

            GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
        });
    }
}