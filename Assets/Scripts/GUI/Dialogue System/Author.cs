using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Author
{
	[SerializeField]
	string displayName;
    public string Name
    {
		get { return displayName; }
    }

	[SerializeField]
	string id;
	public string Key
	{
		get { return id; }
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
    [SerializeField]
    [Range(0, 1)]
    float w;
    [SerializeField]
    [Range(0, 1)]
    float h;

    public Rect GuiRectangle
    {
        get { return new Rect(x * Screen.width, y * Screen.height, w * Screen.width, h * Screen.height); }
    }

    [SerializeField]
    public TextAnchor Alignment;

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
}