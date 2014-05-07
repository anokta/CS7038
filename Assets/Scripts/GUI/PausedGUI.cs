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

//            GUI.skin = GUIManager.GetSkin();

			var rect = GUI.Window(1, new Rect(Screen.width / 2.0f - _actualWindowSize / 2.0f, Screen.height / 2.0f - (_actualWindowSize* 0.7f) / 2.0f, _actualWindowSize, _actualWindowSize * 0.7f), DoMenuWindow, "", GUIManager.Style.inGameWindow);
			//var butRec = new Rect(rect.x - _actualButtonSize * 0.75f, (Screen.height - _actualButtonSize / 2) / 2, _actualButtonSize / 2, _actualButtonSize / 2);
			float backSize = _actualButtonSize / 2;
			//var butRec = new Rect((rect.xMax - backSize/2), (rect.y - backSize/2), backSize, backSize);
			var backRec = new Rect((rect.xMax - backSize*0.75f), (rect.y - backSize * 0.25f), backSize, backSize);
			GUI.Window(2, backRec, DoBackButton, "", GUIStyle.none);
			//GUI.FocusWindow(2);
			GUI.BringWindowToFront(2);
        }
    }

	void DoBackButton(int windowID) {
		if (GUILayout.Button("Back", GUIManager.Style.xbutton) )
		{
			AudioManager.PlaySFX("Level Swipe Reversed");

			guiTargetScale = 0.0f;
			action = ResumeGame;
		}
	}

    void DoMenuWindow(int windowID)
    {
        if (guiCurrentScale != guiTargetScale)
            return;
		GUILayout.BeginVertical();
		GUILayout.Label("Paused", GUIManager.Style.overTitle);
        
		//GUILayout.FlexibleSpace();
		GUILayout.Label("Level " + (LevelManager.Instance.Level + 1), GUIManager.Style.levelLabel);

		GUILayout.BeginHorizontal(); {
			GUILayout.FlexibleSpace();
            // Menu
            GUILayout.BeginVertical();
            {
                GUILayout.FlexibleSpace();
				if (GUILayout.Button("Menu", GUIManager.Style.menu, GUILayout.Width(_actualButtonSize / 2.0f), GUILayout.Height(_actualButtonSize / 2.0f)))
                {
                    FadeToMainMenu();
                }
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			//Restart
			GUILayout.BeginVertical(); {
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Restart", GUIManager.Style.restartInGame, GUILayout.Width(_actualButtonSize), GUILayout.Height(_actualButtonSize))) {
					RestartLevel();
				}
				GUILayout.FlexibleSpace();
			} GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
            //Volume controls
            GUILayout.BeginVertical();
            {
                GUILayout.FlexibleSpace();
                //string styleOfVolume = AudioListener.volume <= 0.001f ? "volume off" : "volume on";
				GUIStyle styleOfVolume = AudioListener.volume <= 0.001f ? GUIManager.Style.volumeOff : GUIManager.Style.volumeOn;
                if (GUILayout.Button("Mute", styleOfVolume, GUILayout.Width(_actualButtonSize / 2.0f), GUILayout.Height(_actualButtonSize / 2.0f)))
                {
                    AudioListener.volume = 1 - AudioListener.volume;
                    PlayerPrefs.SetFloat("Audio Volume", AudioListener.volume);
                }
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
		}
        GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();


    }

    void FadeToMainMenu()
    {

		ScreenFader.QueueEvent(BackgroundRenderer.instance.SetSunBackground);
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
