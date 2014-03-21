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

        if (GUI.Button(new Rect(GUIManager.OffsetX(), GUIManager.OffsetY(), GUIManager.ButtonSize(), GUIManager.ButtonSize()), "Pause", GUI.skin.GetStyle("pause")))
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Paused"];
        }
    }
}
