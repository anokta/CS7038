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
        Color guiColor = GUI.color;

        // Levels
        float offsetX = 0.5f * Screen.width - columnCount * buttonSize / 2.0f;
        float offsetY = 0.5f * Screen.height - rowCount * buttonSize / 2.0f;

		// GUI.color = new Color(0.75f, 0.9f, 0.75f, 1.0f);
        if (GUI.Button(new Rect(offsetX - buttonSize, offsetY, buttonSize, buttonSize), "Intro"))
        {
            ShowIntro();
        }

        GUI.color = new Color(0.75f, 0.75f, 0.75f, 1.0f);
		GUI.color = Color.white;

		Color grayCol = new Color(1f, 1f, 1f);
	
        for (int i = 0; i < rowCount; ++i)
        {
            for (int j = 0; j < columnCount; ++j)
            {
                Rect buttonRect = new Rect(offsetX + j * buttonSize, offsetY + i * buttonSize, buttonSize, buttonSize);
				bool checkmark = false;
                int level = i * columnCount + j;
				if (level == LevelManager.Instance.Level + 1) {
					GUI.color = Color.white;
				} else if (level > LevelManager.Instance.Level + 1) {
					GUI.color = grayCol;
					GUI.enabled = false;
				} else {
					checkmark = true;
				}

				if (GUI.enabled) {
					if (GUI.Button(buttonRect, (level + 1).ToString(), GUI.skin.GetStyle("rect button"))) {
						LevelManager.Instance.Level = level - 1;

						ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate() {
							GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
						});
					}
					if (checkmark) {
						GUI.color = Color.white;
						GUI.DrawTexture(buttonRect, checkTexture);
					}
				}
				else {
					GUI.Button(buttonRect, "", GUI.skin.GetStyle("rect button"));
					//GUI.enabled = true;
					//GUI.color = Color.white;
					GUI.DrawTexture(buttonRect, lockTexture);
				}
            }
        }

        GUI.color = guiColor;
        GUI.enabled = true;


        // Back
        if (GUI.Button(new Rect(25, Screen.height - buttonSize / 2 - 25, buttonSize / 2, buttonSize / 2), "Back", GUI.skin.GetStyle("back")))
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
