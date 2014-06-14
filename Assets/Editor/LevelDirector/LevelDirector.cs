using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using HandyEditor;
using System;
using System.IO;
using System.Runtime.InteropServices;

public class LevelDirector : EditorWindow
{
	public void OnEnable()
	{
		LevelPath = Application.dataPath + "/Levels/Resources/";
		LevelFile = LevelPath + "levels.yaml";
		DialoguePath = LevelPath + "Dialogues/";
		//errorStyle = new GUIStyle(EditorStyles.miniLabel);
		//errorStyle.normal.textColor = Color.cyan;
		//errorStyle.fontStyle = FontStyle.Bold;
		//	errorStyle.normal = Color.cyan;
		Refresh();
	}

	//GUIStyle errorStyle;
	private int LevelsPerPage = 12;
	private string LevelPath;
	private string DialoguePath;
	private string LevelFile;

	private List<LevelEntry> levelData = new List<LevelEntry>();
	private List<Rect> rectData = new List<Rect>();

	void Refresh()
	{
		var files = (System.IO.File.ReadAllLines(LevelFile));
		levelData.Clear();
		foreach (var file in files) {
			if (!StringExt.IsNullOrWhitespace(file)) {
				levelData.Add(new LevelEntry() { Name = file.Trim() });
			}
		}
		foreach (var entry in levelData) {
			string dir = LevelPath + entry.Name + ".xml";
			//if (File.Exists(dir)) {
			entry.File = new FileInfo(dir);
			//}
			string dialDir = DialoguePath + entry.Name + ".txt";
			//if (entry.HasDialogue = File.Exists(dialDir)) {
			entry.Dialogue = new FileInfo(dialDir);
			//}
		}
		int pages;
		if (levelData.Count > 0) {
			pages = levelData.Count / LevelsPerPage + 1;
		} else {
			pages = 0;
		}

		while (pageToggles.Count < pages) {
			pageToggles.Add(true);
		}

		//_saveDirty = false;
		this.Repaint();
	}

	class LevelEntry
	{
		public string Name;
		public FileInfo File;
		public FileInfo Dialogue;
	}




