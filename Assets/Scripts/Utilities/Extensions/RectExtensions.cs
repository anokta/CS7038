using UnityEngine;
using System.Collections;

public static class RectExtensions
{
	public static Rect Scale(this Rect rect, float scale) {
		return new Rect(rect.x * scale,	rect.y * scale,	rect.width * scale, rect.height * scale);
	}

	public static Rect Add(this Rect rect, Rect other) {
		return new Rect(
			rect.x + other.x,
			rect.y + other.y,
			rect.width + other.width,
			rect.height + other.height);
	}

	public static Rect Add(this Rect rect, float x, float y, float width, float height) {
		return new Rect(rect.x + x, rect.y + y, rect.width + width, rect.height + height);
	}

	public static Rect Centered(this Rect rect) {
		return new Rect(
			Mathf.Round((Screen.width - rect.width) * 0.5f),
			Mathf.Round((Screen.height - rect.height) * 0.5f),
			rect.width, rect.height);
	}

	public static Rect Rounded(this Rect rect) {
		return new Rect(
			Mathf.Round(rect.x), Mathf.Round(rect.y),
			Mathf.Round(rect.width), Mathf.Round(rect.height));
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

