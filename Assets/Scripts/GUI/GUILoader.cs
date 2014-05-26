using UnityEngine;
using System.Collections;
using Grouping;

public class GUILoader : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		GroupManager.main.group["Loading"].Add(this);
		count -= interval;
	}
	// Update is called once per frame
	void FixedUpdate()
	{
		count += interval;
	}

	int count;

	int interval = 5;

	void OnGUI() {
		if (Event.current.type.Equals(EventType.Repaint)) {
		
			if (count >= GUIManager.StyleList.Count) {
				GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
				Destroy(this);
				return;
			}
			for (int i = count; count >= 0 && i < interval && count + i < GUIManager.StyleList.Count; ++i) {
				GUI.Button(
					new Rect(
						Screen.width / (float)(i + 1), Screen.height * 2,
						Screen.width / (float)interval, Screen.height / (float)interval),
					"",
					GUIManager.StyleList[count + i].style);
			}
			//count += interval;
		}
		string progress = "Loading... " + Mathf.Floor((count / (float)GUIManager.StyleList.Count) * 100) + "%";
		var vec2 = GUIManager.Style.loading.CalcSize(new GUIContent(progress));
		GUI.Label(
			new Rect((Screen.width - vec2.x)/2, (Screen.height - vec2.y)/2, vec2.x, vec2.y),
			progress, GUIManager.Style.loading);
	}
}

