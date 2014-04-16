using UnityEngine;
using System.Collections;
using Grouping;
using HandyGestures;


public class LevelSelector : MonoBehaviour, IPan
{
    public int columnCount = 4;
    public int rowCount = 3;

	private float _originalButtonSize;
    public float buttonSize = 0.2f;

    public Texture checkTexture, lockTexture;

    int pagesCount;
    int currentPage;

    float currentScroll, targetScroll;

    #region Gestures

    public float flingThreshold = 0.2f;
    float flingTimer;

    bool isHeld, canDrag;

    float currentX, targetX;

    public void OnGesturePan(PanArgs args)
    {
        switch (args.state)
        {
            case PanArgs.State.Down:
                flingTimer = Time.time;
                isHeld = true;
                break;

            case PanArgs.State.Move:
                targetX -= args.delta.x;
                break;

            case PanArgs.State.Interrupt:
            case PanArgs.State.Up:
                if (Time.time - flingTimer < flingThreshold)
                {
                    if (targetX > Screen.width * 0.2f)
                        targetX = Screen.width;
                    else if (targetX < -Screen.width * 0.2f)
                        targetX = -Screen.width;
                    else
                        targetX = 0;
                }
                else
                {
                    targetX = 0;
                }
                isHeld = false;
                break;
        }
    }

    #endregion

	void ResetSize() {
		buttonSize = _originalButtonSize * Screen.height;
	}

    void Start()
    {

        GroupManager.main.group["Level Select"].Add(this);
		GroupManager.main.group["Level Select"].Add(this, new GroupDelegator(null, ResetSize, null));
        GroupManager.main.group["Intro"].Add(this, new GroupDelegator(null, FadeBackToLevelSelection, null));
		_originalButtonSize = buttonSize;
		ResetSize();

        currentX = 0;
        targetX = 0;

        currentPage = 0;
        pagesCount = 4;

        canDrag = true;
        isHeld = false;

        currentScroll = MainMenu.ScreenScrollValue - MainMenu.ScreenScrollValue * 0.05f;
        targetScroll = 0.0f;
    }

    void OnEnable()
    {
        currentPage = LevelManager.Instance.Level / (rowCount * columnCount);
    }

    void Update()
    {
		if (GUIManager.ScreenResized) {
			ResetSize();
		}

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            targetScroll = MainMenu.ScreenScrollValue;
            AudioManager.PlaySFX("Menu Prev");
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ToNextPage();
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) 
        {
            ToPreviousPage();
        }

        if (!isHeld)
        {
            if (currentX < -Screen.width * 0.5f)
            {
                if (canDrag) ToNextPage();
            }
            else if (currentX > Screen.width * 0.5f)
            {
                if (canDrag) ToPreviousPage();
            }
            else if (targetX == 0.0f)
            {
                canDrag = true;
            }
        }

        currentX = Mathf.Lerp(currentX, targetX, Time.deltaTime * 6.0f);


