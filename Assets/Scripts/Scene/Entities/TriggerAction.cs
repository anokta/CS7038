using System;
using UnityEngine;

public class TriggerAction
{
	public readonly bool repeat;
	public readonly Rect area;

	public TriggerAction(TmxObject obj, LevelSettings settings)
	{
		repeat = obj.properties.GetInt("Repeat", 0) != 0;
		
		string targetDialogue;
		if (obj.properties.GetTag("Dialogue", out targetDialogue)) {
			var dia = settings.dialogueMap[targetDialogue];
			Run += () => {
				DialogueManager.DialogueComplete = GameWorld.GoBackToLevel;
				DialogueManager.ActivateDialogue(dia);
			};
		}

		area = obj.position;
	}

	public event System.Action Run;

	public void OnRun() {
		if (Run!= null) {
			Run();
		}
	}
}
