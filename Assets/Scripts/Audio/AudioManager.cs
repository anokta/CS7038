using UnityEngine;
using System.Collections;
using Grouping;

public class AudioManager : MonoBehaviour
{
    public AudioSource menuNext, menuPrev, levelSwipe;
    public AudioSource collectSfx, pushSfx, push2Sfx, trolleyLoopSfx, doorSfx, fountainSfx, fountainLoopSfx, leverSfx, mirrorSfx, treatedSfx, diedSfx, treatingSfx, laserSfx, explosionSfx, burnSfx;

    private static AudioManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GroupManager.main.group["Level Start"].Add(this, new GroupDelegator(null, LevelStart, null));
        GroupManager.main.group["Running"].Add(this, new GroupDelegator(null, null, StopAllSfx));
    }

    public static void PlaySFX(string type)
    {
        instance.playSfx(type);
    }

    public static void PlaySFXDelayed(string type, float delay)
    {
        switch (type)
        {
            case "Explosion Crate":
                instance.explosionSfx.PlayScheduled(Mathf.Max(0.0f, delay));
                break;
        }
    }

    public static void StopSFX(string type)
    {
        instance.stopSfx(type);
    }

    void playSfx(string type)
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
                doorSfx.PlayScheduled(0.1f);
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
                treatedSfx.pitch += Random.Range(-0.025f, 0.025f);
                treatedSfx.Play();
                break;

            case "Died":
                diedSfx.pitch += Random.Range(-0.025f, 0.025f);
                diedSfx.Play();
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
                levelSwipe.pitch += Random.Range(-0.025f, 0.025f);
                levelSwipe.Play();
                break;

            case "Loop Patient":
                treatingSfx.Play();
                break;

            case "Burn":
                burnSfx.Play();
                break;
        }
    }

    void stopSfx(string type)
    {
        switch (type)
        {
            case "Loop Trolley":
                trolleyLoopSfx.Stop();
                break;

            case "Loop Fountain":
                fountainLoopSfx.Stop();
                break;

            case "Loop Patient":
                treatingSfx.Stop();
                break;
        }
    }

    void LevelStart()
    {
        collectSfx.pitch = 1.0f;
        pushSfx.pitch = 1.0f;
        push2Sfx.pitch = 1.0f;
    }

    void StopAllSfx()
    {
        trolleyLoopSfx.Stop();
        fountainLoopSfx.Stop();
        treatingSfx.Stop();
    }
}
