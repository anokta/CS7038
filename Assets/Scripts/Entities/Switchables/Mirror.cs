using System;
using UnityEngine;

public class Mirror : Switchable
{
    /// <summary>
    /// Indicates the direction of the mirror. If true, laser coming from top is reflected to right.
    /// </summary>
    public bool Forward;

	public Sprite MirrorForward;
	public Sprite MirrorInverse;

    public Mirror()
    {
        //TODO: parameterize
        Forward = true;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    public Direction Reflect(Direction incoming)
    {
        switch (incoming)
        {
            case Direction.Up:
                return Forward ? Direction.Left : Direction.Right;
            case Direction.Down:
                return Forward ? Direction.Right : Direction.Left;
            case Direction.Left:
                return Forward ? Direction.Up : Direction.Down;
            case Direction.Right:
                return Forward ? Direction.Down : Direction.Up;
            default:
                throw new Exception("Impossible");
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Forward && spriteRenderer.sprite != MirrorForward)
        {
            spriteRenderer.sprite = MirrorForward;
        }
        else if (!Forward && spriteRenderer.sprite != MirrorInverse)
        {
            spriteRenderer.sprite = MirrorInverse;
        }
    }

	public override void Switch()
	{
        audioManager.PlaySFX("Mirror");

		Forward = !Forward;

        var controller = FindObjectOfType<PlayerController>();
        controller.spoilHand(0.5f);
	}
}
