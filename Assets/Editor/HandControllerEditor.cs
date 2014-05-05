using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(HandController))]
public class HandControllerEditor : Editor
{
	HandController _target;

	void OnEnable() {
		_target = (HandController)target;
	}

	bool _foldout;

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		_target.value = EditorGUILayout.IntField(_target.value);
		_target.value = EditorGUILayout.IntSlider(_target.value, HandController.MinValue, HandController.InfectionThreshold);
		_foldout = EditorGUILayout.Foldout(_foldout, "Info");
		if (_foldout) {
			GUI.enabled = false;
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("State");
				EditorGUILayout.TextField(_target.state.ToString());
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Ratio");
				EditorGUILayout.FloatField(_target.Ratio);
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Min");
				EditorGUILayout.FloatField(HandController.MinValue);
				GUILayout.Label("Max");
				EditorGUILayout.FloatField(HandController.MaxValue);
			}
			GUILayout.EndHorizontal();
			GUI.enabled = true;
		}
	}
}

