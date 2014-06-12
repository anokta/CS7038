using System.Collections.Generic;
using UnityEngine;

public class LeverGateManager
{
    private readonly List<Lever> levers;
    private readonly List<Gate> gates;

	public static readonly Color Color1 = new Color(1, 0.9f, 0.9f);
	public static readonly Color Color2 = new Color(0.9f, 1, 0.9f);
	public static readonly Color Color3 = new Color(0.9f, 0.9f, 1);

	public static Color GetColorOf(LeverGateType type) {
		switch (type) {
			case LeverGateType.Type1:
				return Color1;
			case LeverGateType.Type2:
				return Color2;
			case LeverGateType.Type3:
				return Color3;
			default:
				return Color.white;
		}
	}

    public LeverGateManager()
    {
        levers = new List<Lever>();
        gates = new List<Gate>();
    }

    public void Add(Lever lever)
    {
        lever.Manager = this;
        levers.Add(lever);
    }

    public void Add(Gate gate)
    {
        gate.Manager = this;
        gates.Add(gate);
    }

    public void Switch(LeverGateType type)
    {
		foreach (var gate in gates)
		{
			if (gate.LeverGateType == type)
			{
				if (!gate.CanSwitch()) {
					return;
				}
			}
		}

        foreach (var lever in levers)
        {
            if (lever.LeverGateType == type)
            {
                lever.SwitchState();
            }
        }

		foreach (var gate in gates)
		{
			if (gate.LeverGateType == type)
			{
				gate.SwitchState();
			}
		}
    }
}
