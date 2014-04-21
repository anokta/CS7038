using UnityEngine;
using System.Collections;

public static class RectExtensions
{
	public static Rect Scale(this Rect rect, float scale) {
		return new Rect(rect.x * scale,	rect.y * scale,	rect.width * scale, rect.height * scale);
	}

	public static RectOffset Scaled(this RectOffset rect, float scale) {
		return new RectOffset(
			Mathf.RoundToInt(rect.left * scale), 
			Mathf.RoundToInt(rect.right * scale),
			Mathf.RoundToInt(rect.top * scale),
			Mathf.RoundToInt(rect.bottom * scale));
	}

	public static void Scale(this RectOffset rect, float scale) {
		rect.left = Mathf.RoundToInt(rect.left * scale);
		rect.right = Mathf.RoundToInt(rect.right * scale);
		rect.top = Mathf.RoundToInt(rect.top * scale);
		rect.bottom = Mathf.RoundToInt(rect.bottom * scale);
	}

	public static void ScaleBy(this RectOffset rect, RectOffset other, float scale) {
		rect.left = Mathf.RoundToInt(other.left * scale);
		rect.right = Mathf.RoundToInt(other.right * scale);
		rect.top = Mathf.RoundToInt(other.top * scale);
		rect.bottom = Mathf.RoundToInt(other.bottom * scale);
	}

	public static RectOffset Clone(this RectOffset rect) {
		return new RectOffset(rect.left, rect.right, rect.top, rect.bottom);
	}
}

