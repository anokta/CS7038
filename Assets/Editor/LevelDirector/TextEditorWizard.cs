using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text;

namespace LevelDirectorEditor
{
	using UnityEditor;
	using UnityEngine;

	public class TextEditorWizard : ScriptableWizard
	{
		FileInfo _info;
		Action _callback;

		public static void Show(string title, FileInfo info)
		{
			Show(title, info, null);
		}

		public static void Show(string title, FileInfo info, Action callback)
		{
			TextEditorWizard tew = null;
			if (_data.ContainsKey(info.FullName)) {
				tew = _data[info.FullName];
			} else {
				tew = ScriptableWizard.DisplayWizard<TextEditorWizard>(title);
				_data[info.FullName] = tew;
			}
			tew._info = info;

			//Reading lines individually fixes a problem with line ending characters
			var lines = File.ReadAllLines(info.FullName);
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < lines.Length - 1; ++i) {
				builder.Append(lines[i]);
				builder.Append('\n');
			}
			builder.Append(lines[lines.Length - 1]);
			//
			tew._contents = builder.ToString();
			//tew._contents = System.IO.File.ReadAllText(info.FullName);
			tew._callback = callback;
			tew.Focus();
		}

		static Dictionary<string, TextEditorWizard> _data = new Dictionary<string, TextEditorWizard>();

		void OnDestroy()
		{
			if (_data.ContainsKey(_info.FullName)) {
				_data.Remove(_info.FullName);
			}
		}

		string _contents;
		//bool _dirty = false;
		Vector2 _scrollPosition;

		void OnGUI()
		{
			EditorGUILayout.BeginVertical();
			_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
			_contents = EditorGUILayout.TextArea(_contents, GUILayout.ExpandHeight(true));
			EditorGUILayout.EndScrollView();
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Cancel", GUILayout.ExpandWidth(false))) {
				Close();
			}
			GUILayout.Space(10);

			if (GUILayout.Button("Apply", GUILayout.ExpandWidth(false))) {
				Save();
			}

			if (GUILayout.Button("Save", GUILayout.ExpandWidth(false))) {
				Save();
				this.Close();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}

		void Save()
		{
			try {
				string replaced = _contents.Replace("\n", Environment.NewLine);
				System.IO.File.WriteAllText(_info.FullName, replaced);
				AssetDatabase.Refresh();
				if (_callback != null) {
					_callback();
				}
			} catch (Exception) {
			}
		}
	}
}