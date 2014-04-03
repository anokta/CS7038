using UnityEngine;
using System.Collections;
using Grouping;

public class AudioIntro : MonoBehaviour {
    
    public AudioSource intro;
    public float volume;

    float introVolume;

	void Start () {
        GroupManager.main.group["Intro"].Add(this);
        GroupManager.main.group["Intro"].Add(this, new GroupDelegator(null, IntroStart, null));
        GroupManager.main.group["Dialogue"].Add(this);

        GroupManager.main.group["Fading"].Add(this);
        GroupManager.main.group["Fading"].Add(this, new GroupDelegator(null, FadeOut, FadeIn));

        GroupManager.main.group["Level Select"].Add(this, new GroupDelegator(null, Stop, null));
        GroupManager.main.group["Level Start"].Add(this, new GroupDelegator(null, Stop, null));
    }
	
    void Update () {
        if (intro.volume != introVolume)
            intro.volume = Mathf.Lerp(intro.volume, introVolume, Time.deltaTime * 2);
	}

    void IntroStart()
    {
        intro.Play();
        introVolume = volume;
    }

    void FadeOut()
    {
        introVolume = 0.0f;
    }

    void FadeIn()
    {
        introVolume = volume;
    }

    void Stop()
    {
        intro.Stop();
    }
}
