using System;

public class Terminal : Accessible
{
	public override bool Enter() {
		
		TerminalGUI.instance.Activate(this);
		return false;
	}
}