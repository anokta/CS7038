using System;
using UnityEngine;
using System.Collections.Generic;

public class TmxObject
{
	public readonly Rect position;
	public readonly PropertyReader properties;
	//public readonly ObjType Type;
	public Collider2D collider;

	public enum ObjType {
		Rect, Ellipse, Tile
	}

	public readonly int id;

	public TmxObject(Rect rect, int id, Dictionary<string, string> dict) {
		this.id = id;
		properties = new PropertyReader(dict);
		position = rect;
		//Type = type;
		collider = null;
	}
}
