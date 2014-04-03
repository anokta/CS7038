using System.Collections.Generic;

public class LeverGateManager
{
    private readonly List<Lever> levers;
    private readonly List<Gate> gates;

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
