using UnityEngine;
using System.Collections;

public class GateCleanReactive : Accessible {

    private bool open;

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

        open |= (playerHand.state == HandController.HandState.Clean);

        UpdateOpenState();
    }

    public override bool Enter()
    {
        return open;
    }

    public void UpdateOpenState()
    {
        collider2D.enabled = !open;

        spriteRenderer.sprite = open ? GateOpen : GateClosed;
    }
}
