using System;
using UnityEngine;

public class Mirror : Switchable
{
    /// <summary>
    /// Indicates the direction of the mirror. If true, laser coming from top is reflected to right.
    /// </summary>
    public bool Forward;

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

	public override void Switch()
	{
		Forward = !Forward;
        entity.localScale = new Vector3(-entity.localScale.x, entity.localScale.y, entity.localScale.z); //transform.rotation = Quaternion.AngleAxis (Forward ? 0 : 90, Vector3.forward);
	}
}
