using UnityEngine;

public class Crate : Pushable
{
    public Crate()
    {
        MovingWithPlayer = true;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    public override bool Push(Vector3 direction)
    {
        bool canPush = base.Push(direction);

        if (canPush)
        {
            audioManager.PlaySFX("Push Crate");
        }

        return canPush;
    }
}
