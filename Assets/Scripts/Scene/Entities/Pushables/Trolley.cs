﻿using UnityEngine;
using Grouping;

public class Trolley : Pushable
{
    private Vector2 previousPosition;
    private Vector2 movement;
    private Timer timer;
    private bool moving { get { return timer.running; } }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        previousPosition = transform.position;

        timer = new Timer(0.2f, CompleteMoving);
        timer.repeating = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        timer.Update();

        if (timer.running)
        {
            AudioManager.PlaySFX("Loop Trolley");

            var newPosition = previousPosition + timer.progress * movement;
            transform.position = newPosition;
        }
    }

    public override bool Push(Vector3 direction, bool byPlayer)
    {
        movement = direction;

        var canPush = base.Push(direction);

        if (canPush)
        {
            if (!timer.running)
            {
                timer.Reset();

                AudioManager.PlaySFX("Push Trolley");

                if (byPlayer)
                {
                    playerHand.SpoilHand(GetInstanceID());
                }
            }
        }

        return canPush;
    }

    protected override bool CanPush(RaycastHit2D hit, Vector3 direction)
    {
        if (moving)
        {
            switch (hit.collider.tag)
            {
                case "Pushable":
                    var trolley = hit.collider.GetComponent<Trolley>();
                    if (trolley != null)
                    {
                        trolley.Push(direction, false);
                        break;
                    }
                    break;
                case "Switchable":
                    Switchable switchable = hit.collider.GetComponent<Switchable>();
                    switchable.Switch(false);

                    /*if (!GameWorld.dialogueOff && LevelManager.instance.Level == 5 && switchable.name.StartsWith("Mirror") && DialogueManager.CurrentDialogue == (DialogueManager.dialogueIndex[LevelManager.instance.Level] + 1))
                    {
                        DialogueManager.DialogueComplete = GameWorld.GoBackToLevel;
                        GroupManager.main.activeGroup = GroupManager.main.group["Dialogue"];

                        return false;
                    }*/
                    break;
            }
        }

        return false;
    }

    private void CompleteMoving()
    {
        previousPosition += movement;
        transform.position = previousPosition;

        var canPush = CanPush(movement);

        if (!canPush)
        {
            timer.Stop();
            AudioManager.StopSFX("Loop Trolley");
            AudioManager.PlaySFX("Push Trolley");
        }
    }
}
