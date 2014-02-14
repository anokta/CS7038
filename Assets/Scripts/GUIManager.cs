using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{

	// Menu items
	public GameObject menuLayout;
	public GUIText start, over;

	// Use this for initialization
	void Awake()
	{
		GameEventManager.GameMenu += GameMenu;
		GameEventManager.LevelStart += LevelStart;
		GameEventManager.LevelOver += LevelOver;
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
