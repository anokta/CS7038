using UnityEngine;
using System.Collections;
using Grouping;
using System;

public class DonationWindow : MonoBehaviour
{
    public float windowSize = 0.5f;
    public float buttonSize = 0.25f;

    private float _windowSize;
    private float _buttonSize;

    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Donation"].Add(this);
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
            BackToMenu();
        }
    }

    void OnGUI()
    {
        Rect rect = GUI.Window(10, new Rect((Screen.width - _windowSize) * 0.5f, (Screen.height - _windowSize * 0.7f) * 0.5f, _windowSize, _windowSize * 0.7f), DoMenuWindow, "", GUIManager.Style.inGameWindow);
        
        float backSize = _buttonSize / 2.5f;
        GUI.Window(11, new Rect((rect.xMax - backSize * 0.75f), (rect.y - backSize * 0.25f), backSize, backSize), DoBackButton, "", GUIStyle.none);
        GUI.BringWindowToFront(11);
    }

    void DoBackButton(int windowID)
    {
        if (GUILayout.Button("Back", GUIManager.Style.xbutton))
        {
            BackToMenu();
        }
    }

    void DoMenuWindow(int windowID)
    {
        GUILayout.BeginVertical();
		GUILayout.Label("Support our cause.", GUIManager.Style.overTitle);

        GUILayout.Label("If you enjoyed the game, please help children by donating to End Polio Now.", GUIManager.Style.overMessage);
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Sure thing!", GUIManager.Style.buttonYes, GUILayout.Width(_buttonSize), GUILayout.Height(_buttonSize / 2.0f)))
        {
            string url = "http://www.endpolio.org/donate";
            #if UNITY_IPHONE || UNITY_ANDROID
				Application.OpenURL(url);
            #else
            Application.ExternalEval("window.open('" + url + "')");
            #endif
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.FlexibleSpace();

        GUILayout.EndVertical();
    }

    void BackToMenu()
    {
        AudioManager.PlaySFX("Level Swipe");
        GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
    }
}
