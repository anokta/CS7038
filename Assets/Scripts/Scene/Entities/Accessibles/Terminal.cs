using System;

public class Terminal : Accessible
{

	bool _toTerminal = false;

	public override bool Enter() {
		_toTerminal = true;
		//TODO: Enable when there's content
		//TerminalGUI.instance.Activate(this);
		return false;
	}

	void LateUpdate() {
		if (_toTerminal) {
			_toTerminal = false;
			TerminalGUI.instance.Activate(this);
		}
	}
}