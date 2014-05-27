using System;
using UnityEngine;

public class Trigger
{
	public readonly bool repeat;
	public readonly bool persist;
	public readonly Rect area;

	public readonly string message;
	
	[Flags]
	//This is used as a flag: All values must be distinct powers of 2!
	public enum ActionType {
		Any = 1,
		Handy = 2,
		Other = 4,
		Destroy = 8,
		On = 16,
		Off = 32,
		Task = 64
	}

	public readonly ActionType type;

	public Trigger(TmxObject obj, LevelSettings settings)
	{
		repeat = obj.properties.GetInt("Repeat", 0) != 0;
		
		string targetDialogue;
		if (obj.properties.GetTag("Dialogue", out targetDialogue)) {
			var dia = settings.dialogueMap[targetDialogue];
			Run += () => {
				if (!LevelManager.instance.settings.HasDialogueFlag(dia)) {
					LevelManager.instance.settings.StoreDialogueFlag(dia);
					DialogueManager.DialogueComplete = GameWorld.GoBackToLevel;
					DialogueManager.ActivateDialogue(dia);
				}
			};
		}

		area = obj.position;
		Trigger.ActionType at = Trigger.ActionType.Any;
		try {
			at = (Trigger.ActionType)Enum.Parse(typeof(Trigger.ActionType), obj.type, true);
		} catch (ArgumentException ae) {
			Debug.LogError(ae.Message);
		}
		type = at;
	}

	public event System.Action Run;

	public void OnRun() {
		if (Run!= null) {
			Run();
		}
	}
}
