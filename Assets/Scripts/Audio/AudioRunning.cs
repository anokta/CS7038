using UnityEngine;
using System.Collections;
using Grouping;

public class AudioRunning : MonoBehaviour
{
    public AudioSource background, over;
    public float volume;

    float backgroundVolume, overVolume;

    void Start()
    {
        GroupManager.main.group["Level Start"].Add(this, new GroupDelegator(null, LevelStart, null));
        GroupManager.main.group["To Level Over"].Add(this, new GroupDelegator(null, ToLevelOver, null));
        GroupManager.main.group["Level Over"].Add(this, new GroupDelegator(null, LevelOver, null));
        GroupManager.main.group["Main Menu"].Add(this, new GroupDelegator(null, Stop, null));
    }

    void Update()
    {
        bool fadeOut = GroupManager.main.activeGroup == GroupManager.main.group["Fading"];

        if (!fadeOut)
        {
            if (background.volume != backgroundVolume)
                background.volume = Mathf.Lerp(background.volume, backgroundVolume, Time.deltaTime * 4);
            if (over.volume != overVolume)
                over.volume = Mathf.Lerp(over.volume, overVolume, Time.deltaTime * 4);
        }
        else
        {
            if (background.volume != 0.0f)
                background.volume = Mathf.Lerp(background.volume, 0.0f, Time.deltaTime);
            if (over.volume != 0.0f)
                over.volume = Mathf.Lerp(over.volume, 0.0f, Time.deltaTime);
        }
    }

    void LevelStart()
    {
        if(!background.isPlaying)
            background.Play();
        if (!over.isPlaying)
            over.Play();

        backgroundVolume = volume;
        overVolume = 0.0f;
    }

    void LevelOver()
    {
        overVolume = volume;
        backgroundVolume = 0.0f;
    }

    void ToLevelOver()
    {
        backgroundVolume = volume / 3.0f;
    }

    void Stop()
    {
        background.Stop();
        over.Stop();
    }
}
