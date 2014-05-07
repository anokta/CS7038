using UnityEngine;
using System.Collections;
using Grouping;
using System;

public class Credits : MonoBehaviour
{
    public float windowSize = 1f;
    public float buttonSize = 0.2f;

    private float _windowSize;
    private float _buttonSize;

    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Credits"].Add(this);
        ResetSize();
    }

    void ResetSize()
    {
        _windowSize = windowSize * Screen.height;
        _buttonSize = buttonSize * Screen.height;
    }

    void Update()
    {
        if (GUIManager.ScreenResized)
        {
            ResetSize();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
        }
    }

    void OnGUI()
    {
        GUI.Window(1, new Rect((Screen.width - _windowSize) * 0.5f, (Screen.height - _windowSize * 0.9f) * 0.5f, _windowSize, _windowSize * 0.9f), DoMenuWindow, "Credits", GUIManager.Style.inGameWindow);
    }

    void DoMenuWindow(int windowID)
    {
        GUILayout.BeginVertical();

        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();

        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
      
        GUILayout.Label("Alper Gungormusler\nCharis Marangos\nChris Xue\nJonathan Kernan", GUIManager.Style.overTitle);
        GUILayout.FlexibleSpace();

		GUILayout.Label("Interactive Entertainment Technology\nTrinity College Dublin", GUIManager.Style.overMessage);
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
		GUILayout.Label("brought to you by Surewash", GUIManager.Style.overMessage);

        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
		if (GUILayout.Button("Back", GUIManager.Style.xbutton, GUILayout.Width(_buttonSize), GUILayout.Height(_buttonSize)))
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.EndVertical();
    }

}
