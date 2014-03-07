using UnityEngine;

public class Gate : Accessible
{
    public LeverGateType LeverGateType;
    public LeverGateManager Manager;
    public bool Open
    {
        get { return Manager[LeverGateType]; }
    }

    // TO BE CHANGED
    public Sprite GateOpen;
    public Sprite GateClosed;
    //

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    public override bool Enter()
    {
        return Open;
    }

    public void UpdateOpenState(bool open)
    {
        collider2D.enabled = !open;

        spriteRenderer.sprite = open ? GateOpen : GateClosed;
    }
}
