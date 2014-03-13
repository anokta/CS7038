using UnityEngine;

public class Lever : Switchable
{
    public LeverGateType LeverGateType;
    public LeverGateManager Manager;
    public bool Open
    {
        get { return Manager[LeverGateType]; }
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

        audioManager.PlaySFX("Lever");

        var controller = FindObjectOfType<HandController>();
        controller.value -= 0.5f;
    }

    public void UpdateOpenState(bool open)
    {
        spriteRenderer.sprite = open ? LeverOpen : LeverClosed;
    }
}
