using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Grouping;

using LOR = GameWorld.LevelOverReason;

public class LevelOverGUI : MonoBehaviour
{
    private float _actualWindowSize;
    private float _actualButtonSize;
	private float _actualSocialSize;
	private float _actualStarSize;

    public float windowSize = 0.55f;
	// public float widthRatio = 2.1f;
	public float heightToWidth = 1.155f;
	public float largeButtonSize = 0.2f;
	public float smallButtonSize = 0.15f;
	public float socialButtonSize = 0.15f;
	public float starSize = 0.1f;

    private static System.Random _rnd;

	Timer starTimer;

    Rect guiWindow;

    string overTitle, overMessage;
    private static Dictionary<GameWorld.LevelOverReason, string[]> overTitleSet;
    private static Dictionary<GameWorld.LevelOverReason, string[]> overMessageSet;
	private static Dictionary<GameWorld.LevelOverReason, string> twitterMessages;

	public LevelOverGUI() {
		starTimer = new Timer(
			0.3f, () => {
				if (visibleScore == currentScore) {
					starTimer.Stop();
				}
				else {
					++visibleScore;
				}
		});
		starTimer.repeating = true;
	}

    static LevelOverGUI()
    {
        _rnd = new System.Random();
        overTitleSet = new Dictionary<GameWorld.LevelOverReason, string[]>();
        overMessageSet = new Dictionary<GameWorld.LevelOverReason, string[]>();

        overTitleSet[GameWorld.LevelOverReason.Success] = new[]
        {
            "Handsomely done!",
            "High five!",
            "Hands down, you rock!",
            "Well handled!",
            "Cleanly done!",
			"You go hand in hand with victory!",
			"I’ve got to hand it to you..."
        };
        overMessageSet[GameWorld.LevelOverReason.Success] = new[]
        {
			"Excellent work! Mankind will forever be grateful for your honorable exploits.",
			"The patient you saved will one day discover a cure for cancer, diabetes, pessimism and dislocative shoulder disorder.",
			"You deserve a handful of medals for bravery beyond the call of duty.",
            "You are a brilliant doctor, well deserving your M.D status.",
            "The Handurian Flu trembles and fears at your sight. It has no chance against your mighty ways!",
            "You get an A for style, because there is no letter that precedes A in the Latin alphabet.",
			"If you assembled an army from all the people you have saved, you’d be able to overthrow the government. But you don't do that, because you're a man of science.",
            "The patient you saved will one day become a charismatic leader who will lead Mankind into victory against the imminent alien invasion."
            //"The patient you saved will one day invent a time machine, which he will use to go back in time and save your life, thereby ensuring the streamlined continuity of the universe and preventing a time-paradox from causing it to implode."
        };

        overTitleSet[GameWorld.LevelOverReason.PatientInfected] = new[]
        {
            "Hand on a minute.",
            "Caught you red handed!",
			"This is getting out of hand!"
        };
        overMessageSet[GameWorld.LevelOverReason.PatientInfected] = new[]
        {
            "I saw what you did there, treating that patient with filthy hands! That was a bad idea.",
            "In retrospect, treating a patient with filthy hands was more harmful than helpful",
            "I am appalled at your unsanitary medical practices. Are you a real doctor?"
        };

        overTitleSet[GameWorld.LevelOverReason.PlayerInfected] = new[]
        {
			"Hand on a minute.",
			"Caught you red handed!",
			"This is getting out of hand!",
			"Wash your hands!",
			"You couldn't handle the flu."
        };
        overMessageSet[GameWorld.LevelOverReason.PlayerInfected] = new[]
        {
			"You were infected by the Handurian flu. Why didn't you just wash your hands?",
			"Over 90% of the people who don't wash their hands eventually die at some point.",
			//"All forest fires start with hands that aren't clean at the time of immolation.",
			"80% of all infectious diseases are transmitted by touch. The Handurian Flu is one of them.",
			"Hand cleaning might not kill all viruses, but it will dilute them to a point below viral threshold.",
			"The thumb, fingertips and the area between fingers are most often missed when cleaning hands. Be meticulous!",
			"Cleaning your hands will also make them smell better.",
			"Soap and water are your friends. Don't forget your friends."
		};

        overTitleSet[GameWorld.LevelOverReason.Squashed] = new[] {
			"Squashed!",
		};
        overMessageSet[GameWorld.LevelOverReason.Squashed] = new[] {
			"It's actually pretty hard to achieve this. Good job, but you still lose."
		};

        overTitleSet[GameWorld.LevelOverReason.LaserKilledPlayer] = new[]
        {
            "Hand on a minute.",
			"Zapadabadoo!",
            "Care the Death Ray!"
        };
        overMessageSet[GameWorld.LevelOverReason.LaserKilledPlayer] = new[]
        {
			"Your organs have been disintegrated by the Death Ray. But do not worry, they will be restored thanks to our advanced cloning technology.",
			"\"Step into the light\" is not always meant to be taken literally.",
			"Lasers can be just as lethal as they can be handy.",
			"Where did you go? Oh. I see.",
			"In times like this, you must learn to pick up the pieces and move on."
		};

        overTitleSet[GameWorld.LevelOverReason.LaserKilledPatient] = new[]
        {
			"Hand on a minute.",
			"Zapadabadoo!",
			"Care the Death Ray!"
        };
        overMessageSet[GameWorld.LevelOverReason.LaserKilledPatient] = new[]
        {
			"The Hippocratic Oath might not make mention of lasers, but I'm pretty sure you shouldn't disintegrate your patients.",
			"Where did he go? Oh. I see.",
			"Lasers can be just as lethal as they can be handy.",
			"Your M.D. status has been revoked."
		};

        overTitleSet[GameWorld.LevelOverReason.ExplosionKilledPlayer] = new[]
        {
			"Kaboom!",
			"Explosive circumstances"
        };
        overMessageSet[GameWorld.LevelOverReason.ExplosionKilledPlayer] = new[]
        {
			"Where did you go? Oh. I see.",
			"You just blew it.",
			"In times like this, you must learn to pick up the pieces and move on."
		};

        overTitleSet[GameWorld.LevelOverReason.ExplosionKilledPatient] = new[]
        {
			"Kaboom!",
			"Explosive circumstances"
        };
        overMessageSet[GameWorld.LevelOverReason.ExplosionKilledPatient] = new[]
        {
			"The Hippocratic Oath might not make mention of explosions, but I'm pretty sure you shouldn't blow up your patients.",
			"Where did he go? Oh. I see.",
			"You just blew it.",
			"Your M.D. status has been revoked."
		};

		twitterMessages = new Dictionary<GameWorld.LevelOverReason, string>() {
			{LOR.ExplosionKilledPatient, "I blew up a patient in #HandyMD!"},
			{LOR.ExplosionKilledPlayer, "I got blown up in #HandyMD!"},
			{LOR.LaserKilledPatient, "I disintegrated a patient with a laser in #HandyMD!"},
			{LOR.LaserKilledPlayer, "I was disintegrated by a laser in #HandyMD!"},
			{LOR.PatientInfected, "I am a horrible doctor, because I just treated a patient with filthy hands in #HandyMD!"},
			{LOR.PlayerInfected, "I died in #HandyMD because I neglected to wash my hands!"},
			{LOR.Squashed, "I was squashed by a heavy door in #HandyMD!"},
			//{LOR.Success, "I successfully finished a level in #HandyMD!},
		};
			
    }

