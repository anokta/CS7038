using UnityEngine;

public class Gate : Accessible
{
    public LeverGateType LeverGateType;
    public LeverGateManager Manager;

    private bool open;
    public bool Open
    {
        get { return open; }
        set
        {
            open = value;
            collider2D.enabled = !open;
            spriteRenderer.sprite = open ? GateOpen : GateClosed;
        }
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

    public void SwitchState()
    {
        Open = !Open;
    }
}
