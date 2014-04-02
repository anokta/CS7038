using UnityEngine;
using System.Collections;
using Grouping;

public class AudioManager : MonoBehaviour
{
    public AudioSource menuMain, menuLevel, background, over;
    float menuMainVolume, menuLevelVolume, backgroundVolume, overVolume;

    public AudioSource menuNext, menuPrev, levelSwipe;
    public AudioSource collectSfx, pushSfx, push2Sfx, trolleyLoopSfx, doorSfx, fountainSfx, fountainLoopSfx, leverSfx, mirrorSfx, treatedSfx, laserSfx, explosionSfx;

    private static AudioManager instance;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Main Menu"].Add(this, new GroupDelegator(null, GameMenu, null));
        GroupManager.main.group["Level Select"].Add(this, new GroupDelegator(null, LevelSelect, null));
        GroupManager.main.group["Level Start"].Add(this, new GroupDelegator(null, LevelStart, null));
        GroupManager.main.group["Level Over"].Add(this, new GroupDelegator(null, LevelOver, null));

        menuMain.volume = 0.0f;
        menuLevel.volume = 0.0f;
        background.volume = 0.0f;
        over.volume = 0.0f;

        menuMain.Play();
        menuLevel.Play();
        background.Play();
        over.Play();
    }

    // Update is called once per frame
    void Update()
    {
        bool fadeOut = GroupManager.main.activeGroup == GroupManager.main.group["Fading"];

        if (!fadeOut)
        {
            if (menuMain.volume != menuMainVolume)
                menuMain.volume = Mathf.Lerp(menuMain.volume, menuMainVolume, Time.deltaTime * 4); 
            if (menuLevel.volume != menuLevelVolume)
                menuLevel.volume = Mathf.Lerp(menuLevel.volume, menuLevelVolume, Time.deltaTime * 2);
            if (background.volume != backgroundVolume)
                background.volume = Mathf.Lerp(background.volume, backgroundVolume, Time.deltaTime * 2);
            if (over.volume != overVolume)
                over.volume = Mathf.Lerp(over.volume, overVolume, Time.deltaTime * 2);
        }
        else
        {
            if (menuMain.volume != 0.0f)
                menuMain.volume = Mathf.Lerp(menuMain.volume, 0.0f, Time.deltaTime * 2);
            if (menuLevel.volume != 0.0f)
                menuLevel.volume = Mathf.Lerp(menuLevel.volume, 0.0f, Time.deltaTime * 2);
            if (background.volume != 0.0f)
                background.volume = Mathf.Lerp(background.volume, 0.0f, Time.deltaTime);
            if (over.volume != 0.0f)
                over.volume = Mathf.Lerp(over.volume, 0.0f, Time.deltaTime);
        }
    }

    public void PlaySFX(string type)
    {
        switch (type)
        {
            case "Push Crate":
                pushSfx.pitch += Random.Range(-0.05f, 0.05f);
                pushSfx.Play();
                break;

            case "Push Trolley":
                push2Sfx.pitch += Random.Range(-0.05f, 0.05f);
                push2Sfx.Play();
                break;

            case "Loop Trolley":
                trolleyLoopSfx.pitch = Random.Range(0.995f, 1.005f);
                if (!trolleyLoopSfx.isPlaying)
                    trolleyLoopSfx.Play();
                break;

            case "Collect":
                collectSfx.pitch += Random.Range(0.0f, 0.01f);
                collectSfx.Play();
                break;

            case "Door":
                doorSfx.Play();
                break;

            case "Fountain":
                fountainSfx.Play();
                break;

            case "Loop Fountain":
                fountainLoopSfx.Play();
                break;

            case "Lever":
                leverSfx.Play();
                break;

            case "Mirror":
                mirrorSfx.Play();
                break;

            case "Treated":
                treatedSfx.Play();
                break;

            case "Laser Hit":
                laserSfx.Play();
                break;

            case "Menu Next":
                menuNext.Play();
                break;

            case "Menu Prev":
                menuPrev.Play();
                break;

            case "Level Swipe":
                levelSwipe.pitch += Random.Range(-0.05f, 0.05f);
                levelSwipe.Play();
                break;
        }
    }

    public static void PlaySfxDelayed(string type, float delay)
    {
        switch (type)
        {
            case "Explosion Crate":
                instance.explosionSfx.PlayScheduled(Mathf.Max(0.0f, delay));
                break;
        }
    }

    public void StopSFX(string type)
    {
        switch (type)
        {
            case "Loop Trolley":
                trolleyLoopSfx.Stop();
                break;

            case "Loop Fountain":
                fountainLoopSfx.Stop();
                break;
        }
    }

    void GameMenu()
    {
        menuMainVolume = 0.4f;
        menuLevelVolume = 0.4f;
        backgroundVolume = 0.0f;
        overVolume = 0.0f;
    }

    void LevelStart()
    {
        collectSfx.pitch = 1.0f;
        pushSfx.pitch = 1.0f;
        push2Sfx.pitch = 1.0f;


        menuMainVolume = 0.0f;
        menuLevelVolume = 0.0f;
        backgroundVolume = 0.3f;
        overVolume = 0.0f;

    }

    void LevelOver()
    {
        backgroundVolume = 0.0f;
        overVolume = 0.3f;
    }

    void LevelSelect()
    {
        menuMainVolume = 0.0f;
        menuLevelVolume = 0.4f;
    }
}
