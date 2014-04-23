using UnityEngine;
using System.Collections;
using Grouping;

public class MainMenu : MonoBehaviour
{
    // Use this for initialization
    public Texture2D menuBackground;

    public Texture logoTexture;
    float logoSize, logoTargetSize, logoRatio;

    float currentScroll, targetScroll;
    public static float ScreenScrollValue
    {
        get { return Mathf.Max(Screen.width, Screen.height); }
    }

    void Start()
    {
        GroupManager.main.group["Main Menu"].Add(this);
        AudioMenu.OnNextBeat += OnNextBeat;

        logoRatio = logoTexture.width / logoTexture.height;
		logoTargetSize = minLogoSize;
        logoSize =  logoTargetSize;
    }

	void Update()	
    {
        // TODO: Remove before release
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerPrefs.DeleteAll();
            LevelManager.Instance.Level = -1;
        }
        //

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) )
        {
            targetScroll = -ScreenScrollValue;
            AudioManager.PlaySFX("Menu Next");
        }

        if (Mathf.Abs(targetScroll - currentScroll) < ScreenScrollValue * 0.05f)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GroupManager.main.activeGroup = GroupManager.main.group["Exiting"];
            }

            if (targetScroll == -ScreenScrollValue)
            {
                AfterFadeOut();

                targetScroll = 0.0f;
            }
        }

        currentScroll = Mathf.Lerp(currentScroll, targetScroll, Time.deltaTime * 5.0f);

        if (logoTargetSize != logoSize)
        {
            logoSize = Mathf.Lerp(logoSize, logoTargetSize, 4.0f * Time.deltaTime);
        }

    }

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(new Vector3(0.0f, currentScroll, 0.0f), Quaternion.identity, Vector3.one);

        GUI.skin = GUIManager.GetSkin();

        //LOGO
        {
            GUI.DrawTexture(new Rect((Screen.width - Screen.height * logoSize * logoRatio) * 0.5f, Screen.height * 0.25f - Screen.height * logoSize * 0.5f, Screen.height * logoSize * logoRatio, Screen.height * logoSize), logoTexture);
        }
        //GUI.Label(new Rect(0, 0, Screen.width, Screen.height * 0.4f), "Handy MD", GUI.skin.GetStyle("title"));

        GUI.matrix = Matrix4x4.TRS(new Vector3(-currentScroll, 0.0f, 0.0f), Quaternion.identity, Vector3.one);

        // PLAY //
        {
            float size = Screen.height * 0.3f;
            if (GUI.Button(
                new Rect((Screen.width - size) * 0.5f, (Screen.height - size) * 0.65f, size, size),
                "", GUI.skin.GetStyle("play")))
            {
                targetScroll = -ScreenScrollValue;
                AudioManager.PlaySFX("Menu Next");
            }
        }

        GUI.matrix = Matrix4x4.TRS(new Vector3(0.0f, -currentScroll, 0.0f), Quaternion.identity, Vector3.one);

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

			ScreenFader.QueueEvent(BackgroundRenderer.instance.SetTileBackground);
            GroupManager.main.activeGroup = GroupManager.main.group["Intro"];
        }
        else
        {
            GroupManager.main.activeGroup = GroupManager.main.group["Level Select"];
        }
    }

	[SerializeField]
	float minLogoSize = 0.212f;
	[SerializeField]
	float maxLogoSize = 0.204f;

    void OnNextBeat(int beatCount)
    {
		logoTargetSize = (beatCount % 2 == 1) ? minLogoSize : maxLogoSize;
    }
}
