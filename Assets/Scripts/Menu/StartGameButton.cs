using UnityEngine;
using System.Collections;

public class StartGameButton : TouchLogic {

    public GUIContent guiContent;
    public GUISkin menuItemSkin;

	// Update is called once per frame
	void Update () {
		PollTouches ();
	}

	public override void OnTouchDown(){

	}

	public override void OnTouchUp(){
		Application.LoadLevel("MainScene");
	}

	//TestingPurposes
	void OnMouseDown(){
		Application.LoadLevel("MainScene");

	}

    void OnGUI() 
    {
        var groupWidth = 120;
        var groupHeight = 150;

        var screenWidth = Screen.width;
        var screenHeight = Screen.height;

        var groupX = (screenWidth - groupWidth) / 2;
        var groupY = (screenHeight - groupHeight) / 2;
        
        GUI.BeginGroup(new Rect(groupX, groupY, 300, groupHeight));
        GUI.Box(new Rect(0, 0, 300, groupHeight), "Level Select");

        if (GUI.Button(new Rect(10, 30, 200, 60), "Start Game", menuItemSkin.customStyles[0]))
        {
            Application.LoadLevel("MainScene");
            LevelManager.Instance.Load(0);
        }
        if (GUI.Button(new Rect(10, 70, 100, 30), "Level 2"))
        {
            Application.LoadLevel("MainScene");
            LevelManager.Instance.Load(1);
        }
        if (GUI.Button(new Rect(10, 110, 100, 30), "Level 3"))
        {
            Application.LoadLevel("MainScene");
            LevelManager.Instance.Load(2);
        }

        GUI.EndGroup();
    }
}
