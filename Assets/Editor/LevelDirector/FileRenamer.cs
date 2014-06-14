using System;
using UnityEditor;
using System.IO;
using UnityEngine;

namespace LevelDirectorEditor
{
	public class FileRenamer : ScriptableWizard
	{
		public static bool IsOpen { get { return _instance != null; } }

		FileInfo _info;
		FileInfo _dialogueInfo;
		static FileRenamer _instance;

		public static void Show(FileInfo info, FileInfo dialogueInfo)
		{
			if (_instance == null) {
				_instance = DisplayWizard<FileRenamer>("Rename file");
			}
			_instance._info = info;
			_instance._newName = System.IO.Path.GetFileNameWithoutExtension(info.FullName);
			_instance._dialogueInfo = dialogueInfo;
			_instance.Repaint();
			_instance.Focus();
		}

		void OnDestroy()
		{
			_instance = null;
		}

		string _newName;

		public void CloseInstance()
		{
			if (_instance != null) {
				_instance.Close();
			}
		}

		public delegate void RenameCallback(FileInfo oldFile,FileInfo newFile);

		Action _final;
		RenameCallback _normal;
		RenameCallback _dialogue;

		public static RenameCallback NormalCallback {
			get { return _instance != null ? _instance._normal : null; }
			set {
				if (_instance != null) {
					_instance._normal = value;
				}
			}
		}

		public static RenameCallback DialogueCallback {
			get { return _instance != null ? _instance._dialogue : null; }
			set {
				if (_instance != null) {
					_instance._dialogue = value;
				}
			}
		}

		public static Action FinalCallback {
			get { return _instance != null ? _instance._final : null; }
			set {
				if (_instance != null) {
					_instance._final = value;
				}
			}
		}

		public void OnGUI()
		{
			GUILayout.BeginVertical();
			GUILayout.Label("Rename file " + _info.Name + " to: ");
			if (Event.current.type == EventType.KeyDown) {
				if (Event.current.keyCode == KeyCode.Return) {
					RenameButton();
				}
				if (Event.current.keyCode == KeyCode.Escape) {
					Close();
				}
			}

			GUI.SetNextControlName("TextFieldThing");
			_newName = GUILayout.TextField(_newName);
			GUI.FocusControl("TextFieldThing");
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Cancel", GUILayout.ExpandWidth(false))) {
				this.Close();
			}
			if (GUILayout.Button("Rename", GUILayout.ExpandWidth(false))) {
				RenameButton();
			}

			GUILayout.EndHorizontal();
		}

		void OnLostFocus()
		{
			
			//this.Close();
			//GUI.BringWindowToFront(this);
			//Focus();
		}

		void RenameButton()
		{
			bool change = false;
			try {
				string newName1 = _info.DirectoryName + "/" + _newName + ".xml";
				string newName2 = _dialogueInfo.DirectoryName + "/" + _newName + ".txt";
				if (File.Exists(newName1)) {
					throw new Exception("File already exists:\n" + newName1);
				}
				if (File.Exists(_dialogueInfo.FullName) && File.Exists(newName2)) {
					throw new Exception("File already exists:\n" + newName2);
				}
				File.Move(_info.FullName, newName1);
				change = true;
				if (_normal != null) {
					_normal(_info, new FileInfo(newName1));
				}
				if (File.Exists(_dialogueInfo.FullName)) {
					File.Move(_dialogueInfo.FullName, newName2);
					if (_dialogue != null) {
						_dialogue(_dialogueInfo, new FileInfo(newName2));
					}
				}
				if (_final != null) {
					_final();
				}
			} catch (Exception e) {
				if (change && _final != null) {
					_final();
				}
				EditorUtility.DisplayDialog("Error!",
					e.Message, "OK");
			}
			this.Close();
		}
	}
}