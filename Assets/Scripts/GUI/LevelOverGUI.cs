using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Grouping;

public class LevelOverGUI : MonoBehaviour
{
    public float windowSize = 0.9f;
    public float buttonSize = 0.2f;

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
			"I’ve got to hand it to you..."
        };
        overMessageSet[GameWorld.LevelOverReason.Success] = new[]
        {
			"Excellent work! Mankind will forever be grateful of your honorable exploits.",
			"The patient you saved will one day discover a cure for cancer, diabetes, pessimism and dislocative shoulder disorder.",
			"You deserve a handful of medals for bravery beyond the call of duty.",
            "You are a brilliant doctor, well deserving your M.D status.",
            "The Handurian Flu trembles and fears at your sight. It has no chance against your mighty ways!",
            "You get an A for style, because there is no letter that precedes A in the Latin alphabet.",
			"If you assembled an army from all the people you have saved, you’d be able to overthrow the government. But you don't do that, because you're a man of science.",
            "The patient you saved will one day become a charismatic leader who will lead Mankind into victory against the imminent alien invasion.",
            "The patient you saved will one day invent a time machine, which he will use to go back in time and save your life, thereby ensuring the streamlined continuity of the universe and preventing a time-paradox from causing it to implode."
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
			"All forest fires start with hands that aren't clean at the time of immolation.",
			"80% of all infectious diseases are transmitted by touch. The Handurian Flu is one of them.",
			"Hand cleaning might not kill all viruses, but it will dilute them to a point below viral threshold.",
			"The thumb, fingertips and the area between fingers are most often missed when cleaning hands. Be meticulous!",
			"Cleaning your hands will also make them smell better, but I guess it's too late now.",
			"Soap and water are your friends. Don't forget your friends."
		};

        overTitleSet[GameWorld.LevelOverReason.LaserKilledPlayer] = new[]
        {
            "Hand on a minute.",
			"Zapadabadoo!",
            "Care the Death Ray!"
        };
        overMessageSet[GameWorld.LevelOverReason.LaserKilledPlayer] = new[]
        {
			"Your organs have been vaporized by the Death Ray. But do not worry, they will be restored thanks to our advanced cloning technology.",
			"\"Step into the light\" is not always meant to be taken literally.",
			"Lasers can be just as lethal as they can be handy.",
			"Where did you go? Oh. I see."
		};

        overTitleSet[GameWorld.LevelOverReason.LaserKilledPatient] = new[]
        {
			"Hand on a minute.",
			"Zapadabadoo!",
			"Care the Death Ray!"
        };
        overMessageSet[GameWorld.LevelOverReason.LaserKilledPatient] = new[]
        {
			"The Hippocratic Oath might not make mention of lasers, but I'm pretty sure you shouldn't vaporize your patients.",
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
			"You just blew it."
		};

        overTitleSet[GameWorld.LevelOverReason.ExplosionKilledPatient] = new[]
        {
			"Kaboom!",
			"Explosive circumstances"
        };
        overMessageSet[GameWorld.LevelOverReason.ExplosionKilledPatient] = new[]
        {
			"The Hippocratic Oath might not make mention of lasers, but I'm pretty sure you shouldn't blow up your patients.",
			"Where did he go? Oh. I see.",
			"You just blew it.",
			"Your M.D. status has been revoked."
		};
    }

    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Level Over"].Add(this);
        GroupManager.main.group["Level Over"].Add(this, new GroupDelegator(null, Enter, null));

        buttonSize *= Screen.height;
        windowSize *= Screen.height;
        guiWindow = new Rect(Screen.width / 2.0f - windowSize / 2.0f, Screen.height / 2.0f - windowSize / 2.0f, windowSize, windowSize);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameWorld.success)
                LevelManager.Instance.Level--;

            FadeToMainMenu();
        }
    }

    // Update is called once per frame
    void OnGUI()
    {
        GUI.skin = GUIManager.GetSkin();

        GUI.Window(2, guiWindow, DoGameOverWindow, GUIContent.none, GUI.skin.GetStyle("over window"));
    }

    void DoGameOverWindow(int windowID)
    {
        // Info
        GUILayout.BeginVertical();
        GUILayout.Label(overTitle, GUI.skin.GetStyle("over title"));
        GUILayout.Label(overMessage, GUI.skin.GetStyle("over message"));

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        // Restart
        if (GUILayout.Button("Restart", GUI.skin.GetStyle("restart over"), GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
        {
            LevelManager.Instance.Level--;

            FadeToLevelStart();
        }
        GUILayout.FlexibleSpace();
        // Next Level
        if (GameWorld.success)
        {
            if (GUILayout.Button("Next Level", GUI.skin.GetStyle("continue"), GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
            {
                FadeToLevelStart();
            }

            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        // Go Back To Menu
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Menu", GUI.skin.GetStyle("menu"), GUILayout.Width(buttonSize / 2.0f), GUILayout.Height(buttonSize / 2.0f)))
        {
            if (!GameWorld.success)
                LevelManager.Instance.Level--;

            FadeToMainMenu();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.EndVertical();
    }

    void Enter()
    {
        var overTitles = overTitleSet[GameWorld.levelOverReason];
        var overMessages = overMessageSet[GameWorld.levelOverReason];

		overTitle = overTitles[_rnd.Next(0, overTitles.Length)];
		overMessage = overMessages[_rnd.Next(0, overMessages.Length)];
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
}