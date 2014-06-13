using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings {

	public int maxScore {
		get; set;
	}
	
	public int minScore {
		get; set;
	}

	public DialogueInstance intro { get; set; }
	public DialogueInstance outro { get; set; }

	public DialogueMap dialogueMap { get; set; }
	private HashSet<DialogueInstance> usedMap;

	public LevelSettings() {
		usedMap = new HashSet<DialogueInstance>();
	}
	
	public void ResetStoredDialogues() {
		usedMap.Clear();
	//	intro = null;
		//outro = null;
		//dialogueMap = null;
		//minScore = 0;
		//maxScore = 0;
		//Debug.Log("Hard Reset gaes");
	}

	public void StoreDialogueFlag(DialogueInstance instance) {
		usedMap.Add(instance);
	}

	public bool HasDialogueFlag(DialogueInstance instance) {
		return usedMap.Contains(instance);
	}

	public int width {get; private set; }
	public int height {get; private set; }
	public void SetFloorData(FloorType[,] data) {
		height = data.GetLength(0);
		width = data.GetLength(1);
		_data = data;
	}

	public FloorType GetFloor(float x, float y) {
		int xx = Mathf.FloorToInt(x + 0.5f);
		int yy = Mathf.FloorToInt(y + 0.5f);
		if (xx < 0 || xx >= width || yy < 0 || yy >= height) {
			return FloorType.Outside;
		}
		return _data[xx, yy];
	}

	FloorType[,] _data;

	public enum FloorType
	{
		Normal,
		Outside,
		HeatPad,
		Carpet
	}
}

