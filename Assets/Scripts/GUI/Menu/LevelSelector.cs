using UnityEngine;
using System.Collections;
using Grouping;
using HandyGestures;
using System.Runtime.Remoting.Messaging;


public class LevelSelector : MonoBehaviour, IPan
{
    public int columnCount = 4;
    public int rowCount = 3;

    private float _actualButtonSize;
    private float _actualElemSize;
    public float buttonSize = 0.2f;
    public float elemSize = 0.2f;
    public float levelSize;

    public Texture checkTexture, lockTexture, starFullTexture, starEmptyTexture;

    int pagesCount;
    int currentPage;
    bool firstLoad;
    int levelCount;
    int playerLevel;

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

    Rect[] _stars;
    float _starSize;
    float _starBorder;
    public float starBorder = 0.1f;

    void ResetSize()
    {
        _actualButtonSize = buttonSize * Screen.height;
        _actualElemSize = elemSize * Screen.height;
        //_starSize = _actualButtonSize / 3;
        _starBorder = starBorder * _actualButtonSize;
        _starSize = (_actualButtonSize - (_starBorder * 2)) / 3;
        _stars = new Rect[3];

        _stars[0] = new Rect(_starBorder, _actualButtonSize - _starSize - _starBorder * 2, _starSize, _starSize);
        _stars[1] = _stars[0].Add(_starSize, 0, 0, 0);
        _stars[2] = _stars[1].Add(_starSize, 0, 0, 0);
        _stars[1].y += _starBorder * 0.5f;
    }

    void Start()
    {
        GroupManager.main.group["Level Select"].Add(this);
        GroupManager.main.group["Level Select"].Add(this, new GroupDelegator(null, ResetSize, null));
        GroupManager.main.group["Intro"].Add(this, new GroupDelegator(null, FadeBackToLevelSelection, null));
        //_originalButtonSize = buttonSize;
        ResetSize();

        currentX = 0;
        targetX = 0;

        currentPage = 0;

        canDrag = true;
        isHeld = false;

        currentScroll = MainMenu.ScreenScrollValue - MainMenu.ScreenScrollValue * 0.05f;
        targetScroll = 0.0f;
    }

    void OnEnable()
    {
        GameWorld.success = true;
        currentPage = LevelManager.instance.Level / (rowCount * columnCount);
        levelCount = LevelManager.instance.LevelCount;
        playerLevel = PlayerPrefs.GetInt("Level", 0);
        pagesCount =
            Mathf.Min(
                levelCount / (rowCount * columnCount),
                playerLevel / (rowCount * columnCount) + 1);

        firstLoad = true;
    }

    void Update()
    {
        if (GUIManager.ScreenResized)
        {
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
            else if (System.Math.Abs(targetX) < 0.0001f)
            {
                canDrag = true;
            }
        }

        currentX = Mathf.Lerp(currentX, targetX, Time.deltaTime * 6.0f);


        if (Mathf.Abs(targetScroll - currentScroll) < MainMenu.ScreenScrollValue * 0.05f)
        {
            if (System.Math.Abs(targetScroll - MainMenu.ScreenScrollValue) < 0.0001f)
            {
                GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];

                targetScroll = 0.0f;
            }
        }
        else if (System.Math.Abs(targetScroll) < 0.0001f && Mathf.Abs(targetScroll - currentScroll) < MainMenu.ScreenScrollValue * 0.1f)
        {
            currentX = currentScroll;
            firstLoad = false;
        }

