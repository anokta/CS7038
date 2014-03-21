using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
    public GUISkin guiSkin;

    public Vector2 screenOffset;
    public static float OffsetX()
    {
        return instance.screenOffset.x * Screen.height;
    }
    public static float OffsetY()
    {
        return instance.screenOffset.y * Screen.height;
    }

    public float defaultButtonSize;
    public static float ButtonSize()
    {
        return instance.defaultButtonSize * Screen.height;
    }

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
