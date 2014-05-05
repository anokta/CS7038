using System.Globalization;
using System.Text;
using UnityEditor;
using UnityEngine;
using HandyGestures;

[CustomEditor(typeof(HandyDetector))]
public class HandyDetectorEditor : Editor
{
	public static bool foldout;
	static readonly string[] _maskStrings;

	static HandyDetectorEditor()
	{
		if (_maskStrings == null) {
			_maskStrings = new string[32];
			for (int i = 0; i < 32; ++i) {
				if (string.IsNullOrEmpty(LayerMask.LayerToName(i))) {
					_maskStrings[i] = i.ToString(CultureInfo.InvariantCulture);
				} else {
					_maskStrings[i] = i + " - " + LayerMask.LayerToName(i);
				}
			}
		}
	}

	public override void OnInspectorGUI()
	{
		var detTarget = (HandyDetector)target;

		detTarget.DisableCasting = EditorGUILayout.ToggleLeft("Disable casting", detTarget.DisableCasting);

		detTarget.longTime = EditorGUILayout.FloatField(
			"Tap Threshold", detTarget.longTime);
		if (detTarget.longTime < 0) {
			detTarget.longTime = 0;
		}

		detTarget.flingTime = EditorGUILayout.FloatField("Fling Threshold", detTarget.flingTime);
		if (detTarget.flingTime < 0) {
			detTarget.flingTime = 0;		
		}

		detTarget.moveThreshold = EditorGUILayout.FloatField("Move Threshold", detTarget.moveThreshold);
		if (detTarget.moveThreshold < 0) {
			detTarget.moveThreshold = 0;
		}

		#region Interceptor and Default Object

		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.LabelField("Interceptor", GUILayout.MaxWidth(120));
			if (detTarget.interceptor == null) {
				GUI.enabled = false;
			}
			if (GUILayout.Button("Info")) {
				ShowInfoFor(
					detTarget.interceptor.gameObject,
					"All gesture detection will go through the interceptor before any other objects in the scene.");
			}
			if (GUILayout.Button("Clear")) {
				detTarget.interceptor = null;
			}
			GUI.enabled = true;
		}
		EditorGUILayout.EndHorizontal();

