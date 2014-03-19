using UnityEngine;
using System;

public class HandController : MonoBehaviour
{
	//public Material GUInormal;
	public Material GUIpie;
	public Texture hand;
	public Texture circle;
	public Texture background;
	public static readonly float MaxValue = 4.0f;
	public static readonly float MinValue = 0.0f;

	public enum HandState
	{
		Clean,
		Dirty,
		Filthy
	}

	public HandState state {
		get { 
			if (Math.Abs(_value - MaxValue) < 0.0001f) {
				return HandState.Clean;
			} 
			if (Ratio >= 0.5f) {
				return HandState.Dirty;
			}
			return HandState.Filthy;
		}
	}

	float _value = 2.0f;

	public float value {
		get { return _value; }
		set { _value = Mathf.Clamp(value, MinValue, MaxValue); }
	}

	public float Ratio { get { return value / (MaxValue - MinValue); } }

	void OnGUI()
	{
		if (Event.current.type.Equals(EventType.Repaint)) {
			Rect drawPos = new Rect(0, 0, Screen.width * 0.1f, Screen.width * 0.1f);
			GUIpie.SetFloat("Value", 1);
			//GUIpie.color = new Color(0.05f, 0.05f, 0.05f);
			Graphics.DrawTexture(drawPos, background, GUIpie);

			GUIpie.SetFloat("Value", Ratio); 
			GUIpie.SetFloat("Clockwise", 0);
			GUIpie.color = new Color((1 - Ratio) * 0.75f, 0.75f, 0, 0.75f);
		
			Graphics.DrawTexture(drawPos, circle, GUIpie);

			GUIpie.SetFloat("Value", 1);
			GUIpie.color = Color.white; 
			switch (state) {
				case HandState.Clean:
					//GUIpie.color = new Color(1, 1, 1, 1);
					break;
				case HandState.Dirty:
					//GUIpie.color = new Color(1, 1, 0, 1);
					break;
				case HandState.Filthy:
					//GUIpie.color = new Color(1, 0, 0, 1);
					break;
			}
			Graphics.DrawTexture(drawPos, hand, GUIpie);
		}
	}
}

