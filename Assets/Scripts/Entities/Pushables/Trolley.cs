using UnityEngine;

public class Trolley : Pushable
{
    private Vector2 previousPosition;
    private Vector2 movement;
    private Timer timer;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        Sfx = "Push Trolley";

        previousPosition = transform.position;

        timer = new Timer();
        timer.duration = 0.2f;
        timer.repeating = true;
        timer.Complete += CompleteMoving;
    }

    // Update is called once per frame
    void Update()
    {
        timer.Update();

        if (timer.running)
        {
            var newPosition = previousPosition + timer.progress * movement;
            transform.position = newPosition;
        }
    }

    public override bool Push(Vector3 direction)
    {
        movement = direction;

        var canPush = base.Push(direction);

        if (canPush && !timer.running)
        {
            timer.Reset();
        }

        return canPush;
    }

    protected override bool Push(RaycastHit2D hit, Vector3 direction)
    {
        switch (hit.collider.tag)
        {
            case "Pushable":
                var trolley = hit.collider.GetComponent<Trolley>();
                if (trolley != null)
                {
                    // OPTIONAL: Should it stay still or replace the original position? (needs an additional check on the next push value if used)
                    trolley.Push(direction);
                    return false;
                }
                return false;
            default:
                return false;
        }
    }

    private void CompleteMoving()
    {
        previousPosition += movement;
        transform.position = previousPosition;

        var canPush = CanPush(movement);

        if (!canPush)
        {
            timer.Stop();
        }
    }
}
