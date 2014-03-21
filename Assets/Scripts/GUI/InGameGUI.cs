using UnityEngine;
using System.Collections;
using Grouping;

public class InGameGUI : MonoBehaviour
{
    void Start()
    {
        GroupManager.main.group["Running"].Add(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Paused"];
        }
    }

    void OnGUI()
    {
        GUI.skin = GUIManager.GetSkin();

        if (GUI.Button(new Rect(Screen.height / 40, Screen.height - Screen.height / 8 - Screen.height / 40, Screen.height / 8, Screen.height / 8), "Pause", GUI.skin.GetStyle("pause")))
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Paused"];
        }
    }
}
