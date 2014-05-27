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
}

