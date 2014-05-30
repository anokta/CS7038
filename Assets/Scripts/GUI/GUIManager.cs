using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour
{
	int _width;
	int _height;
	Vector3 _cameraPos;
	float _cameraSize;

	public int defaultFontSize = 16;

	[SerializeField]
	private Material _GuiPieMaterial;

	public static Material GUIPie { get; private set; }

	//Called when screen resizes
	void ResetScreen() { 
		_width = Screen.width;
		_height = Screen.height;

		Printer.Print("Width: " + _width + ", Height: " + _height);

		float ratio = (float)Screen.height / VerticalResolution;
		for (int i = 0; i < _styles.Count; ++i) {
			_styles[i].style.fontSize = Mathf.RoundToInt(ratio * _styles[i].originalSize);
			_styles[i].style.margin.ScaleBy(_styles[i].originalMargin, ratio);// = _styles[i].originalMargin.Scale(ratio);
			_styles[i].style.padding.ScaleBy(_styles[i].originalPadding, ratio);
		}
	}
		
	void ResetCamera() {
		_cameraPos = Camera.main.transform.position;
		_cameraSize = Camera.main.orthographicSize;
	}

	public struct StyleEntry {
		public static int DefaultFontSize;
		public StyleEntry(GUIStyle style) {
			this.style = style;
			this.originalSize = style.fontSize;

			if (originalSize == 0) {
				originalSize = DefaultFontSize;
			}

			this.originalMargin = style.margin.Clone();
			this.originalPadding = style.padding.Clone();
		}
		public GUIStyle style;
		public int originalSize;
		public RectOffset originalMargin;
		public RectOffset originalPadding;
	}
		
	List<StyleEntry> _styles;

	public int VerticalResolution = 500;

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

	private HandyStyles _handyStyles;

	public static HandyStyles Style {
		get { return instance._handyStyles; }
	}

	public static List<StyleEntry> StyleList {
		get { return instance._styles; }
	}

    void Awake()
	{
		StyleEntry.DefaultFontSize = defaultFontSize;
		GUIPie = Object.Instantiate(_GuiPieMaterial) as Material;
		instance = this;
		guiSkin = Object.Instantiate(guiSkin) as GUISkin;

		_handyStyles = new HandyStyles(guiSkin);
		_styles = new List<StyleEntry>();

		for (int i = 0; i < guiSkin.customStyles.Length; ++i) {
			_styles.Add(new StyleEntry(guiSkin.customStyles[i]));
		}
		_styles.Add(new StyleEntry(guiSkin.box));	
		_styles.Add(new StyleEntry(guiSkin.label));
		_styles.Add(new StyleEntry(guiSkin.textArea));
		_styles.Add(new StyleEntry(guiSkin.textField));
		_styles.Add(new StyleEntry(guiSkin.button));
		_styles.Add(new StyleEntry(guiSkin.window));
		_styles.Add(new StyleEntry(guiSkin.toggle));
		_styles.Add(new StyleEntry(guiSkin.scrollView));

		ResetScreen();
	}

	void Start() {
		Printer.Print("Width: " + _width + ", Height: " + _height);
	}

	//public static GUISkin GetSkin()
	//{
	//    return instance.guiSkin;
	//}

	public static GUISkin skin { get { return instance.guiSkin; } }

	void LateUpdate () {
		if (Screen.width != _width || Screen.height != _height) {
			ResetScreen();
			ScreenResized = true;
		} else {
			ScreenResized = false;
		}

		if (Camera.main.transform.position != _cameraPos || Camera.main.orthographicSize != _cameraSize) {
			ResetCamera();
			CameraChanged = true;
		} else {
			CameraChanged = false;
		}
	}

	/// <summary>
	/// Gets a value indicating whether the screen was resized in the previous update.
	/// </summary>
	/// <value><c>true</c> if screen was resized in the previous update; otherwise, <c>false</c>.</value>
	public static bool ScreenResized { get; private set; }

	/// <summary>
	/// Gets a value indicating whether the camera was modified in the previous update.
	/// </summary>
	/// <value><c>true</c> if camera was modified in the previous update; otherwise, <c>false</c>.</value>
	public static bool CameraChanged { get; private set; }
}
