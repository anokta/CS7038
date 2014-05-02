using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class Printer : MonoBehaviour
{
	public static Printer instance {
		get;
		private set;
	}

	void Awake() {
		lines = new Queue<string>();
		if (!UnityEngine.Debug.isDebugBuild) {
			Destroy(this);
		} else {
			instance = this;
		}
	}

	[SerializeField]
	private int maxLines = 10;

	Queue<string> lines;

	[Conditional("DEBUGPRINT")]
	public static void Print(object obj) {
		if (instance == null) {
			return;
		}
		instance.lines.Enqueue(obj.ToString());
		while (instance.lines.Count > instance.maxLines) {
			instance.lines.Dequeue();
		}
	}

	[Conditional("DEBUGPRINT")]
	void OnGUI() {
		GUILayout.BeginVertical();
		foreach (string text in lines) {
			GUILayout.Label(text, GUIManager.GetSkin().GetStyle("debug text"));
		}
		GUILayout.EndVertical();
	}
}

