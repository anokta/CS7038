using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public AudioSource collectSfx, pushSfx;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySFX(string type)
    {
        switch (type)
        {
            case "Push":
                pushSfx.Play();
                break;

            case "Collect":
                collectSfx.Play();
                break;
        }
    }
}