	[MenuItem("Window/Level Director")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(LevelDirector));
	}

	Vector2 _scrollPosition;

	float GetMaxOrderWidth()
	{
		float maxWidth = 0;
		for (int i = 1; i < levelData.Count; ++i) {
			float width = EditorStyles.label.CalcSize(new GUIContent(i.ToString())).x;
			if (width > maxWidth) {
				maxWidth = width;
			}
		}
		return maxWidth;
	}

	int selectedIndex = -1;
	int hoverIndex = -1;

	bool MouseInside()
	{
		Vector2 inner = Event.current.mousePosition;
		return (inner.x >= 0 && inner.y >= 0 && inner.x - _scrollPosition.x < Screen.width && inner.y - _scrollPosition.y < Screen.height);
	}

	int GetIndex()
	{
		Vector2 screenPos = Event.current.mousePosition;
		for (int i = 0; i < rectData.Count; ++i) {
			if (rectData[i].Contains(screenPos)) {
				return i;
			}
		}
		return -1;
	}

	/*struct LevelEntry {
		public string Name;
		public File File;
		public bool HasDialogue;
		public File Dialogue;
	}*/

	static List<bool> pageToggles = new List<bool>();

	void OnGUI()
	{
		title = "Level Director";
		GUILayout.Space(10);
		_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);//.BeginScrollView(
		//	Debug.Log(_scrollPosition.ToString());
		//int id = 1;
		float orderWidth = GetMaxOrderWidth();
		if (Event.current.type == EventType.MouseDown) {
			rectData.Clear();
		}
		GUILayout.BeginVertical();
		for (int i = 0; i < levelData.Count; ++i) {
			int pageIndex = (i / LevelsPerPage);
			if (i % LevelsPerPage == 0) {
				if (i != 0) {
					//	EditorGUILayout.EndToggleGroup();
				}
				//GUILayout.Label("Page " + (pageIndex + i), EditorStyles.boldLabel);
				//pageToggles[pageIndex] = //EditorGUILayout.BeginToggleGroup("Page " + (pageIndex + 1), pageToggles[pageIndex]);
				pageToggles[pageIndex] = EditorGUILayout.Foldout(pageToggles[pageIndex], "Page " + (pageIndex + 1));
				//GUILayout.Label("Page " + (i / LevelsPerPage + 1), EditorStyles.boldLabel);
			}
			if (pageToggles[pageIndex]) {
				if (Event.current.type == EventType.MouseDown) {
					rectData.Add(new Rect());
				}
				continue;
			}
			//EditorGUILayout.BeginToggleGroup(
			GUILayout.BeginHorizontal();
			GUILayout.Space(5);
			if (GUILayout.Button("-", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false))) {
				//_saveDirty = true;
				levelData.RemoveAt(i);
				SaveOrder();
				this.Repaint();
				return;
				//rectData.RemoveAt(i);
			}

			if (GUILayout.Button("edit", EditorStyles.miniButtonMid, GUILayout.ExpandWidth(false))) {
				TextEditorWizard.CreateWizard("Edit Level", levelData[i].File);
			}

			if (GUILayout.Button("rename", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false))) {
				Rename(i);
			}
			//GUILayout.Button("Dialogue", GUILayout.ExpandWidth(false));

			GUILayout.Label((i + 1).ToString(), GUILayout.Width(orderWidth));
			GUILayout.Space(5);
			var absoluteSize = EditorStyles.largeLabel.CalcSize(new GUIContent(levelData[i].Name));
			if (!levelData[i].File.Exists) {
				var pre1 = GUI.color;
				GUI.color = Color.cyan;
				//GUI.contentColor = Color.white;
				GUILayout.Label("(error)", EditorStyles.miniLabel);
				GUI.color = pre1;
			}
			if ((selectedIndex == i || hoverIndex == i) && MouseInside()) {
				GUILayout.Label(levelData[i].Name, EditorStyles.toolbarTextField, GUILayout.Width(absoluteSize.x), GUILayout.Height(absoluteSize.y));
			} else {
				GUILayout.Label(levelData[i].Name, GUILayout.Width(absoluteSize.x), GUILayout.Height(absoluteSize.y));
				
			}
			if (Event.current.type == EventType.MouseDown) {
				var rect = GUILayoutUtility.GetLastRect();
				rect.x -= _scrollPosition.x;
				rect.y -= _scrollPosition.y;
				rectData.Add(rect);
				//Debug.Log(rectData[i]);
			}
			//GUILayout.Space(10);
			GUILayout.FlexibleSpace();
			if (levelData[i].Dialogue.Exists) {
				if (GUILayout.Button("-", EditorStyles.miniButtonLeft)) {
					DeleteDialogue(i);
				}
				GUILayout.Space(5);
				if (GUILayout.Button("edit dialogue", EditorStyles.miniButtonRight)) {
					TextEditorWizard.CreateWizard("Edit dialogue", levelData[i].Dialogue);
				}
				//GUILayout.Label("(dialogue)", EditorStyles.miniLabel);
			} else {
				var pre1 = GUI.color;
				GUI.color = Color.cyan;
				GUILayout.Label("(no dialogue)", EditorStyles.miniLabel);
				GUI.color = pre1;
				if (GUILayout.Button("add", EditorStyles.miniButtonRight)) {
					CreateDialogue(i);
				}
			}
			GUILayout.EndHorizontal();
		}//

		EditorGUILayout.EndScrollView();
		GUILayout.Space(10);
		GUILayout.EndVertical();
		GUILayout.BeginHorizontal();
		{
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Refresh", GUILayout.ExpandWidth(false))) {
				Refresh();
			}
			//GUI.enabled = _saveDirty;
			if (GUILayout.Button("Save Order", GUILayout.ExpandWidth(false))) {
				SaveOrder();
			}
			//GUI.enabled = true;
			/*if (GUILayout.Button("Save", GUILayout.ExpandWidth(false))) {
				Save();
			}*/
			GUILayout.Space(10);
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(10);

		if (Event.current.type == EventType.MouseDown) {
			selectedIndex = GetIndex();
		}

		if (selectedIndex != -1 && Event.current.type == EventType.MouseDrag) {
			if (MouseInside()) {
				hoverIndex = GetIndex();
			} else {
				hoverIndex = -1;
			}
			this.Repaint();
		}

		if (selectedIndex != -1 && Event.current.type == EventType.Repaint) {
			Vector2 inner = Event.current.mousePosition;
			if (inner.x >= 0 && inner.y >= 0) {
				GUI.Label(new Rect(Event.current.mousePosition.x,
					Event.current.mousePosition.y,
					rectData[selectedIndex].width,
					rectData[selectedIndex].height), levelData[selectedIndex].Name, EditorStyles.toolbarTextField);
			}
		}

		if (Event.current.type == EventType.MouseMove) {
			//if (selected != -1) {
			//selected = -1;
			//	this.Repaint();
			//}
		}

		if (Event.current.type == EventType.MouseUp) {
			if (selectedIndex != -1) {
				hoverIndex = GetIndex();
				if (hoverIndex > -1 && hoverIndex != selectedIndex) {

					var temp = levelData[selectedIndex];
					levelData[selectedIndex] = levelData[hoverIndex];
					levelData[hoverIndex] = temp;
					SaveOrder();
					//_saveDirty = true;
					//Save();
				}
			}

			selectedIndex = -1;
			hoverIndex = -1;

			Repaint();
		}
	}

	void Rename(int index)
	{
		var entry = levelData[index];
		FileRenamer.Show(levelData[index].File, levelData[index].Dialogue);
		FileRenamer.NormalCallback = delegate(FileInfo old, FileInfo newFile) {
			//Debug.Log("N");
			levelData[index].File = newFile;
			levelData[index].Name = Path.GetFileNameWithoutExtension(newFile.FullName);
		};
		FileRenamer.DialogueCallback = delegate(FileInfo old, FileInfo newFile) {
			//Debug.Log("N2");	
			levelData[index].Dialogue = newFile;
		};

		SaveOrder();
		this.Repaint();
		//	GUILayout.Window(index, new Rect(0, 0, 100, 100).Centered(), DoRename, "Rename");
		//}
	}

	void CreateDialogue(int index)
	{
		var entry = levelData[index];
		if (!entry.Dialogue.Exists) {
			using (File.CreateText(entry.Dialogue.FullName)) {
				entry.Dialogue = new FileInfo(entry.Dialogue.FullName);
			}
			;
		}
		this.Repaint();
	}

	void DoRename(int id)
	{
	}

	void DeleteDialogue(int index)
	{
		if (EditorUtility.DisplayDialog("Confirm",
			    "Are you sure you want to delete this dialogue file? This operation cannot be undone.",
			    "Yes", "No")) {
			File.Delete(levelData[index].Dialogue.FullName);
			levelData[index].Dialogue = new FileInfo(levelData[index].Dialogue.FullName);
		}
	}

	void SaveOrder()
	{
		/*if (EditorUtility.DisplayDialog("Confirm",
			"Are you sure you want to save the order of the levels? This operation cannot be undone.",
			    "Yes", "No")) {*/
		using (System.IO.StreamWriter file = new System.IO.StreamWriter(LevelFile)) {
			foreach (var entry in levelData) {
				// If the line doesn't contain the word 'Second', write the line to the file. 
				file.WriteLine(entry.Name);
			}
			//_saveDirty = false;
		}

		AssetDatabase.Refresh();

		Refresh();
		//}
	}

	//bool _saveDirty = false;

	void Save()
	{
		//Debug.Log();
		//string path = 
	}
}