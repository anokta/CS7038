using System;
using UnityEngine;

public class Mirror : Switchable
{
    /// <summary>
    /// Indicates the direction of the mirror. If true, laser coming from top is reflected to right.
    /// </summary>
    public bool Forward;

	public Sprite mirrorForward;
	public Sprite mirrorSwitched;

    public Mirror()
    {
        //TODO: parameterize
        Forward = true;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
		renderer = GetComponent<SpriteRenderer>();
    }

	private SpriteRenderer renderer;

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

        if (Forward && renderer.sprite != mirrorForward)
        {
            renderer.sprite = mirrorForward;
        }
        else if (!Forward && renderer.sprite != mirrorSwitched)
        {
            renderer.sprite = mirrorSwitched;
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
