using UnityEngine;

public class Lever : Switchable
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
            spriteRenderer.sprite = open ? LeverOpen : LeverClosed;
        }
    }

    public Sprite LeverOpen;
    public Sprite LeverClosed;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    public override void Switch()
    {
        Manager.Switch(LeverGateType);

        AudioManager.PlaySFX("Lever");

        playerHand.SpoilHand(-0.55f, GetInstanceID());
    }

    public void SwitchState()
    {
        Open = !Open;
    }
}
