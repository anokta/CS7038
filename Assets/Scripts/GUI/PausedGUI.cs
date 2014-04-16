using UnityEngine;
using System.Collections;
using Grouping;
using System;

public class PausedGUI : MonoBehaviour
{

    public float windowSize = 0.6f;
    public float buttonSize = 0.2f;

	private float _actualWindowSize;
	private float _actualButtonSize;

    float guiCurrentScale, guiTargetScale;

    Action action;

	void ResetSize() {
		_actualWindowSize = windowSize * Screen.height;
		_actualButtonSize = buttonSize * Screen.height;
	}

    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Paused"].Add(this);
        GroupManager.main.group["Paused"].Add(this, new GroupDelegator(null, Enter, null));        

        guiCurrentScale = 0.0f;
        guiTargetScale = 0.0f;
    }

    void Update()
    {
		if (GUIManager.ScreenResized) {
			ResetSize();
		}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.PlaySFX("Level Swipe Reversed");
            guiTargetScale = 0.0f;
            action = ResumeGame;
        }

        if (guiCurrentScale != guiTargetScale)
        {
            guiCurrentScale = Mathf.Lerp(guiCurrentScale, guiTargetScale, Time.deltaTime * 20.0f);

            if (Mathf.Abs(guiCurrentScale - guiTargetScale) <= 0.1f)
            {
                guiCurrentScale = guiTargetScale;

                if (action != null)
                    action();
            }
        }
    }

    void OnGUI()
    {
        if (guiCurrentScale > 0.0f)
        {
            GUI.matrix = Matrix4x4.Scale(new Vector3(guiCurrentScale, guiCurrentScale, 1.0f));

            if (guiCurrentScale != guiTargetScale) GUI.enabled = false;

            GUI.skin = GUIManager.GetSkin();

			GUI.Window(1, new Rect(Screen.width / 2.0f - _actualWindowSize / 2.0f, Screen.height / 2.0f - _actualWindowSize / 2.0f, _actualWindowSize, _actualWindowSize), DoMenuWindow, "PAUSED", GUI.skin.GetStyle("ingame window"));
        }
    }

    void DoMenuWindow(int windowID)
    {
        if (guiCurrentScale != guiTargetScale)
            return;

        GUILayout.BeginVertical();

        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();

        GUILayout.Label("Level " + (LevelManager.Instance.Level + 1), GUI.skin.GetStyle("level label"));

        GUILayout.FlexibleSpace();

        // Mute
        //TODO: Temporary hack, fix
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
		if (GUILayout.Button("Restart", GUI.skin.GetStyle("restart ingame"), GUILayout.Width(_actualButtonSize), GUILayout.Height(_actualButtonSize)))
        {
            RestartLevel();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        // Options
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
		if (GUILayout.Button("Menu", GUI.skin.GetStyle("menu"), GUILayout.Width(_actualButtonSize / 2.0f), GUILayout.Height(_actualButtonSize / 2.0f)))
        {
            FadeToMainMenu();
        }
        GUILayout.FlexibleSpace();
        string styleOfVolume = AudioListener.volume <= 0.001f ? "volume off" : "volume on";
		if (GUILayout.Button("Mute", GUI.skin.GetStyle(styleOfVolume), GUILayout.Width(_actualButtonSize / 2.0f), GUILayout.Height(_actualButtonSize / 2.0f)))
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
		if (GUILayout.Button("Back", GUI.skin.GetStyle("back"), GUILayout.Width(_actualButtonSize / 2.0f), GUILayout.Height(_actualButtonSize / 2.0f)))
        {
            AudioManager.PlaySFX("Level Swipe Reversed");

            guiTargetScale = 0.0f;
            action = ResumeGame;
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
                GroupManager.main.activeGroup = GroupManager.main.group["Level Select"];

                AudioManager.PlaySFX("Menu Next");
            });
        });
    }

    void RestartLevel()
    {
        ScreenFader.StartFade(Color.clear, Color.black, 0.5f, delegate()
        {
            LevelManager.Instance.Level--;
            GameWorld.success = false;
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

    void Enter()
    {
        guiTargetScale = 1.0f;
        action = null;
		ResetSize();
    }
}
