using UnityEngine;

public class Crate : Pushable
{
    private Vector2 previousPosition;
    private Vector2 movement;
    private Timer movementTimer;
    private Timer handTimer;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        MovingWithPlayer = true;

        previousPosition = transform.position;

        movementTimer = new Timer(0.4f, CompleteMoving);
        movementTimer.repeating = false;

        handTimer = new Timer(2.1f, HandTimerRanOut);
    }

    protected override void Update()
    {
        base.Update();

        movementTimer.Update();

        if (movementTimer.running)
        {
            var newPosition = previousPosition + movementTimer.progress * movement;
            transform.position = newPosition;
        }

        handTimer.Update();
    }

    public override bool Push(Vector3 direction, bool byPlayer = true)
    {
        bool canPush = base.Push(direction);

        if (canPush)
        {
            AudioManager.PlaySFX("Push Crate");

            if (byPlayer)
            {
                if (SpoilHand)
                {
                    if (playerHand.LastTouchedID != GetInstanceID())
                    {
						playerHand.SpoilHand(GetInstanceID());
                    }

                    handTimer.Reset();
                }
				//OnActivate();
				Execute(Trigger.ActionType.Handy);
            }
            else
            {
                if (!movementTimer.running)
                {
                    movement = direction;
                    movementTimer.Reset();
                }
				Execute(Trigger.ActionType.Other);
            }

        }

        return canPush;
    }

    private void CompleteMoving()
    {
        previousPosition += movement;
        transform.position = previousPosition;
    }

    void HandTimerRanOut()
    {
        if (playerHand.LastTouchedID == GetInstanceID())
        {
            playerHand.LastTouchedID = 0;
        }
    }
}
