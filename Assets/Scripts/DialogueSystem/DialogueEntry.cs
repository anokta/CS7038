using UnityEngine;
using System.Collections;
using System;

public class DialogueEntry
{
    public static int ENTRY_WIDTH = 300;
    public static int ENTRY_HEIGHT = 150;

    [SerializeField]
    Author author;
    public Author Author
    {
        get { return author; }
    }

    [SerializeField]
    [Multiline]
    string content;
    public string Content
    {
        get { return content; }
    }

    public DialogueEntry(Author author, string content = "Content")
    {
        this.author = author;
        this.content = content;
    }

    public bool DisplayEntry(GUISkin guiSkin, string displayedText)
    {
        bool pressed = false;

        if(Author.Avatar != null)
            GUI.DrawTexture(new Rect(Author.ScreenPosition.x - ENTRY_HEIGHT, Author.ScreenPosition.y, ENTRY_HEIGHT, ENTRY_HEIGHT), Author.Avatar);

        GUILayout.BeginArea(new Rect(Author.ScreenPosition.x, Author.ScreenPosition.y, ENTRY_WIDTH, ENTRY_HEIGHT));

        if (Author.Name != "[Narrator]")
        {
            GUIStyle authorStyle = guiSkin.GetStyle("author");

            authorStyle.normal.textColor = Author.TextColor;
            pressed |= GUILayout.Button(Author.Name + ":>", authorStyle);
        }

        GUIStyle contentStyle = guiSkin.GetStyle("content");

        contentStyle.normal.textColor = Author.TextColor + new Color(0.5f, 0.5f, 0.5f);
        pressed |= GUILayout.Button(displayedText, contentStyle);

        GUILayout.EndArea();

        return pressed;
    }

    public bool DisplayContinueButton(GUIStyle style)
    {
        return GUI.Button(new Rect(Author.ScreenPosition.x + ENTRY_WIDTH - 60, Author.ScreenPosition.y + ENTRY_HEIGHT, 60, 30), GUIContent.none, style);
    }
}