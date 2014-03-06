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
        GroupManager.main.group["Game"].Add(this, new GroupDelegator(null, LevelStart, null));
        GroupManager.main.group["Over"].Add(this, new GroupDelegator(null, LevelOver, null));
	}
	
	// Update is called once per frame
	void Update()
	{
	
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
