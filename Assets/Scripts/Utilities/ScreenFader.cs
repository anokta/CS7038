using UnityEngine;
using System.Collections;
using System;
using Grouping;

public class ScreenFader : MonoBehaviour {
    
    public Texture mask;

    static Color startColor, endColor;
    static float speed;

    static bool fading;
    static float progress;

    static Action FadeComplete;


	void Start () {
        startColor = Color.clear;
        endColor = Color.clear;

        fading = false;
        progress = 0.0f;
	}
	

	void Update () {
        if(fading)
        {
            progress += speed;
            if (progress >= 1.0f)
            {
                fading = false;

                FadeComplete();
            }
        }
	}

    void OnGUI()
    {
        if (fading)
        {
            GUI.depth = -1;
            GUI.color = Color.Lerp(startColor, endColor, progress);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mask);
        }
    }

    public static void SetCallback(Action action)
    {
        FadeComplete = action;
    }

    public static void StartFade(Color start, Color end)
    {
        StartFade(start, end, 1.0f, FadeComplete);
    }

    public static void StartFade(Color start, Color end, float duration, Action action)
    {
        FadeComplete = action;

        startColor = start;
        endColor = end;

        speed = Time.deltaTime / duration;
        progress = 0.0f;

        fading = true;

        GroupManager.main.activeGroup = GroupManager.main.group["Fading"];
    }

    public static void FadeToState(string state, float fadeOut = 1.0f, float fadeIn = 0.5f)
    {
        StartFade(Color.clear, Color.black, fadeOut, delegate()
        {
            StartFade(Color.black, Color.clear, fadeIn, delegate()
            {
                GroupManager.main.activeGroup = GroupManager.main.group[state];
            });
        });
    }
}
