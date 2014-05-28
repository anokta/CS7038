using UnityEngine;
using System.Collections;
using Grouping;
using Prime31;

public class SocialIntegration : MonoBehaviour {
    
	#if UNITY_IPHONE || UNITY_ANDROID

    protected bool popupActive = false;
    protected string popupMessage = "";
    protected float popupY = 1.0f;
    
    protected virtual void Start () {
        GroupManager.main.group["Level Over"].Add(this);
	}

    protected virtual void OnEnable()
    {
        popupY = 1.0f;
    }

    void OnGUI()
    {
        if (popupActive || popupY < 1.0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                popupActive = false;
            }

            popupY = Mathf.Lerp(popupY, popupActive ? 0.9f : 1.1f, Time.deltaTime * 4);

            GUI.Window(5, new Rect(Screen.width * 0.5f - Screen.height * 0.25f, Screen.height * popupY, Screen.height * 0.5f, Screen.height * 0.1f), DoPopup, "", GUIManager.Style.inGameWindow);
        }
    }

    void DoPopup(int id)
    {
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(popupMessage, GUIManager.Style.log);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
    }

    public virtual void Post(string info, string url)
    {
    }

    #endif
}
