using System;
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
	
}

