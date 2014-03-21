using UnityEngine;
using System.Collections;
using Grouping;

public class LevelSelector : MonoBehaviour
{

    public int columnCount = 4;
    public int rowCount = 3;

    public float buttonSize = 0.2f;

    public Texture lockTexture;
    public Texture checkTexture;

    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Level Select"].Add(this);

        buttonSize *= Screen.height;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ScreenFader.FadeToState("Main Menu", 0.5f, 0.5f);
        }
    }

    void OnGUI()
    {
        GUI.skin = GUIManager.GetSkin();

        // Levels
        float offsetX = 0.5f * Screen.width - columnCount * buttonSize / 2.0f;
        float offsetY = 0.5f * Screen.height - rowCount * buttonSize / 2.0f;

        if (GUI.Button(new Rect(offsetX - buttonSize, offsetY, buttonSize, buttonSize), "Intro"))
        {
            ShowIntro();
        }

        int levelProgress = PlayerPrefs.GetInt("Level", 0);
        for (int i = 0; i < rowCount; ++i)
        {
            for (int j = 0; j < columnCount; ++j)
            {
                Rect buttonRect = new Rect(offsetX + j * buttonSize, offsetY + i * buttonSize, buttonSize, buttonSize);
                bool checkmark = false;
                int level = i * columnCount + j;

                if (level < levelProgress)
                {
                    checkmark = true;
                }
                else if (level > levelProgress)
                {
                    GUI.enabled = false;
                }


                if (GUI.enabled)
                {
                    if (GUI.Button(buttonRect, (level + 1).ToString(), GUI.skin.GetStyle("rect button")))
                    {
                        LevelManager.Instance.Level = level - 1;

                        ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate()
                        {
                            GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
                        });
                    }
                    if (checkmark)
                    {
                        GUI.DrawTexture(buttonRect, checkTexture);
                    }
                }
                else
                {
                    GUI.Button(buttonRect, "", GUI.skin.GetStyle("rect button"));
                    GUI.DrawTexture(buttonRect, lockTexture);
                }
            }
        }

        GUI.enabled = true;


        // Back
        if (GUI.Button(new Rect(GUIManager.OffsetX() * 2.0f, Screen.height - buttonSize / 2 - GUIManager.OffsetY() * 2.0f, buttonSize / 2, buttonSize / 2), "Back", GUI.skin.GetStyle("back")))
        {
            ScreenFader.FadeToState("Main Menu", 0.5f, 0.5f);
        }
    }

    void ShowIntro()
    {
        DialogueManager.CurrentDialogue = -1;
        ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate()
        {
            ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
            {
                DialogueManager.DialogueComplete = delegate()
                {
                    ScreenFader.FadeToState("Level Select", 1.0f, 0.5f);
                };
                GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
            });
        });
    }
}
