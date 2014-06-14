using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace HandyEditor
{
	using UnityEditor;
	using UnityEngine;

	public class TextEditorWizard : ScriptableWizard {
		FileInfo _info;
		public static void CreateWizard(string title, FileInfo info) {
			TextEditorWizard tew = null;
			if (_data.ContainsKey(info.FullName)) {
				tew = _data[info.FullName];
			} else {
				tew = ScriptableWizard.DisplayWizard<TextEditorWizard>(title);
				_data[info.FullName] = tew;
			}
			tew._info = info;
			tew._contents = System.IO.File.ReadAllText(info.FullName);
			tew.Focus();
		}
		static Dictionary<string, TextEditorWizard> _data = new Dictionary<string, TextEditorWizard>();

		void OnDestroy() {
			if (_data.ContainsKey(_info.FullName)) {
				_data.Remove(_info.FullName);
			}
		}

		string _contents;
		bool _dirty = false;
		Vector2 _scrollPosition;
		void OnGUI() {
			EditorGUILayout.BeginVertical();
			_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
			_contents = EditorGUILayout.TextArea(_contents, GUILayout.ExpandHeight(true));
			EditorGUILayout.EndScrollView();
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Save and Close", GUILayout.ExpandWidth(false))) {
				Save();
				this.Close();
			}
			if (GUILayout.Button("Save", GUILayout.ExpandWidth(false))) {
				Save();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}

		void Save() {
			System.IO.File.WriteAllText(_info.FullName, _contents);
		}
	}
}