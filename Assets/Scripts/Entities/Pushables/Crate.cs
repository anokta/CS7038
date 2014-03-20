using UnityEngine;

public class Crate : Pushable
{
    Timer handTimer;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        MovingWithPlayer = true;

        handTimer = new Timer(2.8f, HandTimerRanOut);
    }

    protected override void Update()
    {
        base.Update();

        handTimer.Update();
    }

    public override bool Push(Vector3 direction)
    {
        bool canPush = base.Push(direction);

        if (canPush)
        {
            audioManager.PlaySFX("Push Crate");

            if (SpoilHand)
            {
                if (playerHand.LastTouchedID != GetInstanceID())
                {
                    playerHand.SpoilHand(-0.75f, GetInstanceID());

                    handTimer.Reset();
                }
            }
        }

        return canPush;
    }

    public void Explode()
    {
    }

    void HandTimerRanOut()
    {
        if (playerHand.LastTouchedID == GetInstanceID())
        {
            playerHand.LastTouchedID = 0;
        }
    }
}