    float windowWidth;
    float windowHeight;

	float _actualButtonSizeSmall;

    void ResetSize()
    {
		_actualButtonSize = largeButtonSize * Screen.height;
		_actualButtonSizeSmall = smallButtonSize * Screen.height;
        _actualWindowSize = windowSize * Screen.height;
		_actualSocialSize = socialButtonSize * Screen.height;
		_actualStarSize = starSize * Screen.height;
        windowHeight = _actualWindowSize;
		//windowWidth = windowHeight * widthRatio;
		windowWidth = heightToWidth * Screen.height;
        guiWindow = new Rect(Screen.width / 2.0f - windowWidth / 2.0f, Screen.height / 2.0f - windowHeight / 2.0f, windowWidth, windowHeight);
    }

    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Level Over"].Add(this);
        GroupManager.main.group["Level Over"].Add(this, new GroupDelegator(null, Enter, null));
        ResetSize();
    }

    void OnEnable()
    {
        //	ResetSize();
    }

    void Update()
    {
		if (GameWorld.success) {
			starTimer.Update();
		}
        if (GUIManager.ScreenResized)
        {
            ResetSize();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameWorld.success)
                LevelManager.instance.Level--;

            FadeToMainMenu();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!GameWorld.success)
            {
                LevelManager.instance.Level--;
                FadeToLevelStart();
            }
            else
            {
                FadeToLevelStart();
            }
        }
    }

    // Update is called once per frame
    void OnGUI()
    {
      // GUI.skin = GUIManager.GetSkin();

		//Note #1: The current window graphic has an inner margin of 3. Change accordingly if the graphic changes.
		//Note #2: The current window graphic has an inner border of 4. ^^^^^^^
		int innerMargin = 3;
		int innerBorder = 4;
		float innerHalf = 3.5f; //3 + 4 / 2

		var style = GUIManager.Style.overMessage;
		float extra = style.CalcHeight(new GUIContent(overMessage), windowWidth -
			(GUIManager.Style.overWindow.padding.left +  GUIManager.Style.overWindow.padding.right));
		if (GameWorld.success) {
			extra += _actualStarSize + GUIManager.Style.starFull.margin.vertical;
			extra -= (GUIManager.Style.overTitle.padding.bottom - GUIManager.Style.overSuccess.padding.bottom);
		}
        var rectIn = (new Rect(0, 0, windowWidth, windowHeight + extra)).Centered();

		var rect = GUI.Window(1, rectIn, DoWindow, GUIContent.none, GUIManager.Style.overWindow);
		float backSize = _actualButtonSizeSmall;


		var backRec = new Rect((rect.xMin - backSize * 0.4f + innerHalf ), (rect.y - backSize * 0.4f + innerHalf), backSize, backSize);
        GUI.Window(2, backRec, DoMenuButtonWindow, "", GUIStyle.none);


		/*var socialRec = 
			(new Rect(
				rect.xMax - _actualSocialSize * 3,
				rect.y - _actualSocialSize * 0.6f,
				_actualSocialSize * 2.5f, _actualSocialSize)).Rounded();*/
		var socialRec = new Rect(
			rect.x - _actualSocialSize * 1.25f,
			                0,
			                _actualSocialSize * 2,
			_actualSocialSize * 2.25f);
		//socialRec.x = socialRec.Centered().x;
		socialRec.y = socialRec.Centered().y;
		GUI.Window(3, socialRec, DoSocialWindow, "", GUIStyle.none);

		GUI.BringWindowToFront(2);
		GUI.BringWindowToFront(3);

	}

	void DoStars() {
		if (GameWorld.success) {
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			int i = 0;
			for (; i < visibleScore; ++i) {
				GUILayout.Label("", GUIManager.Style.starFull,
					GUILayout.Width(_actualStarSize), GUILayout.Height(_actualStarSize));
			}
			for (; i < 3; ++i) {
				GUILayout.Label("", GUIManager.Style.starEmpty,
					GUILayout.Width(_actualStarSize), GUILayout.Height(_actualStarSize));
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
	}

    void DoWindow(int windowID)
    {
		GUILayout.BeginVertical();
		if (GameWorld.success) {
			GUILayout.Label(overTitle, GUIManager.Style.overSuccess);
		} else {
			GUILayout.Label(overTitle, GUIManager.Style.overTitle);
		}

		DoStars();
        //GUILayout.FlexibleSpace();
		GUILayout.Label(overMessage, GUIManager.Style.overMessage);

		GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
		{
			GUILayout.FlexibleSpace();
			if (GameWorld.success) {
				GUILayout.FlexibleSpace();
				GUILayout.FlexibleSpace();
				GUILayout.FlexibleSpace();
			}
			if (GUILayout.Button("Restart", GUIManager.Style.restartOver, GUILayout.Width(_actualButtonSize), GUILayout.Height(_actualButtonSize))) {
				LevelManager.instance.Level--;

				FadeToLevelStart();
			}
			GUILayout.FlexibleSpace();
			if (GameWorld.success) {
				if (GUILayout.Button("Next Level", GUIManager.Style.continueButton, GUILayout.Width(_actualButtonSize), GUILayout.Height(_actualButtonSize))) {
					FadeToLevelStart();
				}

				GUILayout.FlexibleSpace();
				GUILayout.FlexibleSpace();
				GUILayout.FlexibleSpace();
				GUILayout.FlexibleSpace();
			}
		}
        GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();	
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        //GUILayout.FlexibleSpace();

        //GUILayout.BeginHorizontal();
        //GUILayout.FlexibleSpace();
    }

    void DoMenuButtonWindow(int windowID)
    {
		if (GUILayout.Button("Menu", GUIManager.Style.menu, GUILayout.Width(_actualButtonSizeSmall), GUILayout.Height(_actualButtonSizeSmall)))
        {
            if (!GameWorld.success)
            {
                LevelManager.instance.Level--;
            }
            FadeToMainMenu();
        }
    }

	int currentScore = 0;
	int visibleScore = 0;

    void Enter()
    {
        ResetSize();
        var overTitles = overTitleSet[GameWorld.levelOverReason];
        var overMessages = overMessageSet[GameWorld.levelOverReason];

        overTitle = overTitles[_rnd.Next(0, overTitles.Length)];
        overMessage = overMessages[_rnd.Next(0, overMessages.Length)];

		visibleScore = 0;

		if (GameWorld.success) {
			starTimer.Reset();
			if (LevelManager.instance.minScore == 0 || GameWorld.score <= LevelManager.instance.minScore) {
				currentScore = 3;
			} else if (LevelManager.instance.maxScore == 0 || GameWorld.score <= LevelManager.instance.maxScore) {
				currentScore = 2;
			} else {
				currentScore = 1;
			}
			//Debug.Log(LevelManager.instance.Level);
			LevelManager.SetScore(LevelManager.instance.Level, currentScore);
		} else {
			currentScore = 0;
		}
    }

    void FadeToLevelStart()
    {
        ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate()
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
        });
    }

	void DoSocialWindow(int windowID) {
		GUILayout.BeginVertical();
		{
			if (GUILayout.Button("Twitter", GUIManager.Style.twitter, GUILayout.Width(_actualSocialSize), GUILayout.Height(_actualSocialSize)))
			{
				if (GameWorld.success) {
					ShareToTwitter(
						"I finished level " + (LevelManager.instance.Level + 1).ToString() + " with " +
						currentScore.ToString() + " star" + ((currentScore == 1) ? "" : "s") + " in #HandyMD!", "http://handymd-game.appspot.com");
				} else {
					ShareToTwitter(twitterMessages[GameWorld.levelOverReason], "http://handymd-game.appspot.com");
				}
				//ShareToTwitter("I, #HandyMD, just cured a patient with clean hands!", "http://handymd-game.appspot.com");
			}
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Facebook", GUIManager.Style.facebook, GUILayout.Width(_actualSocialSize), GUILayout.Height(_actualSocialSize)))
			{
				ShareToFacebook("I, #HandyMD, just cured a patient with clean hands!", "http://handymd-game.appspot.com");
			}
		}
		GUILayout.EndVertical();
	}

    void FadeToMainMenu()
    {
        ScreenFader.QueueEvent(BackgroundRenderer.instance.SetSunBackground);
        ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate()
        {
            // Clear resources
            LevelManager.instance.Clear();

            ScreenFader.StartFade(Color.black, Color.clear, 0.5f, delegate()
            {

                GroupManager.main.activeGroup = GroupManager.main.group["Level Select"];

                AudioManager.PlaySFX("Menu Next");
            });
        });
    }

    void ShareToTwitter(string textToDisplay, string urlToDisplay)
    {
        Application.OpenURL("http://twitter.com/intent/tweet?text=" + WWW.EscapeURL(textToDisplay) + "&amp;url=" + WWW.EscapeURL(urlToDisplay) + "&amp;lang=en");
    }

    void ShareToFacebook(string textToDisplay, string urlToDisplay)
    {
        // Sample text doesn't work for now.
        Application.OpenURL("http://www.facebook.com/sharer/sharer.php?u=" + WWW.EscapeURL(urlToDisplay) + "&t=" + WWW.EscapeURL(textToDisplay));   
    }
}