

using UnityEngine;
using Grouping;

public class EpilogueGUI : MonoBehaviour
{

	[SerializeField]
	private float
		height = 0.7f;
	[SerializeField]
	private float width = 0.8f;
	private float buttonSize = 0.2f;
	private float _height;
	private float _width;
	private float _buttonSize;

	void Resize()
	{
		_height = height * Screen.height;
		_width = width * Screen.height;
		_buttonSize = buttonSize * Screen.height;
	}

	void Start()
	{
		Resize();
		GroupManager.main.group["Epilogue"].Add(this);
	}

	void OnGUI()
	{
		Rect size = new Rect(0, 0, _width, _height);
		GUI.Window(0, size.Centered(), DoWindow, "", GUIManager.Style.overWindow);
	}

	void Update()
	{
		if (GUIManager.ScreenResized) {
			Resize();
		}
	}

	void DoWindow(int id)
	{
		GUILayout.BeginVertical();
		{
			GUILayout.Label("Thank you for playing!", GUIManager.Style.overTitle);
			GUILayout.Label("Stay tuned for more!", GUIManager.Style.overMessage);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("I am excited!", GUIManager.Style.buttonYes, GUILayout.Width(_buttonSize * 2), GUILayout.Height(_buttonSize))) {
					GoToMainMenu();
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndVertical();
	}

	void GoToMainMenu()
	{
		GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
	}
}