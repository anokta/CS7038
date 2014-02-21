using System;
using UnityEngine;
using System.Collections;

public class Mirror : Pushable
{
    /// <summary>
    /// Indicates the direction of the mirror. If true, laser coming from top is reflected to right.
    /// </summary>
    public bool Forward { get; set; }

    public Mirror()
    {
        MovingWithPlayer = true;
        Sfx = "Push Crate";
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        //TODO: parameterize
        Forward = true;
    }

    // Update is called once per frame
    void Update()
    {
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
}
