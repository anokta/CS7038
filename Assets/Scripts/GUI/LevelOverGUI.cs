using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Grouping;

public class LevelOverGUI : MonoBehaviour
{
    private float _actualWindowSize;
    private float _actualButtonSize;

    public float windowSize = 0.55f;
    public float widthRatio = 2.1f;
	public float largeButtonSize = 0.2f;
	public float smallButtonSize = 0.15f;

    private static System.Random _rnd;

    Rect guiWindow;

    string overTitle, overMessage;
    private static Dictionary<GameWorld.LevelOverReason, string[]> overTitleSet;
    private static Dictionary<GameWorld.LevelOverReason, string[]> overMessageSet;

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
			"You go hand in hand with greatness!",
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
    }

    float windowWidth;
    float windowHeight;

	float _actualButtonSizeSmall;

    void ResetSize()
    {
		_actualButtonSize = largeButtonSize * Screen.height;
		_actualButtonSizeSmall = smallButtonSize * Screen.height;
        _actualWindowSize = windowSize * Screen.height;
        windowHeight = _actualWindowSize;
        windowWidth = windowHeight * widthRatio;
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
        if (GUIManager.ScreenResized)
        {
            ResetSize();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameWorld.success)
                LevelManager.Instance.Level--;

            FadeToMainMenu();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!GameWorld.success)
            {
                LevelManager.Instance.Level--;
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
        GUI.skin = GUIManager.GetSkin();

        var style = GUI.skin.GetStyle("over title");
        float extra = style.CalcHeight(new GUIContent(overMessage), windowWidth);
        var rectIn = (new Rect(0, 0, windowWidth, windowHeight + extra)).Centered();

        var rect = GUI.Window(1, rectIn, DoWindow, GUIContent.none, GUI.skin.GetStyle("over window"));
		float backSize = _actualButtonSizeSmall;


        var backRec = new Rect((rect.xMin - backSize * 0.25f), (rect.y - backSize * 0.25f), backSize, backSize);
        GUI.Window(2, backRec, DoMenuButtonWindow, "", GUIStyle.none);
        GUI.BringWindowToFront(2);
    }

    void DoWindow(int windowID)
    {

        GUILayout.Label(overTitle, GUI.skin.GetStyle("over title"));

        GUILayout.BeginVertical();
        //GUILayout.FlexibleSpace();
        GUILayout.Label(overMessage, GUI.skin.GetStyle("over message"));
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Twitter"))
        {
            ShareToTwitter("I, #HandyMD, just cured a patient with clean hands!", "http://handymd-game.appspot.com");
        }

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Facebook"))
        {
            ShareToFacebook("I, #HandyMD, just cured a patient with clean hands!", "http://handymd-game.appspot.com");
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Restart", GUI.skin.GetStyle("restart over"), GUILayout.Width(_actualButtonSize), GUILayout.Height(_actualButtonSize)))
        {
            LevelManager.Instance.Level--;

            FadeToLevelStart();
        }
        GUILayout.FlexibleSpace();
        if (GameWorld.success)
        {
            if (GUILayout.Button("Next Level", GUI.skin.GetStyle("continue"), GUILayout.Width(_actualButtonSize), GUILayout.Height(_actualButtonSize)))
            {
                FadeToLevelStart();
            }

            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        //GUILayout.FlexibleSpace();

        //GUILayout.BeginHorizontal();
        //GUILayout.FlexibleSpace();
    }

    void DoMenuButtonWindow(int windowID)
    {
		if (GUILayout.Button("Menu", GUI.skin.GetStyle("menu"), GUILayout.Width(_actualButtonSizeSmall), GUILayout.Height(_actualButtonSizeSmall)))
        {
            if (!GameWorld.success)
            {
                LevelManager.Instance.Level--;
            }
            FadeToMainMenu();
        }
    }

    void Enter()
    {
        ResetSize();
        var overTitles = overTitleSet[GameWorld.levelOverReason];
        var overMessages = overMessageSet[GameWorld.levelOverReason];

        overTitle = overTitles[_rnd.Next(0, overTitles.Length)];
        overMessage = overMessages[_rnd.Next(0, overMessages.Length)];

		if (GameWorld.success) {
			if (LevelManager.Instance.minScore == 0 || GameWorld.score <= LevelManager.Instance.minScore) {
				Debug.Log("Golden Soap");
			} else if (LevelManager.Instance.maxScore == 0 || GameWorld.score <= LevelManager.Instance.maxScore) {
				Debug.Log("Silver Soap");
			} else {
				Debug.Log("Bronze Soap");
			}
		}
    }

    void FadeToLevelStart()
    {
        ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate()
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Level Start"];
        });
    }

    void FadeToMainMenu()
    {
        ScreenFader.QueueEvent(BackgroundRenderer.instance.SetSunBackground);
        ScreenFader.StartFade(Color.clear, Color.black, 1.0f, delegate()
        {
            // Clear resources
            LevelManager.Instance.Clear();

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