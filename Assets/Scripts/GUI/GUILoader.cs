using UnityEngine;
using System.Collections;
using Grouping;

public class GUILoader : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		count -= _interval;
	}

	float _totalWidth;
	[SerializeField]
	Texture pixel;

	int count;

	[SerializeField]
	int _interval = 1;
	[SerializeField]
	float _barWidth = 0.5f;
	[SerializeField]
	float _borderWidth = 0.01f;
	[SerializeField]
	float _innerBorderWidth = 0.01f;

	void Update() {
		_totalWidth = Screen.height * _barWidth;
	}

	void OnGUI()
	{
		if (Event.current.type.Equals(EventType.Repaint)) {
			count += _interval;
		}
		for (int i = 0; count >= 0 && i < _interval && count + i < GUIManager.StyleList.Count; ++i) {
			GUI.Button(
				new Rect(0, 0, Screen.width, Screen.height),
				"",
				GUIManager.StyleList[count + i].style);
		}
		GUI.color = Color.black;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), pixel);
		GUI.color = Color.white;	
		//count += interval;

		if (count >= 0) {
			string progress = "Loading...";
			var vec2 = GUIManager.Style.loading.CalcSize(new GUIContent(progress));
			GUI.Label(
				new Rect((Screen.width - vec2.x) / 2, (Screen.height - vec2.y) / 2-vec2.y * 0.75f, vec2.x, vec2.y),
				progress, GUIManager.Style.loading);

			GUI.color = Color.white;
			var bar = new Rect((Screen.width - _totalWidth) / 2, (Screen.height - vec2.y) / 2 + vec2.y * 0.75f, _totalWidth, vec2.y/2);
			GUI.DrawTexture(bar, pixel);
			GUI.color = Color.black;
			var innerB = bar.Expanded(Mathf.Round(Mathf.Min(-1, -Screen.height * _borderWidth)));
			GUI.DrawTexture(innerB, pixel);
			GUI.color = new Color(0.1f, 0.6f, 0.2f);
			var inner = innerB.Expanded(Mathf.Round(Mathf.Min(-1, -Screen.height * _innerBorderWidth)));
			inner.width = Mathf.Min(inner.width, inner.width * 
				(float)count / GUIManager.StyleList.Count);
			GUI.DrawTexture(inner, pixel);
			GUI.color = Color.white;
		}
		//}

		if (Event.current.type.Equals(EventType.Repaint) && count >= GUIManager.StyleList.Count) {
			GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
			Destroy(this);
		}
	}
}

