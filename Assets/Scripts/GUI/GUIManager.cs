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

	//Called when screen resizes
	void ResetScreen() { 
		_width = Screen.width;
		_height = Screen.height;

		float ratio = (float)Screen.height / VerticalResolution;
		for (int i = 0; i < _customStyles.Length; ++i) {
			_customStyles[i].style.fontSize = Mathf.RoundToInt(ratio * _customStyles[i].originalSize);
			_customStyles[i].style.margin = _customStyles[i].originalMargin.Scale(ratio);
			_customStyles[i].style.padding = _customStyles[i].originalPadding.Scale(ratio);
		}
		for (int i = 0; i < _builtinStyles.Count; ++i) {
			_builtinStyles[i].style.fontSize = Mathf.RoundToInt(ratio * _builtinStyles[i].originalSize);
			_builtinStyles[i].style.margin = _builtinStyles[i].originalMargin.Scale(ratio);
			_builtinStyles[i].style.padding = _builtinStyles[i].originalPadding.Scale(ratio);
		}
	}
		
	void ResetCamera() {
		_cameraPos = Camera.main.transform.position;
		_cameraSize = Camera.main.orthographicSize;
	}

	private struct StyleEntry {
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
		
	private List<StyleEntry> _builtinStyles;
	StyleEntry[] _customStyles;

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

    void Awake()
	{
		StyleEntry.DefaultFontSize = defaultFontSize;

		instance = this;
		guiSkin = GUISkin.Instantiate(guiSkin) as GUISkin;
		_customStyles = new StyleEntry[guiSkin.customStyles.Length];
		for (int i = 0; i < _customStyles.Length; ++i) {
			_customStyles[i] = new StyleEntry(guiSkin.customStyles[i]);
		}
		_builtinStyles = new List<StyleEntry>();
		_builtinStyles.Add(new StyleEntry(guiSkin.box));
		_builtinStyles.Add(new StyleEntry(guiSkin.label));
		_builtinStyles.Add(new StyleEntry(guiSkin.textArea));
		_builtinStyles.Add(new StyleEntry(guiSkin.textField));
		_builtinStyles.Add(new StyleEntry(guiSkin.button));
		_builtinStyles.Add(new StyleEntry(guiSkin.window));
		_builtinStyles.Add(new StyleEntry(guiSkin.toggle));
		_builtinStyles.Add(new StyleEntry(guiSkin.scrollView));

		ResetScreen();
	}

    public static GUISkin GetSkin()
    {
        return instance.guiSkin;
    }

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
