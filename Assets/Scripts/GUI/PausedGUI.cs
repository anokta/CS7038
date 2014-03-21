using UnityEngine;
using System.Collections;
using Grouping;

public class PausedGUI : MonoBehaviour {

    public float windowSize = 0.6f;
    public float buttonSize = 0.2f;

	// Use this for initialization
	void Start () {
        GroupManager.main.group["Paused"].Add(this);

        windowSize *= Screen.height; 
        buttonSize *= Screen.height;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }

    void OnGUI()
    {
        GUI.skin = GUIManager.GetSkin();

        GUI.Window(1, new Rect(Screen.width / 2.0f - windowSize / 2.0f, Screen.height / 2.0f - windowSize / 2.0f, windowSize, windowSize), DoMenuWindow, "PAUSED", GUI.skin.GetStyle("ingame window"));
    }

    void DoMenuWindow(int windowID)
    {
        GUILayout.BeginVertical();

        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();

        // Mute
        //TODO: Temporary hack, fix
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Restart", GUI.skin.GetStyle("restart ingame"), GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
        {
            RestartLevel();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        // Options
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Menu", GUI.skin.GetStyle("menu"), GUILayout.Width(buttonSize / 2.0f), GUILayout.Height(buttonSize / 2.0f)))
        {
            FadeToMainMenu();
        }
        GUILayout.FlexibleSpace();
        string styleOfVolume = AudioListener.volume <= 0.001f ? "volume off" : "volume on";
        if (GUILayout.Button("Mute", GUI.skin.GetStyle(styleOfVolume), GUILayout.Width(buttonSize / 2.0f), GUILayout.Height(buttonSize / 2.0f)))
        {
            AudioListener.volume = 1 - AudioListener.volume;
            PlayerPrefs.SetFloat("Audio Volume", AudioListener.volume);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        // Go Back
		GUILayout.BeginHorizontal(); 
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Back", GUI.skin.GetStyle("back"), GUILayout.Width(buttonSize / 2.0f), GUILayout.Height(buttonSize / 2.0f)))
        {
            ResumeGame();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

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

    void ResumeGame()
    {
        GroupManager.main.activeGroup = GroupManager.main.group["Running"];
    }
}
