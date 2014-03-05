using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Author
{
    [SerializeField]
    string name;
    public string Name
    {
        get { return name; }
    }

    [SerializeField]
    AudioClip voice;
    public AudioClip VoiceSample
    {
        get { return voice; }
    }

    [SerializeField]
    [Range(0, 1)]
    float x;
    [SerializeField]
    [Range(0, 1)]
    float y;

    public Vector2 NormalizedPosition
    {
        get { return new Vector2(x, y); }
        set { x = value.x; y = value.y; }
    }
    public Vector2 ScreenPosition
    {
        get { return new Vector2(Screen.width * x - DialogueEntry.ENTRY_WIDTH / 2, Screen.height * y - DialogueEntry.ENTRY_HEIGHT / 2); }
    }

    [SerializeField]
    Color textColor;
    public Color TextColor
    {
        get { return textColor; }
        set { textColor = value; }
    }

    [SerializeField]
    Texture2D avatar;
    public Texture2D Avatar
    {
        get { return avatar; }
        set { avatar = value; }
    }

    public Author(string name, AudioClip voice, Vector2 position, Color textColor = default(Color), Texture2D avatar = null)
    {
        this.name = name;
        this.voice = voice;
        NormalizedPosition = position;
        this.textColor = textColor;
        this.avatar = avatar;
    }
}