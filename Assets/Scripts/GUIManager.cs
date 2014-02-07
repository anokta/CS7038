using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

    // Menu items
    public GameObject menuLayout;
    public GUIText start;

	// Use this for initialization
    void Awake()
    {
        GameEventManager.GameMenu += GameMenu;
        GameEventManager.LevelStart += LevelStart;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void GameMenu()
    {
        menuLayout.SetActive(true);
    }

    void LevelStart()
    {
        menuLayout.SetActive(false);
    }
}
