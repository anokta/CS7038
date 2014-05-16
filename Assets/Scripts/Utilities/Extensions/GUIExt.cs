using System;
using UnityEngine;

public static class GUIExt
{
	public static void LabelOutlined(Rect position, string text, GUIStyle style, Color outColor){

		var backup = style.normal.textColor;

		style.normal.textColor = outColor;
		position.x--;
		GUI.Label(position, text, style);
		position.x +=2;
		GUI.Label(position, text, style);
		position.x--;
		position.y--;
		GUI.Label(position, text, style);
		position.y +=2;
		GUI.Label(position, text, style);
		position.y--;

		style.normal.textColor = backup;
		GUI.Label(position, text, style);
	}

	public static void LabelOutlined(Rect position, string text, GUIStyle style, Color outColor, int offset){

		var backup = style.normal.textColor;

		style.normal.textColor = outColor;
		position.x -= offset;
		GUI.Label(position, text, style);
		position.x += 2 * offset;
		GUI.Label(position, text, style);
		position.x -= offset;
		position.y -= offset;
		GUI.Label(position, text, style);
		position.y += 2 * offset;
		GUI.Label(position, text, style);
		position.y -= offset;

		style.normal.textColor = backup;
		GUI.Label(position, text, style);
	}
}