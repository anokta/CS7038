using System.Collections.Generic;

public class LeverGateManager
{
    private readonly List<Lever> levers;
    private readonly List<Gate> gates;
    private readonly Dictionary<LeverGateType, bool> open;

    public LeverGateManager()
    {
        levers = new List<Lever>();
        gates = new List<Gate>();
        open = new Dictionary<LeverGateType, bool>();
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

    public bool this[LeverGateType type]
    {
        get
        {
            if (!open.ContainsKey(type)) open[type] = false;
            return open[type];
        }
        set
        {
            Set(type, value);
        }
    }

    public void Switch(LeverGateType type)
    {
        this[type] = !this[type];
    }

    private void Set(LeverGateType type, bool newOpen)
    {
        open[type] = newOpen;

        foreach (var lever in levers)
        {
            if (lever.LeverGateType == type)
            {
                lever.UpdateOpenState(newOpen);
            }
        }

        foreach (var gate in gates)
        {
            if (gate.LeverGateType == type)
            {
                gate.UpdateOpenState(newOpen);
            }
        }
    }
}
