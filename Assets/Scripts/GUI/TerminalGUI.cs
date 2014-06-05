using System.Threading;
using UnityEngine;
using Grouping;


public class TerminalGUI : MonoBehaviour {
	public static TerminalGUI instance { get; private set; }

	[SerializeField]
	float _height = 0.7f;
	[SerializeField]
	float _width = 0.6f;
	[SerializeField]
	float _buttonSize = 0.1f;

	float _acHeight;
	float _acWidth;
	float _acButtonSize;

	//Terminal current;

	public void Activate(Terminal terminal) {
		//current = terminal;
		GroupManager.main.activeGroup = GroupManager.main.group["Terminal"];
		Resize();
	}

	void Start() {
		instance = this;
		GroupManager.main.group["Terminal"].Add(this);
	}

	void Update() {
		if (GUIManager.ScreenResized) {
			Resize();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Back();
		}

	}

	void Resize() {
		_acHeight = Screen.height * _height;
		_acWidth = Screen.height * _width;
		_acButtonSize = Screen.height * _buttonSize;
	}

	void Back() {
		AudioManager.PlaySFX("Level Swipe Reversed");
		GroupManager.main.activeGroup = GroupManager.main.group["Running"];
	}


	void OnGUI() {
		Rect rect = new Rect((Screen.width - _acWidth)*0.5f, (Screen.height-_acHeight)*0.5f, _acWidth, _acHeight);
		GUI.Window(0, rect, DoWindow, "", GUIManager.Style.overWindow);
	}

	void DoWindow(int id) {
		GUILayout.BeginVertical();
		{
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Back", GUIManager.Style.buttonYes,
					GUILayout.Width(_acButtonSize * 2),
					GUILayout.Height(_acButtonSize))) {
					Back();
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndVertical();
	}

}