        currentScroll = Mathf.Lerp(currentScroll, targetScroll, Time.deltaTime * 5.5f);
    }

    void OnGUI()
    {
        Vector3 position = new Vector3((currentScroll < MainMenu.ScreenScrollValue * 0.05f) ? currentX : currentScroll, 0.0f, 0.0f);
        GUI.matrix = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);

        // Levels
        float offsetX = 0.5f * Screen.width - columnCount * _actualButtonSize / 2.0f;
        float offsetY = 0.5f * Screen.height - rowCount * _actualButtonSize / 2.0f;
        //float starOffset = 0.05f * Screen.height;
        float starOffset = 0;

        int levelProgress = Mathf.Min(levelCount - 1, playerLevel);

        // intro button
        if (currentPage == 0 && 
            GUI.Button(new Rect(offsetX - _actualButtonSize, offsetY, _actualButtonSize, _actualButtonSize), "Intro", GUIManager.skin.button))
        {
            ShowIntro();

            currentScroll = MainMenu.ScreenScrollValue;
            targetScroll = 0.0f;
        }

        // level buttons
		#region Level Buttons
        for (int p = (targetScroll == 0.0f && !firstLoad) ? 0 : currentPage; p < pagesCount; ++p)
        {
            int pageStart = p * columnCount * rowCount;

            for (int i = 0; i < rowCount; ++i)
            {
                for (int j = 0; j < columnCount; ++j)
                {
                    Rect buttonRect = new Rect(
                        offsetX + (p - currentPage) * Screen.width + j * _actualButtonSize,
                        offsetY + i * (_actualButtonSize + starOffset), _actualButtonSize, _actualButtonSize);

                    int level = pageStart + i * columnCount + j;

                    if (level > levelProgress)
                    {
                        GUI.enabled = false;
                    }


                    if (GUI.enabled)
                    {
                        if (GUI.Button(buttonRect, (level + 1).ToString(), GUIManager.Style.rectButton))
                        {
                            LevelManager.instance.Level = level - 1;

                            ScreenFader.QueueEvent(BackgroundRenderer.instance.SetTileBackground);

                            ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate()
                            {
                                GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
                            });

                            currentScroll = MainMenu.ScreenScrollValue;
                            targetScroll = 0.0f;
                            currentPage = 0;
                        }

                        if (LevelManager.instance.scores[level] > 0)
                        {
                            int si = 0;
                            for (; si < LevelManager.instance.scores[level]; ++si)
                            {
                                GUI.DrawTexture(_stars[si].Add(buttonRect.x, buttonRect.y, 0, 0), starFullTexture);
                            }
                            for (; si < 3; ++si)
                            {
                                GUI.DrawTexture(_stars[si].Add(buttonRect.x, buttonRect.y, 0, 0), starEmptyTexture);
                            }
                        }
                    }
                    else
                    {
                        GUI.Label(buttonRect, "", GUIManager.Style.rectButton);
                        if (levelCount - 3 <= level)
                        {
                            GUI.color = new Color(1, 0.7f, 0.85f, 0.9f);
                        }
                        GUI.DrawTexture(buttonRect, lockTexture);
                        GUI.color = Color.white;
                    }
                }
            }
        }
        GUI.enabled = true;
		#endregion


        // epilogue button
		#region Epilogue
        if (currentPage == 1 && playerLevel >= levelCount && 
            GUI.Button(new Rect(Screen.width - offsetX, Screen.height - offsetY - _actualButtonSize, _actualButtonSize, _actualButtonSize), "Epilogue", GUIManager.skin.button))
        {
            ShowEpilogue();

            currentScroll = MainMenu.ScreenScrollValue;
            targetScroll = 0.0f;
        }
		#endregion

        GUI.matrix = Matrix4x4.TRS(new Vector3(-currentScroll, 0.0f, 0.0f), Quaternion.identity, Vector3.one);

		#region Back button
        // Back
        if (GUI.Button(
            new Rect(GUIManager.OffsetX() * 2.0f,
                Screen.height - _actualButtonSize / 2 - GUIManager.OffsetY() * 2.0f,
                _actualButtonSize / 2, _actualButtonSize / 2), "Back", GUIManager.Style.back))
        {
            targetScroll = MainMenu.ScreenScrollValue;
            AudioManager.PlaySFX("Menu Prev");
        }
		#endregion

        GUI.matrix = Matrix4x4.TRS(new Vector3(0.0f, -currentScroll, 0.0f), Quaternion.identity, Vector3.one);

		#region Scoring

        if (LevelManager.TotalScore > 0)
        {
			Rect textRec = new Rect(offsetX, 0, Screen.width-offsetX * 2, offsetY);
			GUIManager.Style.scores.alignment = TextAnchor.MiddleCenter;
			GUI.Label(textRec, "Score: " + LevelManager.TotalScore, GUIManager.Style.scores);

			/*string score = "Score: " + LevelManager.TotalScore;
			string starScore = LevelManager.TotalStars + "/" + LevelManager.instance.LevelCount * 3;
			var scoreStyle = GUIManager.Style.scores;
			scoreStyle.alignment = TextAnchor.MiddleLeft;
			Rect textRec = new Rect(offsetX, 0, Screen.width-offsetX * 2, offsetY);
			GUI.Label(textRec, score, scoreStyle);
			scoreStyle.alignment = TextAnchor.MiddleRight;
			GUI.Label(textRec, starScore, scoreStyle);
			var scoreSize = scoreStyle.CalcSize(new GUIContent(starScore));
			float sSize = 0.1f * Screen.height;
			GUI.DrawTexture(
				new Rect(textRec.xMax - scoreSize.x - sSize, (offsetY - sSize) * 0.5f, sSize, sSize),
				starFullTexture);*/
        }

		#endregion

        GUI.matrix = Matrix4x4.TRS(new Vector3(0.0f, currentScroll, 0.0f), Quaternion.identity, Vector3.one);

        if ((currentPage > 0) && GUI.Button(new Rect(Screen.width / 2.0f - _actualElemSize, Screen.height - _actualElemSize / 2 - GUIManager.OffsetY() * 2.0f, _actualElemSize, _actualElemSize / 2), '\u25C0'.ToString(), GUIManager.Style.overMessage))
        {
            ToPreviousPage();
        }
        if ((currentPage < pagesCount - 1) && GUI.Button(new Rect(Screen.width / 2.0f, Screen.height - _actualElemSize / 2 - GUIManager.OffsetY() * 2.0f, _actualElemSize, _actualElemSize / 2), '\u25B6'.ToString(), GUIManager.Style.overMessage))
        {
            ToNextPage();
        }

        GUI.Label(new Rect(Screen.width / 2.0f - _actualElemSize / 2, Screen.height - _actualElemSize / 2 - GUIManager.OffsetY() * 2.0f, _actualElemSize, _actualElemSize / 2), (currentPage + 1) + " / " + pagesCount, GUIManager.Style.overMessage);
    }

    void ShowIntro()
    {
        ScreenFader.QueueEvent(BackgroundRenderer.instance.SetTileBackground);
        if (LevelManager.instance.Level != -1)
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

    void ShowEpilogue()
    {
        ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate()
            {
                FindObjectOfType<AudioMenu>().menuMain.volume = 0.0f;
                FindObjectOfType<AudioMenu>().menuLevel.volume = 0.0f;
                GameWorld.success = false;
                GroupManager.main.activeGroup = GroupManager.main.group["Level Over"];

                ScreenFader.StartFade(Color.black, Color.clear, 1.0f, delegate()
                {
                    //TODO: Fade away from dialogue
                    DialogueManager.DialogueComplete = () =>
                    {
                        GroupManager.main.activeGroup = GroupManager.main.group["Epilogue"];
                    };

                    DialogueManager.ActivateDialogue(
                        DialogueManager.instance.defaultMap["Epilogue"]);

                });
            });
    }

    void FadeBackToLevelSelection()
    {
        //Disclaimer: This is Alper's code. Shame on him.
        // This indeed is MY code, and I'm PROUD of it. ^Alper
        if (LevelManager.instance.Level != -1)
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
                                ScreenFader.QueueEvent(BackgroundRenderer.instance.SetSunBackground);
                                ScreenFader.StartFade(Color.clear, Color.black, 0.75f, delegate()
                                {
                                    ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
                                    {
                                        AudioManager.PlaySFX("Menu Prev");

                                        GroupManager.main.activeGroup = GroupManager.main.group["Level Select"];
                                    });
                                });
                            };

                            DialogueManager.ActivateDialogue(DialogueManager.instance.defaultMap["Intro2"]);
                            //Debug.Log("Dia1");
                            //GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
                        });
                    });
                };
                //Debug.Log("Dia2");
                //GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];
                DialogueManager.ActivateDialogue(DialogueManager.instance.defaultMap["Intro1"]);
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
