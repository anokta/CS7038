using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(GroupWizard))]
public class GroupWizardEditor : Editor
{
	private int _count;

	void OnEnable()
	{
		if (Application.isEditor) {
			var all = GameObject.FindObjectsOfType<GroupWizard>();
			bool flag = false;
			foreach (var gw in all) {
				if (gw.main) {
					flag = true;
					break;
				}
			}
			if (!flag) {
				((GroupWizard)target).main = true;
			}
		}
	}

	void OnDisable()
	{
		if (Application.isEditor && ((GroupWizard)target).main) {
			var all = GameObject.FindObjectsOfType<GroupWizard>();
			foreach (var gw in all) {
				if (gw != this) {
					gw.main = true;
					break;
				}
			}
		}
	}

	public override void OnInspectorGUI()
	{
		var detTarget = (GroupWizard)target;
		var all = GameObject.FindObjectsOfType<GroupWizard>();
		if (all.Length > 1) {
			EditorGUILayout.HelpBox("There are more than one group managers in this scene. Only one will be used.", MessageType.Warning);
			bool isMain = detTarget.main;
			if (isMain) {
				GUI.enabled = false;
				EditorGUILayout.ToggleLeft("This is the main group manager", true);
				GUI.enabled = true;
			} else {
				detTarget.main = EditorGUILayout.ToggleLeft("This is the main group manager", detTarget.main);
				GroupManager.main = detTarget.manager;
			}
			if (detTarget.main) {
				foreach (var gw in all) {
					if (gw != detTarget) {
						gw.main = false;
					}
				}
			}
		}

		if (Application.isPlaying) {
			EditorGUILayout.HelpBox("Cannot edit groups while in-game", MessageType.Info);
			foreach (var group in detTarget.manager.enumerate) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(group.name);
				if (detTarget.manager.activeGroup == group) {
					GUI.enabled = false;
				}
				if (GUILayout.Button("Activate")) {
					detTarget.manager.activeGroup = group;
				}
				GUI.enabled = true;
				EditorGUILayout.EndHorizontal();
			}
			return;
		}
		
		
		detTarget.caseSensitive = EditorGUILayout.ToggleLeft("Case-sensitive", detTarget.caseSensitive);
		HashSet<string> check;
		if (detTarget.caseSensitive) {
			check = new HashSet<string>();
		} else {
			check = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
		}

		EditorGUILayout.LabelField("Groups");
		int toDelete = -1;
		int toUp = -1;
		int toDown = -1;
		for (int i = 0; i < detTarget.states.Count; ++i) {
			string name = detTarget.states[i];

			if (!StringExtensions.IsNullOrWhitespace(name) && check.Contains(name)) {
				//detTarget.states[i] = null;
				EditorGUILayout.HelpBox(
						string.Format("'{0}' is already a group", name),
						MessageType.Warning);
			}
			EditorGUILayout.BeginHorizontal();
			{
				string newName = EditorGUILayout.TextField(name);
				if (newName != null) {
					newName = newName.Trim();
				}
				detTarget.states[i] = newName;
				check.Add(newName);

				if (i == 0) {
					GUI.enabled = false;
				}
				if (GUILayout.Button('\u25B2'.ToString())) {
					GUI.FocusControl(null);
					toUp = i;
				}
				GUI.enabled = true;
				if (i == detTarget.states.Count - 1) {
					GUI.enabled = false;
				}
				if (GUILayout.Button('\u25BC'.ToString())) {
					GUI.FocusControl(null);
					toDown = i;
				}
				GUI.enabled = true;
				if (GUILayout.Button("Delete")) {
					GUI.FocusControl(null);
					toDelete = i;
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.BeginHorizontal(); 
		{
			if (GUILayout.Button("Add")) {
				detTarget.states.Add("");
			}
			if (detTarget.states.Count == 0) {
				GUI.enabled = false;
			}
			if (GUILayout.Button("Clear")) {
				if (EditorUtility.DisplayDialog(
                "Confirm", "Are you sure you want to remove all the groups?", "Yes", "No")) {
					detTarget.states.Clear();
				}
			}
			GUI.enabled = true;
		}
		EditorGUILayout.EndHorizontal();
		if (toDelete != -1) {
			detTarget.states.RemoveAt(toDelete);
		}
		if (toUp != -1) {
			string temp = detTarget.states[toUp];
			detTarget.states[toUp] = detTarget.states[toUp - 1];
			detTarget.states[toUp - 1] = temp;
		}
		if (toDown != -1) {
			string temp = detTarget.states[toDown];
			detTarget.states[toDown] = detTarget.states[toDown + 1];
			detTarget.states[toDown + 1] = temp;
		}
	}


}

