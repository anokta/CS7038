using UnityEngine;
using System.Collections;
using Grouping;

public class MainMenu : MonoBehaviour
{
    // Use this for initialization
    public Texture2D menuBackground;
    public Texture logoTexture;

    float currentScroll, targetScroll;
    public static float ScreenScrollValue
    {
        get { return Mathf.Max(Screen.width, Screen.height); }
    }

    void Start()
    {
        GroupManager.main.group["Main Menu"].Add(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Exiting"];
        }

        if (Mathf.Abs(targetScroll - currentScroll) < ScreenScrollValue * 0.05f)
        {
            if (targetScroll == -ScreenScrollValue)
            {
                AfterFadeOut();

                targetScroll = 0.0f;
            }
        }

        currentScroll = Mathf.Lerp(currentScroll, targetScroll, Time.deltaTime * 5.0f);
    }

    void OnGUI()
    {
        Matrix4x4 guiMatrix = GUI.matrix;
        GUI.matrix *= Matrix4x4.TRS(new Vector3(0.0f, currentScroll, 0.0f), Quaternion.identity, Vector3.one);

        GUI.skin = GUIManager.GetSkin();

        //LOGO
        {
            Rect logoRect = new Rect(0, 0, 0, Screen.height * 0.2f);
            float logoRatio = (logoRect.height) / logoTexture.height;
            logoRect.width = logoRatio * logoTexture.width;
            logoRect.x = (Screen.width - logoRect.width) * 0.5f;
            logoRect.y = (Screen.height - logoRect.height) * 0.2f;

            GUI.DrawTexture(logoRect, logoTexture);
        }
        //GUI.Label(new Rect(0, 0, Screen.width, Screen.height * 0.4f), "Handy MD", GUI.skin.GetStyle("title"));

        GUI.matrix = guiMatrix;
        GUI.matrix *= Matrix4x4.TRS(new Vector3(-currentScroll, 0.0f, 0.0f), Quaternion.identity, Vector3.one);

        // PLAY //
        {
            float size = Screen.height * 0.3f;
            if (GUI.Button(
                new Rect((Screen.width - size) * 0.5f, (Screen.height - size) * 0.65f, size, size),
                "", GUI.skin.GetStyle("play")))
            {
                targetScroll = -ScreenScrollValue;
            }
        }

        GUI.matrix = guiMatrix;
        GUI.matrix *= Matrix4x4.TRS(new Vector3(0.0f, -currentScroll, 0.0f), Quaternion.identity, Vector3.one);

        // LEFT CORNER //
        GUILayout.BeginArea(new Rect(GUIManager.OffsetX() * 2.0f, Screen.height - GUIManager.OffsetY() * 2.0f - GUIManager.ButtonSize(), 2.0f * GUIManager.ButtonSize(), GUIManager.ButtonSize()));

        GUILayout.BeginHorizontal();

        // Mute
        //TODO: Temporary hack, fix
        string styleOfVolume = AudioListener.volume <= 0.001f ? "volume off" : "volume on";
        if (GUILayout.Button("Mute", GUI.skin.GetStyle(styleOfVolume), GUILayout.Width(GUIManager.ButtonSize()), GUILayout.Height(GUIManager.ButtonSize())))
        {
            AudioListener.volume = 1 - AudioListener.volume;
            PlayerPrefs.SetFloat("Audio Volume", AudioListener.volume);
        }

        // Credits
        if (GUILayout.Button("Credits", GUI.skin.GetStyle("credits"), GUILayout.Width(GUIManager.ButtonSize()), GUILayout.Height(GUIManager.ButtonSize())))
        {
            // TODO : Credits
        }

        GUILayout.EndHorizontal();

        GUILayout.EndArea();


        // RIGHT CORNER //
        GUILayout.BeginArea(new Rect(Screen.width - Screen.height / 10.0f - GUIManager.OffsetX() * 2.0f, Screen.height - Screen.height / 10.0f - GUIManager.OffsetY() * 2.0f, Screen.height / 10.0f, Screen.height / 10.0f));

        GUILayout.BeginHorizontal();

        // Url
        if (GUILayout.Button("i", GUI.skin.GetStyle("info"), GUILayout.Width(Screen.height / 10.0f), GUILayout.Height(Screen.height / 10.0f)))
        {
            Application.OpenURL("http://www.surewash.com/");
        }

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    void AfterFadeOut()
    {
        // Start the level
        if (LevelManager.Instance.Level == -1)
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Intro"];
        }
        else
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Level Select"];
        }
    }
}
