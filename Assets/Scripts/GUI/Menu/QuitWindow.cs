using UnityEngine;
using System.Collections;
using Grouping;
using System;

public class QuitWindow : MonoBehaviour
{
    public float windowSize = 0.5f;
    public float buttonSize = 0.25f;

	private float _windowSize;
	private float _buttonSize;

    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Exiting"].Add(this);
		ResetSize();
    }

	void ResetSize() {
		_windowSize = windowSize * Screen.height;
		_buttonSize = buttonSize * Screen.height;
	}

    void Update()
    {
		if (GUIManager.ScreenResized) {
			ResetSize();
		}
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
        }
    }

    void OnGUI()
    {
            //GUI.skin = GUIManager.GetSkin();

		GUI.Window(1, new Rect((Screen.width- _windowSize) * 0.5f, (Screen.height - _windowSize * 0.55f) * 0.5f, _windowSize, _windowSize * 0.55f), DoMenuWindow, "", GUIManager.Style.inGameWindow);
    }

    void DoMenuWindow(int windowID)
    {
        GUILayout.BeginVertical();
		GUILayout.Label("So, you want to quit.", GUIManager.Style.overTitle);
		//  GUILayout.FlexibleSpace();
		//  GUILayout.FlexibleSpace();

		//  GUILayout.FlexibleSpace();
		GUILayout.Label("Make sure you clean your hands after playing this game!", GUIManager.Style.overMessage);
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
		if (GUILayout.Button("I won't", GUIManager.Style.buttonNo, GUILayout.Width(_buttonSize), GUILayout.Height(_buttonSize / 2.0f)))
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
        }
        GUILayout.FlexibleSpace();
		if (GUILayout.Button("I will", GUIManager.Style.buttonYes, GUILayout.Width(_buttonSize), GUILayout.Height(_buttonSize / 2.0f)))
        {
            Application.Quit();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.FlexibleSpace();

        GUILayout.EndVertical();
    }
}
