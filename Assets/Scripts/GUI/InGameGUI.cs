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

        if (GUI.Button(new Rect(0, Screen.height - Screen.height / 8, Screen.height / 8, Screen.height / 8), "Pause"))
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Paused"];
        }
    }
}
