using System;
using UnityEditor;


[CustomEditor(typeof(Preloader))]
public class PreloaderEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var detTarget = (Preloader)target;
		EditorGUILayout.HelpBox(
			"This object is used to preload textures into memory. It gets destroyed in runtime after all textures have preloaded.",
			MessageType.Info, true);

		DrawDefaultInspector();
	}
}