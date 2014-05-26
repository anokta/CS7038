using System;
using UnityEngine;
using System.Collections.Generic;

public class TmxObject
{
	public readonly Rect position;
	public readonly PropertyReader properties;
	//public readonly TriggerAction.ActionType type;
	public readonly ObjectType objectType;
	public readonly string type;
	public Collider2D collider;

	public enum ObjectType {
		Rect, Ellipse, Tile
	}

	public TmxObject(Rect rect, ObjectType tType, string type, Dictionary<string, string> dict) {
		objectType = tType;
		this.type = type;
		properties = new PropertyReader(dict);
		position = rect;
		//Type = type;
		collider = null;
	}
}
