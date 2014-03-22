using UnityEngine;
using System.Collections;
using Grouping;
using HandyGestures;


public class LevelSelector : MonoBehaviour, IPan
{

    public int columnCount = 4;
    public int rowCount = 3;

    public float buttonSize = 0.2f;

    public Texture checkTexture, lockTexture;

    int pagesCount;
    int currentPage;

    #region Gestures

    float currentX, targetX;
    bool canDrag;

    public void OnGesturePan(PanArgs args)
    {
        switch (args.state)
        {
            case PanArgs.State.Move:
                if (canDrag)
                    targetX -= args.delta.x;
                break;

            case PanArgs.State.Interrupt:
            case PanArgs.State.Up:
                canDrag = true;
                targetX = 0;
                break;
        }
    }

    #endregion

    void Start()
    {
        GroupManager.main.group["Level Select"].Add(this);

        buttonSize *= Screen.height;

        currentX = 0;
        targetX = 0;

        currentPage = 0;
        pagesCount = 4;

        canDrag = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ScreenFader.FadeToState("Main Menu", 0.5f, 0.5f);
        }

        currentX = Mathf.Lerp(currentX, targetX, Time.deltaTime * 6.0f);

        if (targetX != 0.0f)
        {
            if (currentX < -Screen.width * 0.5f || targetX < -Screen.width * 0.6f)
            {
                ToNextPage();
            }
            else if (currentX > Screen.width * 0.5f || targetX > Screen.width * 0.6f)
            {
                ToPreviousPage();
            }
        }
    }

    void OnGUI()
    {
        Matrix4x4 guiMatrix = GUI.matrix;
        GUI.matrix *= Matrix4x4.TRS(new Vector3(currentX, 0.0f, 0.0f), Quaternion.identity, Vector3.one);

        GUI.skin = GUIManager.GetSkin();

        // Levels
        float offsetX = 0.5f * Screen.width - columnCount * buttonSize / 2.0f;
        float offsetY = 0.5f * Screen.height - rowCount * buttonSize / 2.0f;

        int pageStart = currentPage * columnCount * rowCount;
        
        if (pageStart == 0 && GUI.Button(new Rect(offsetX - buttonSize, offsetY, buttonSize, buttonSize), "Intro"))
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
                int level = pageStart + i * columnCount + j;

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

        GUI.matrix = guiMatrix;

        // Back
        if (GUI.Button(new Rect(GUIManager.OffsetX() * 2.0f, Screen.height - buttonSize / 2 - GUIManager.OffsetY() * 2.0f, buttonSize / 2, buttonSize / 2), "Back", GUI.skin.GetStyle("back")))
        {
            ScreenFader.FadeToState("Main Menu", 0.5f, 0.5f);
        }

        GUI.Label(new Rect(Screen.width / 2.0f - buttonSize / 2, Screen.height - buttonSize / 2 - GUIManager.OffsetY() * 2.0f, buttonSize, buttonSize / 2), (currentPage + 1) + " / " + pagesCount, GUI.skin.GetStyle("over message"));
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

    void ToPreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage = currentPage - 1;
            currentX = -Screen.width * 0.5f;
        }

        targetX = 0;

        canDrag = false;
    }

    void ToNextPage()
    {
        if (currentPage < pagesCount - 1)
        {
            currentPage = currentPage + 1;
            currentX = Screen.width * 0.5f;
        }

        targetX = 0;

        canDrag = false;
    }
}
