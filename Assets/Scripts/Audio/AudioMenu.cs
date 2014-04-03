using UnityEngine;
using System.Collections;
using Grouping;

public class AudioMenu : MonoBehaviour {
    
    public AudioSource menuMain, menuLevel;
    public float volume;

    float menuMainVolume, menuLevelVolume;
	
    // Use this for initialization
	void Start () {

        GroupManager.main.group["Main Menu"].Add(this);
        GroupManager.main.group["Main Menu"].Add(this, new GroupDelegator(null, MainMenu, null));
        GroupManager.main.group["Level Select"].Add(this);
        GroupManager.main.group["Level Select"].Add(this, new GroupDelegator(null, LevelSelect, null));

        GroupManager.main.group["Fading"].Add(this);
        GroupManager.main.group["Fading"].Add(this, new GroupDelegator(null, FadeOut, null));

        GroupManager.main.group["Intro"].Add(this, new GroupDelegator(null, Stop, null));
        GroupManager.main.group["Level Start"].Add(this, new GroupDelegator(null, Stop, null));
	}
	
	// Update is called once per frame
    void Update()
    {
        if (menuMain.volume != menuMainVolume)
            menuMain.volume = Mathf.Lerp(menuMain.volume, menuMainVolume, Time.deltaTime * 4);
        if (menuLevel.volume != menuLevelVolume)
            menuLevel.volume = Mathf.Lerp(menuLevel.volume, menuLevelVolume, Time.deltaTime * 2);
    }

    void MainMenu()
    {
        if (!menuLevel.isPlaying)
        {
            menuMain.Play();
            menuLevel.Play();
        }

        menuMainVolume = volume;
        menuLevelVolume = volume;
    }

    void LevelSelect()
    {
        if (!menuLevel.isPlaying)
        {
            menuMain.Play();
            menuLevel.Play();
        }

        menuMainVolume = 0.0f;
        menuLevelVolume = volume;
    }

    void FadeOut()
    {
        menuMainVolume = 0.0f;
        menuLevelVolume = 0.0f;
    }

    void Stop()
    {
        menuMain.Stop();
        menuLevel.Stop();
    }
}
