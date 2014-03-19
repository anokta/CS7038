using UnityEngine;
using System.Collections;
using Grouping;

public class GameMenu : MonoBehaviour
{ 
    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Menu"].Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) || (GUIUtility.hotControl == 0 && Input.GetMouseButtonDown(0)))
        {
            ScreenFader.StartFade(Color.clear, Color.black, 1.0f, AfterFadeOut);
        }
    }

    void AfterFadeOut()
    {
        // Start the level
        GroupManager.main.activeGroup = GroupManager.main.group["Intro"];
    }
}
