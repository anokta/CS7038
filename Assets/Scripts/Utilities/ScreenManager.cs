using UnityEngine;
using System.Collections;

/// <summary>
/// Handles screen resizing events
/// </summary>
public class ScreenManager : MonoBehaviour {

	int _width;
	int _height;

	// Use this for initialization
	void Start () {
		Reset();
	}

	void OnEnable() {
		Available = true;
	}

	void OnDisable() {
		var scripts = Object.FindObjectsOfType<ScreenManager>();
		Available = false;
		foreach (var script in scripts) {
			if (script != this && script.enabled == true) {
				Available = true;
				break;
			}
		}
	}

	void Reset() { 
		_width = Screen.width;
		_height = Screen.height;
	}

	/// <summary>
	/// Gets a value indicating whether the screen was resized in the previous update.
	/// </summary>
	/// <value><c>true</c> if screen was resized in the previous update; otherwise, <c>false</c>.</value>
	public static bool WasResized { get; private set; }

	/// <summary>
	/// Gets a value indicating whether a <see cref="ScreenManager"/> script is running.
	/// </summary>
	/// <value><c>true</c> if available; otherwise, <c>false</c>.</value>
	public static bool Available { get; private set; }

	void LateUpdate () {
		if (Screen.width != _width || Screen.height != _height) {
			Reset();
			WasResized = true;
		} else {
			WasResized = false;
		}
	}
}
