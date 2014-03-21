using UnityEngine;
using System.Collections;

public class MainMenuScreen : TouchLogic
{

    public GUIContent guiContent;
    public GUISkin menuItemSkin;
    public Texture surewashLogo;

    void Update()
    {
        PollTouches();
    }

    public override void OnTouchDown()
    {
        Application.OpenURL("http://www.surewash.com/");
    }

    //Testing purposes only
    void OnMouseDown()
    {

        Application.OpenURL("http://www.surewash.com/");
    }

    void OnGUI()
    {
        
        int numButtons = 3;

        int groupWidth  = (int)Screen.width /2;
        int groupHeight = (int)(Screen.height *3)/8;

        int screenWidth  = Screen.width;
        int screenHeight = Screen.height;

        int groupX = (int)((screenWidth - groupWidth) /2);
        int groupY = (int)((screenHeight - groupHeight) /2);

        int buttonTextureWidth = menuItemSkin.customStyles[0].hover.background.width;

        int buttonWidth = groupWidth - groupWidth / 5;
        int buttonHeight = groupHeight / (numButtons+2);

        int buttonX = (int)(groupWidth - buttonWidth) / 2;

        int buttonMargin = (groupHeight / numButtons) / 3;

        GUI.BeginGroup(new Rect(groupX, groupY, groupWidth, groupHeight));
        GUI.Box(new Rect(0, 0, groupWidth, groupHeight), "Level Select", menuItemSkin.customStyles[1]);
        
        if (GUI.Button(new Rect(buttonX, buttonMargin, buttonWidth, buttonHeight), "Start Game", menuItemSkin.customStyles[0]))
        {
            Application.LoadLevel("MainScene");
            LevelManager.Instance.Load(9);
        }
        if (GUI.Button(new Rect(buttonX, (2*buttonMargin)+buttonHeight, buttonWidth, buttonHeight), "Options", menuItemSkin.customStyles[0]))
        {
            
            LevelManager.Instance.Load(8);
            Application.LoadLevel("MainScene");
            
        }
        if (GUI.Button(new Rect(buttonX, (3*buttonMargin) + (2*buttonHeight) , buttonWidth, buttonHeight), "Exit Game", menuItemSkin.customStyles[0]))
        {
            Application.Quit();
        }

        
        GUI.EndGroup();
        //GUI.DrawTexture(new Rect((Screen.width-(Screen.width/6))/2, Screen.height-(Screen.height/16)-(Screen.height/12), Screen.width/6, Screen.height/12), surewashLogo);

       // GUI.Label(new Rect(groupX, groupY, 100, 50), "Xwdwdawwwwwwwwwwwwwwwwwwwwwww");
    }
}

