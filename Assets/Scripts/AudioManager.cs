using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioSource background;
    public AudioSource collectSfx, pushSfx, push2Sfx;

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
                collectSfx.Play();
                break;
        }
    }

    void GameMenu()
    {
        background.Stop();
    }

    void LevelStart()
    {
        background.Play();
    }
}
