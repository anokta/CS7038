using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioSource background;
    public AudioSource collectSfx, pushSfx, push2Sfx, doorSfx, fountainSfx, leverSfx, treatedSfx;

    // Use this for initialization
    void Awake()
    {
        GameEventManager.GameMenu += GameMenu;
        GameEventManager.LevelStart += LevelStart;
    }

    // Update is called once per frame
    void Update()
    {
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

            case "Lever":
                leverSfx.Play();
                break;

            case "Treated":
                treatedSfx.Play();
                break;
        }
    }

    void GameMenu()
    {
        background.Stop();
    }

    void LevelStart()
    {
        collectSfx.pitch = 1.0f;
        pushSfx.pitch = 1.0f;
        push2Sfx.pitch = 1.0f;

        background.Play();
    }
}
