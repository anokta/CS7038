using System;
using System.IO;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using System.Configuration;

namespace LevelDirectorEditor
{
	public class LevelPickerWizard : ScriptableWizard
	{
		static LevelPickerWizard _instance;
		static bool _showOthers;
		IEnumerable<FileInfo> _filterFiles;
		DirectoryInfo _folder;
		IEnumerable<FileInfo> _included;
		IEnumerable<FileInfo> _invalid;

		public delegate IEnumerable<FileInfo> FilterCallback();

		public delegate void SelectionCallback(FileInfo selected);

		FilterCallback _filter;
		SelectionCallback _callback;
		DirectoryInfo _levelDir;

		bool _viewOnly;

		public static void Show(DirectoryInfo levelDir, FilterCallback filter, SelectionCallback callback)
		{
			//ListSelector ls = null;
			if (_instance == null) {
				_instance = ScriptableWizard.DisplayWizard<LevelPickerWizard>("Select level");
			}
			_instance._viewOnly = callback == null;
			_instance._levelDir = levelDir;
			_instance._filter = filter;
			_instance._callback = callback;

			_instance.Refresh();

			_instance.Focus();
		}

		void OnLostFocus()
		{
			//DestroyImmediate(this);
		}

		Vector2 _scrollView;

		void OnGUI()
		{
			Refresh();
			GUILayout.BeginVertical();
			{
				_scrollView = GUILayout.BeginScrollView(_scrollView);
				{
					foreach (var file in _included) {
						GUILayout.BeginHorizontal();
						{
							if (GUILayout.Button("-", GUILayout.ExpandWidth(false))) {
								if (EditorUtility.DisplayDialog(
									    "Confirm", "Are you sure you want to delete this file? This operation cannot be undone.",
									    "Yes", "No")) {
									File.Delete(file.FullName);
								}
							}
							GUILayout.Label(file.Name);
							GUILayout.FlexibleSpace();
							if (!_viewOnly) {
								if (GUILayout.Button("Select", GUILayout.ExpandWidth(false))) {
									if (_callback != null) {
										_callback(file);
										Close();
									}
								}
							}
						}
						GUILayout.EndHorizontal();
					}
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndScrollView();
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Refresh", GUILayout.ExpandWidth(false))) {
					Refresh();
				}
				if (GUILayout.Button("Cancel", GUILayout.ExpandWidth(false))) {
					Close();
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}

		class Comparer : IEqualityComparer<FileInfo>
		{
			public bool Equals(FileInfo x, FileInfo y)
			{
				return x.FullName.Equals(y.FullName, StringComparison.OrdinalIgnoreCase);
			}

			public int GetHashCode(FileInfo obj)
			{
				return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.FullName);
			}
		}

		static Comparer _comparer = new Comparer();

		void OnDestroy() {
			_instance = null;
		}

		void Refresh()
		{
			if (_filter != null) {
				_filterFiles = _filter();
			} else {
				_filterFiles = new FileInfo[0];
			}
			var allXml = _levelDir.GetFiles("*.xml", SearchOption.TopDirectoryOnly);
			_included = allXml.Except(_filterFiles, _comparer);
			_invalid = _levelDir.GetFiles("*", SearchOption.TopDirectoryOnly).Except(
				allXml, _comparer);
		}

		public static void CloseInstance() {
			if (_instance != null) {
				_instance.Close();
			}
		}
	}
}

