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

    public void Display(GUISkin guiSkin, string displayedText)
    {
        GUIStyle authorStyle = guiSkin.GetStyle("author");
        GUIStyle contentStyle = guiSkin.GetStyle("content");

        GUI.DrawTexture(new Rect(Author.ScreenPosition.x - ENTRY_HEIGHT, Author.ScreenPosition.y, ENTRY_HEIGHT, ENTRY_HEIGHT), Author.Avatar);

        GUILayout.BeginArea(new Rect(Author.ScreenPosition.x, Author.ScreenPosition.y, ENTRY_WIDTH, ENTRY_HEIGHT));

        authorStyle.normal.textColor = Author.TextColor;
        GUILayout.Label(Author.Name + ":>", authorStyle);

        contentStyle.normal.textColor = Author.TextColor + new Color(0.5f, 0.5f, 0.5f);
        GUILayout.Label(displayedText, contentStyle);

        GUILayout.EndArea();
    }

    public bool DisplayButton(GUIStyle style)
    {
        return GUI.Button(new Rect(Author.ScreenPosition.x + ENTRY_WIDTH - 60, Author.ScreenPosition.y + ENTRY_HEIGHT, 60, 30), GUIContent.none, style);
    }
}