		detTarget.interceptor =
			EditorGUILayout.ObjectField(detTarget.interceptor, typeof(Transform), true, null) as
			Transform;

		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.LabelField("Default", GUILayout.MaxWidth(120));
			if (detTarget.defaultObject == null) {
				GUI.enabled = false;
			}
			if (GUILayout.Button("Info")) {
				ShowInfoFor(
					detTarget.defaultObject.gameObject,
					"This object handles all detected gestures that have not yet been handled.");
			}
			if (GUILayout.Button("Clear")) {
				detTarget.defaultObject = null;
			}
			GUI.enabled = true;
		}
		EditorGUILayout.EndHorizontal();

		detTarget.defaultObject =
			EditorGUILayout.ObjectField(detTarget.defaultObject, typeof(Transform), true, null) as
			Transform;
		#endregion

		EditorGUILayout.Space();

		//HandyDetector.CollisionMode mode;

		detTarget.CastMode = (HandyDetector.CollisionMode)EditorGUILayout.EnumPopup(detTarget.CastMode);
		if (detTarget.CastMode == HandyDetector.CollisionMode.Mode3D) {
			#region 3d Mode
			EditorGUILayout.BeginHorizontal();
			{
				detTarget.CollisionMethod =
				(HandyDetector.CollisionMethodType)
				EditorGUILayout.EnumPopup("Collision Function", detTarget.CollisionMethod);
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			{

				switch (detTarget.CollisionMethod) {
					case HandyDetector.CollisionMethodType.Custom:
						detTarget.CustomFunction =
						(EditorGUILayout.ObjectField(
							detTarget.CustomFunction, typeof(CollisionFunctor), true, null)
							as CollisionFunctor);
						if (detTarget.CustomFunction == null) {
							GUI.enabled = false;
						}
						if (GUILayout.Button("Clear")) {
							//detTarget.defaultObject = null;
							detTarget.CustomFunction = null;
						}
						GUI.enabled = true;
						break;
					case HandyDetector.CollisionMethodType.Spherecast:
						detTarget.sphereRadius = EditorGUILayout.FloatField(
							"Radius", detTarget.sphereRadius);
						if (detTarget.sphereRadius < 0) {
							detTarget.sphereRadius = 0;
						}
						break;
				}

			}
			EditorGUILayout.EndHorizontal();
			detTarget.castDistance = EditorGUILayout.FloatField("Distance", detTarget.castDistance);
			if (detTarget.castDistance < 0) {
				detTarget.castDistance = 0;
			}

			#endregion
		} else {
			#region 2d Mode
			detTarget.CollisionMethod2D =
				(HandyDetector.CollisionMethodType2D)
				EditorGUILayout.EnumPopup("Collision Function", detTarget.CollisionMethod2D);

			EditorGUILayout.BeginHorizontal();
			{

				switch (detTarget.CollisionMethod2D) {
					case HandyDetector.CollisionMethodType2D.Circle:
						detTarget.circleRadius = EditorGUILayout.FloatField(
							"Radius", detTarget.circleRadius);
						if (detTarget.circleRadius < 0) {
							detTarget.circleRadius = 0;
						}
						break;
					case HandyDetector.CollisionMethodType2D.Custom:
						detTarget.CustomFunction2D =
							(EditorGUILayout.ObjectField(
							detTarget.CustomFunction2D, typeof(CollisionFunctor2D), true, null)
								as CollisionFunctor2D);
						if (detTarget.CustomFunction2D == null) {
							GUI.enabled = false;
						}
						if (GUILayout.Button("Clear")) {
							//detTarget.defaultObject = null;
							detTarget.CustomFunction = null;
						}
						GUI.enabled = true;
						break;
				}
			}
			EditorGUILayout.EndHorizontal();

			detTarget.minDepth = EditorGUILayout.FloatField("Min Depth", detTarget.minDepth);
			detTarget.maxDepth = EditorGUILayout.FloatField("Max Depth", detTarget.maxDepth);

			#endregion
		}
		//detTarget.layerMask = EditorGUILayout.MaskField((int)detTarget.layerMask, maskStrings);

		//EditorGUILayout.LayerField(val);
		//detTarget.layerMask = -1;
		int count = 0;
		for (int i = 0; i < 32; ++i) {
			if (!string.IsNullOrEmpty(LayerMask.LayerToName(i))
			    && ((1 << i) & detTarget.layerMask) != 0) {
				++count;
			}
		}

		foldout = EditorGUILayout.Foldout(foldout, "Layers (" + count + " selected)");
		if (foldout) {
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Clear")) {
				detTarget.layerMask = 0;
			}
			if (GUILayout.Button("Select All")) {
				detTarget.layerMask = -1;
			}
			EditorGUILayout.EndHorizontal();
			for (int i = 0; i < 32; ++i) {
				string layerToName = LayerMask.LayerToName(i);
				bool available = !string.IsNullOrEmpty(layerToName);
				if (available) {
					bool check = EditorGUILayout.Toggle(
						             i + " - " + layerToName,
						             ((1 << i) & detTarget.layerMask) != 0);
					if (check) {
						detTarget.layerMask = (int)(((uint)1 << i) | (uint)detTarget.layerMask);
					} else {
						detTarget.layerMask = (int)(~((uint)1 << i) & (uint)detTarget.layerMask);
					}
				}
			}
		}
	}
	//enum CollisionMode { Raycast, Spherecast, Custom };
	//enum LayerMode { All, One, Multi };
	void ShowInfoFor(object o, string message)
	{
		var mb = o as GameObject;
		if (mb == null) {
			return;
		}

		bool flag = false;
		var sb = new StringBuilder();
		sb.Append(message);
		sb.Append("\n\n");
		var gestures = new StringBuilder();
		if (mb.GetComponent(typeof(IPress)) != null) {
			flag = true;
			gestures.Append("Press");
		}
		if (mb.GetComponent(typeof(ITap)) != null) {
			gestures.Append((flag ? ", " : "") + "Tap");
			flag = true;
		}
		if (mb.GetComponent(typeof(ILongPress)) != null) {
			gestures.Append((flag ? ", " : "") + "Longpress");
			flag = true;
		}
		if (mb.GetComponent(typeof(IPan)) != null) {
			gestures.Append((flag ? ", " : "") + "Pan");
			flag = true;
		}
		if (mb.GetComponent(typeof(IFling)) != null) {
			gestures.Append((flag ? ", " : "") + "Fling");
			flag = true;
		}
		/*	if (mb.GetComponent(typeof(ISwipe)) != null) {
			gestures.Append((flag ? ", " : "") + "Swipe");
			flag = true;
		}
		if (mb.GetComponent(typeof(IPinch)) != null) {
			gestures.Append((flag ? ", " : "") + "Pinch");
			flag = true;
		}*/
		if (!flag) {
			sb.Append("This object does not support any gestures");
		} else {
			sb.Append("The following gestures are supported:\n" + gestures);
		}

		EditorUtility.DisplayDialog(
			"Gesture information for " + mb.name, sb.ToString(), "Ok");

		//EditorUtility.DisplayDialog(" " + detTarget.interceptor.ToString(), "", "OKAY");
	}
}
