using System;
using UnityEngine;

public class KeySequenceDetector
{
    public bool Loop { get; set; }

    private readonly KeyCode[] keySequence;
    private readonly Action handler;
    private int currentIndex;

    public KeySequenceDetector(string keys, Action handler, bool loop = false)
    {
        keySequence = KeyCodeExt.Parse(keys);
        this.handler = handler;
        Loop = loop;
    }

    public void Update()
    {
        if (!Input.anyKeyDown) return;

        if (Input.GetKeyDown(keySequence[currentIndex]))
        {
            currentIndex++;

            if (currentIndex >= keySequence.Length)
            {
                currentIndex = 0;
                handler();

                if (!Loop)
                {
                    KeyboardController.Instance.Remove(this);
                }
            }
        }
        else
        {
            currentIndex = 0;
        }
    }
}
