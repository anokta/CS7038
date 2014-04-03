using UnityEngine;

public class Gate : Accessible
{
    public LeverGateType LeverGateType;
    public LeverGateManager Manager;

    private bool open;
    public virtual bool Open
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

    protected override void Update()
    {
        base.Update();
        spriteRenderer.sortingOrder = Entity.Place(transform.position.y) - 1;
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
