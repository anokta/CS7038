using UnityEngine;
using System.Collections;
using Grouping;

public class AudioManager : MonoBehaviour
{
    public AudioSource menu, background, over;
    float menuVolume, backgroundVolume, overVolume;

    public AudioSource collectSfx, pushSfx, push2Sfx, trolleyLoopSfx, doorSfx, fountainSfx, fountainLoopSfx, leverSfx, mirrorSfx, treatedSfx, laserSfx, explosionSfx;

    private static AudioManager instance;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        GroupManager.main.group["Menu"].Add(this, new GroupDelegator(null, GameMenu, null));
        GroupManager.main.group["Level Start"].Add(this, new GroupDelegator(null, LevelStart, null));
        GroupManager.main.group["Level Over"].Add(this, new GroupDelegator(null, LevelOver, null));

        menu.volume = 0.0f;
        background.volume = 0.0f;
        over.volume = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        bool fadeOut = GroupManager.main.activeGroup == GroupManager.main.group["Fading"];

        if (!fadeOut)
        {
            if (menu.volume != menuVolume)
                menu.volume = Mathf.Lerp(menu.volume, menuVolume, Time.deltaTime * 2);
            if (background.volume != backgroundVolume)
                background.volume = Mathf.Lerp(background.volume, backgroundVolume, Time.deltaTime * 2);
            if (over.volume != overVolume)
                over.volume = Mathf.Lerp(over.volume, overVolume, Time.deltaTime * 2);
        }
        else
        {
            if (menu.volume != menuVolume)
                menu.volume = Mathf.Lerp(menu.volume, 0.0f, Time.deltaTime);
            if (background.volume != backgroundVolume)
                background.volume = Mathf.Lerp(background.volume, 0.0f, Time.deltaTime);
            if (over.volume != overVolume)
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
        menuVolume = 0.3f;
        backgroundVolume = 0.0f;
        overVolume = 0.0f;

        menu.Play();

        background.Play();
        over.Play();
    }

    void LevelStart()
    {
        collectSfx.pitch = 1.0f;
        pushSfx.pitch = 1.0f;
        push2Sfx.pitch = 1.0f;


        menuVolume = 0.0f;
        backgroundVolume = 0.3f;
        overVolume = 0.0f;

    }

    void LevelOver()
    {
        backgroundVolume = 0.0f;
        overVolume = 0.3f;
    }
}
