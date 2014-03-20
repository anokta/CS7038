using UnityEngine;
using System.Collections;
using Grouping;

public class InGameGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GroupManager.main.group["Running"].Add(this);
	}

    void OnGUI ()
    {
        GUI.skin = GUIManager.GetSkin();

        if(GUI.Button(new Rect(0, Screen.height - Screen.height / 8, Screen.height / 8, Screen.height / 8), "Back"))
        {
            ScreenFader.StartFade(Color.clear, Color.black, 0.5f, delegate()
            {
                GroupManager.main.activeGroup = GroupManager.main.group["Level Over"];

                ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
                {
                    GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
                });
            });
        }
    }
}
