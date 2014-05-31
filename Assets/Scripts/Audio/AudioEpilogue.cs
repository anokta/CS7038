using UnityEngine;
using System.Collections;
using Grouping;

public class AudioEpilogue : MonoBehaviour
{
    public AudioSource epilogue;
    public float volume;

    float epilogueVolume;

    void Start()
    {
        GroupManager.main.group["Dialogue"].Add(this);
        GroupManager.main.group["Dialogue"].Add(this, new GroupDelegator(null, EpilogueStart, null));

        GroupManager.main.group["Epilogue"].Add(this);
        GroupManager.main.group["Epilogue"].Add(this, new GroupDelegator(null, FadeOut, null));

        GroupManager.main.group["Fading"].Add(this);
        GroupManager.main.group["Fading"].Add(this, new GroupDelegator(null, null, FadeIn));

        GroupManager.main.group["Main Menu"].Add(this, new GroupDelegator(null, Stop, null));
    }

    void Update()
    {
        if (epilogue.volume != epilogueVolume)
            epilogue.volume = Mathf.Lerp(epilogue.volume, epilogueVolume, Time.deltaTime * 3);
    }

    void EpilogueStart()
    {
        if (LevelManager.instance.Level >= LevelManager.instance.LevelCount - 2)
        {
            epilogue.timeSamples = GetComponent<AudioRunning>().SampleOffset();
            epilogue.Play();
            epilogueVolume = volume;
        }
    }

    void FadeOut()
    {
        epilogueVolume = 0.0f;
    }

    void FadeIn()
    {
        epilogueVolume = volume;
    }

    void Stop()
    {
        epilogue.Stop();
    }
}
