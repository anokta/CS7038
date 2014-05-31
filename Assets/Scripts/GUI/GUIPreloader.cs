using UnityEngine;
using System.Collections;
using Grouping;

public class GUIPreloader : MonoBehaviour
{
	int switcheroo = 0;
	
	// Update is called once per frame
	void Update()
	{
		if ((++switcheroo) == 2) {
			GroupManager.main.activeGroup = GroupManager.main.group["Loading"];
			Destroy(this);
		}
	}

	void OnGUI() {
		string progress = "Loading...";
		var vec2 = GUIManager.Style.loading.CalcSize(new GUIContent(progress));
		GUI.Label(
			new Rect((Screen.width - vec2.x) / 2, (Screen.height - vec2.y) / 2 - vec2.y * 0.75f, vec2.x, vec2.y),
			progress, GUIManager.Style.loading);
	}
}