        if (Mathf.Abs(targetScroll - currentScroll) < MainMenu.ScreenScrollValue * 0.05f)
        {
            if (targetScroll == MainMenu.ScreenScrollValue)
            {
                GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];

                targetScroll = 0.0f;
            }
        }
        else if (targetScroll == 0.0f && Mathf.Abs(targetScroll - currentScroll) < MainMenu.ScreenScrollValue * 0.1f)
        {
            currentX = currentScroll;
        }

        currentScroll = Mathf.Lerp(currentScroll, targetScroll, Time.deltaTime * 5.5f);
    }

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(new Vector3((currentScroll < MainMenu.ScreenScrollValue * 0.05f) ? currentX : currentScroll, 0.0f, 0.0f), Quaternion.identity, Vector3.one);

        GUI.skin = GUIManager.GetSkin();

        // Levels
        float offsetX = 0.5f * Screen.width - columnCount * buttonSize / 2.0f;
        float offsetY = 0.5f * Screen.height - rowCount * buttonSize / 2.0f;

        int levelProgress = Mathf.Min(LevelManager.Instance.LevelCount - 1, PlayerPrefs.GetInt("Level", 0));

        for (int p = (targetScroll == 0.0f) ? 0 : currentPage; p < pagesCount; ++p)
        {
            int pageStart = p * columnCount * rowCount;

            if (pageStart == 0 && GUI.Button(new Rect((p - currentPage) * Screen.width + offsetX - buttonSize, offsetY, buttonSize, buttonSize), "Intro"))
            {
                ShowIntro();

                currentScroll = MainMenu.ScreenScrollValue;
                targetScroll = 0.0f;
            }

            for (int i = 0; i < rowCount; ++i)
            {
                for (int j = 0; j < columnCount; ++j)
                {
                    Rect buttonRect = new Rect(offsetX + (p - currentPage) * Screen.width + j * buttonSize, offsetY + i * buttonSize, buttonSize, buttonSize);
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

                            currentScroll = MainMenu.ScreenScrollValue;
                            targetScroll = 0.0f;
                            currentPage = 0;
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
        }
        GUI.enabled = true;

        GUI.matrix = Matrix4x4.TRS(new Vector3(-currentScroll, 0.0f, 0.0f), Quaternion.identity, Vector3.one);

        // Back
        if (GUI.Button(new Rect(GUIManager.OffsetX() * 2.0f, Screen.height - buttonSize / 2 - GUIManager.OffsetY() * 2.0f, buttonSize / 2, buttonSize / 2), "Back", GUI.skin.GetStyle("back")))
        {
            targetScroll = MainMenu.ScreenScrollValue;
            AudioManager.PlaySFX("Menu Prev");
        }

        GUI.matrix = Matrix4x4.TRS(new Vector3(0.0f, currentScroll, 0.0f), Quaternion.identity, Vector3.one);

        if ((currentPage > 0) && GUI.Button(new Rect(Screen.width / 2.0f - buttonSize, Screen.height - buttonSize / 2 - GUIManager.OffsetY() * 2.0f, buttonSize, buttonSize / 2), "<-", GUI.skin.GetStyle("over message")))
        {
            ToPreviousPage();
        }
        if ((currentPage < pagesCount - 1) && GUI.Button(new Rect(Screen.width / 2.0f, Screen.height - buttonSize / 2 - GUIManager.OffsetY() * 2.0f, buttonSize, buttonSize / 2), "->", GUI.skin.GetStyle("over message")))
        {
            ToNextPage();
        }
        
        GUI.Label(new Rect(Screen.width / 2.0f - buttonSize / 2, Screen.height - buttonSize / 2 - GUIManager.OffsetY() * 2.0f, buttonSize, buttonSize / 2), (currentPage + 1) + " / " + pagesCount, GUI.skin.GetStyle("over message"));
    }

    void ShowIntro()
    {
        DialogueManager.CurrentDialogue = -1;

        if (LevelManager.Instance.Level != -1)
        {
            ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate()
            {
                GroupManager.main.activeGroup = GroupManager.main.group["Intro"];
            });
        }
        else
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Intro"];
        }
    }

    void FadeBackToLevelSelection()
    {
        if (LevelManager.Instance.Level != -1)
        {
            ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
            {
                DialogueManager.DialogueComplete = delegate()
                {
                    ScreenFader.StartFade(Color.clear, Color.black, 0.75f, delegate()
                    {
                        ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
                        {
                            DialogueManager.DialogueComplete = delegate()
                            {
                                ScreenFader.StartFade(Color.clear, Color.black, 0.75f, delegate()
                                {
                                    ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
                                    {
                                        AudioManager.PlaySFX("Menu Prev");

                                        GroupManager.main.activeGroup = GroupManager.main.group["Level Select"];
                                    });
                                });
                            };

                            GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
                        });
                    });
                };

                GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
            });
        }
    }

    void ToPreviousPage()
    {
        canDrag = false;

        if (currentPage > 0)
        {
            currentPage = currentPage - 1;
            currentX = -Screen.width + currentX;

            AudioManager.PlaySFX("Level Swipe");
        }

        targetX = 0;
    }

    void ToNextPage()
    {
        canDrag = false;

        if (currentPage < pagesCount - 1)
        {
            currentPage = currentPage + 1;
            currentX = Screen.width + currentX;

            AudioManager.PlaySFX("Level Swipe");
        }

        targetX = 0;
    }
}
