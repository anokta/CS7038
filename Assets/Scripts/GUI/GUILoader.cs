using UnityEngine;
using System.Collections;
using Grouping;

public class GUILoader : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		//GroupManager.main.group["Loading"].Add(this);
		count -= interval;
	}
	// Update is called once per frame
	void FixedUpdate()
	{
		count += interval;
		if (count >= GUIManager.StyleList.Count) {
			Debug.Log(GUIManager.StyleList.Count);
			GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
			Destroy(this);
		}
	}

	[SerializeField]
	Texture pixel;

	int count;

	int interval = 5;

	void OnGUI()
	{
		//	if (Event.current.type.Equals(EventType.Repaint)) {
		
		if (count >= GUIManager.StyleList.Count) {
			return;
		}
		for (int i = count; count >= 0 && i < interval && count + i < GUIManager.StyleList.Count; ++i) {
			GUI.Button(
				new Rect(0, 0, Screen.width, Screen.height),
				"",
				GUIManager.StyleList[count + i].style);
		}
		GUI.color = Color.black;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), pixel);
		GUI.color = Color.white;	
		//count += interval;

		string progress = "Loading... " + Mathf.Floor((count / (float)GUIManager.StyleList.Count) * 100) + "%";
		var vec2 = GUIManager.Style.loading.CalcSize(new GUIContent(progress));
		GUI.Label(
			new Rect((Screen.width - vec2.x) / 2, (Screen.height - vec2.y) / 2, vec2.x, vec2.y),
			progress, GUIManager.Style.loading);
		//}

	}
}

