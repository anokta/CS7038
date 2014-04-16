using UnityEngine;
using System.Collections;

public static class RectExtensions
{
	public static Rect Scale(this Rect rect, float scale) {
		return new Rect(rect.x * scale,	rect.y * scale,	rect.width * scale, rect.height * scale);
	}

	public static RectOffset Scale(this RectOffset rect, float scale) {
		return new RectOffset(
			Mathf.RoundToInt(rect.left * scale), 
			Mathf.RoundToInt(rect.right * scale),
			Mathf.RoundToInt(rect.top * scale),
			Mathf.RoundToInt(rect.bottom * scale));
	}

	public static RectOffset Clone(this RectOffset rect) {
		return new RectOffset(
			Mathf.RoundToInt(rect.left), 
			Mathf.RoundToInt(rect.right),
			Mathf.RoundToInt(rect.top),
			Mathf.RoundToInt(rect.bottom));
	}
}

