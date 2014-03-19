using UnityEngine;
using System.Collections;
using Grouping;

public class GUIManager : MonoBehaviour
{

	// Menu items
	public GameObject menuLayout;
	public GUIText start, over;

	// Use this for initialization
	void Start()
    {
        GroupManager.main.group["Menu"].Add(this, new GroupDelegator(null, GameMenu, null));
        GroupManager.main.group["Intro"].Add(this, new GroupDelegator(null, LevelStart, null));
        GroupManager.main.group["Level Start"].Add(this, new GroupDelegator(null, LevelStart, null));
        GroupManager.main.group["Level Over"].Add(this, new GroupDelegator(null, LevelOver, null));
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

    void OnGUI()
    {
        // For testing only //
        // TODO: Replace this with Game Menu script
        if (GroupManager.main.activeGroup == GroupManager.main.group["Level Over"])
        {
            if(GUI.Button(new Rect(Screen.width/2 - 50, 2*Screen.height/3, 100, 50), "Next Level"))
            {
                GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
            }
        }
    }

	void GameMenu()
	{
		over.enabled = false;

		menuLayout.SetActive(true);
	}

	void LevelStart()
	{
		over.enabled = false;

		menuLayout.SetActive(false);
	}

	void LevelOver()
	{
		over.enabled = true;
		over.text = "Handsomely done!\nScore: " + GlobalState.score;
	}
}
