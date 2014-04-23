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
            GUI.skin = GUIManager.GetSkin();

		GUI.Window(1, new Rect((Screen.width- _windowSize) * 0.5f, (Screen.height - _windowSize * 0.55f) * 0.5f, _windowSize, _windowSize * 0.55f), DoMenuWindow, "", GUI.skin.GetStyle("ingame window"));
    }

    void DoMenuWindow(int windowID)
    {
        GUILayout.BeginVertical();
		GUILayout.Label("So, you want to quit.", GUI.skin.GetStyle("over title"));
		//  GUILayout.FlexibleSpace();
		//  GUILayout.FlexibleSpace();

		//  GUILayout.FlexibleSpace();
        GUILayout.Label("Make sure you clean your hands after playing this game!", GUI.skin.GetStyle("over message"));
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
		if (GUILayout.Button("I won't", GUI.skin.GetStyle("button no"), GUILayout.Width(_buttonSize), GUILayout.Height(_buttonSize / 2.0f)))
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
        }
        GUILayout.FlexibleSpace();
		if (GUILayout.Button("I will", GUI.skin.GetStyle("button yes"), GUILayout.Width(_buttonSize), GUILayout.Height(_buttonSize / 2.0f)))
        {
            Application.Quit();
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
