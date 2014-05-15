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
			GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Loading... " + count / (float)GUIManager.StyleList.Count);
			if (count >= GUIManager.StyleList.Count) {
				GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
				Destroy(this);
				return;
			}
			for (int i = count; count >= 0 && i < interval && count + i < GUIManager.StyleList.Count; ++i) {
				GUI.Button(
					new Rect(
						Screen.width / (float)(i + 1), Screen.height,
						Screen.width / (float)interval, Screen.height / (float)interval),
					"",
					GUIManager.StyleList[count + i].style);
			}
			//count += interval;
		}
	}
}

