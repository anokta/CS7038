using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
    public GUISkin guiSkin;

    private static GUIManager instance;

    void Awake()
    {
        instance = this;
    }

    public static GUISkin GetSkin()
    {
        return instance.guiSkin;
    }
}
