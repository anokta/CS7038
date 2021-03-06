﻿using UnityEngine;
using System.Collections;
using Grouping;
using System;

public class Credits : MonoBehaviour
{
    public float windowSize = 1f;
    public float buttonSize = 0.2f;

    private float _windowSize;
    private float _buttonSize;

	public Texture TrinityLogo;
	public Texture LogoBackground;
	float _sureWashRatio;
    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Credits"].Add(this);
		_sureWashRatio = GUIManager.Style.sureButton.normal.background.width /
		GUIManager.Style.sureButton.normal.background.height;
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
		var rect = GUI.Window(1, new Rect((Screen.width - _windowSize) * 0.5f, (Screen.height - _windowSize * 0.9f) * 0.5f, _windowSize, _windowSize * 0.9f), DoMenuWindow, "", GUIManager.Style.inGameWindow);
		float backSize = _buttonSize;
		//var butRec = new Rect((rect.xMax - backSize/2), (rect.y - backSize/2), backSize, backSize);
		var backRec = new Rect((rect.xMax - backSize*0.75f), (rect.y - backSize * 0.25f), backSize, backSize);
		GUI.Window(2, backRec, DoBackButton, "", GUIStyle.none);
		//GUI.FocusWindow(2);
		GUI.BringWindowToFront(2);
    }

    void DoMenuWindow(int windowID)
    {
        GUILayout.BeginVertical();
		{

			GUILayout.Label("Credits", GUIManager.Style.overTitle);
      
			//GUILayout.Label("Alper Gungormusler\nCharis Marangos\nChris Xue\nJonathan Kernan", GUIManager.Style.overTitle);
			GUILayout.Label("Alper Gungormusler", GUIManager.Style.creditsName);
			GUILayout.Label("Programming, Design & Audio", GUIManager.Style.creditsDescription);
			GUILayout.Label("Charis Marangos", GUIManager.Style.creditsName);
			GUILayout.Label("Programming, Graphics, Design & Puns", GUIManager.Style.creditsDescription);
			GUILayout.Label("Chris Jianghan Xue", GUIManager.Style.creditsName);
			GUILayout.Label("Programming", GUIManager.Style.creditsDescription);
			GUILayout.Label("Jonathan Kernan", GUIManager.Style.creditsName);
			GUILayout.Label("Programming", GUIManager.Style.creditsDescription);
			GUILayout.FlexibleSpace();
			//GUILayout.BeginHorizontal();
			//{
				//GUILayout.FlexibleSpace();
				//	GUILayout.BeginVertical();
				//	{
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				//GUILayout.Label(TrinityLogo, GUIStyle.none, GUILayout.Height(Screen.height * 0.125f), GUILayout.Width(Screen.height * 0.125f));
				GUILayout.Label("Interactive Entertainment Technology\nTrinity College Dublin", GUIManager.Style.creditsMessage);
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label("Brought to you by ", GUIManager.Style.creditsMessage);
				float height = GUIManager.Style.creditsMessage.CalcHeight(new GUIContent("Brought to you by "), Screen.width) * 2;
				if (GUILayout.Button("", GUIManager.Style.sureButton, GUILayout.Height(height), GUILayout.Width(height * _sureWashRatio))) {
					string url = "http://www.surewash.com";
					#if UNITY_IPHONE || UNITY_ANDROID
					Application.OpenURL(url);
					#else
					Application.ExternalEval("window.open('" + url + "')");
					#endif
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
				//	}
			//GUILayout.EndVertical();
				//GUILayout.FlexibleSpace();
			//}
		//GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
		}
        GUILayout.EndVertical();
    }

	void DoBackButton(int windowID) {
		if (GUILayout.Button("Back", GUIManager.Style.xbutton) )
		{
            BackToMenu();
		}
	}

    void BackToMenu()
    {
        AudioManager.PlaySFX("Level Swipe");
        GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
    }
}
