﻿using System;
using UnityEngine;
using System.Collections;

public class Mirror : Pushable
{
    /// <summary>
    /// Indicates the direction of the mirror. If true, laser coming from top is reflected to right.
    /// </summary>
    public bool Forward { get; set; }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        //TODO: parameterize
        Forward = true;
    }

    public override bool Push(Vector3 direction)
    {
        // Check collisions
        RaycastHit2D hit = Physics2D.Raycast(entity.position + direction, direction, 0.0f);
        if (hit.collider != null)
        {
            // Collision detected
            switch (hit.collider.tag)
            {
                /* TODO: Specify restrictions per each entity type if needed later. */
                default:
                    return false;
            }
        }
        else
        {
            audioManager.PlaySFX("Push Crate");
			var controller = GameObject.FindObjectOfType<PlayerController>();
			controller.spoilHand();
            // Translate if not collided
            entity.position += direction;

            return true;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Direction Reflect(Direction incoming)
    {
        switch (incoming)
        {
            case Direction.Top:
                return Forward ? Direction.Left : Direction.Right;
            case Direction.Down:
                return Forward ? Direction.Right : Direction.Left;
            case Direction.Left:
                return Forward ? Direction.Top : Direction.Down;
            case Direction.Right:
                return Forward ? Direction.Down : Direction.Top;
            default:
                throw new Exception("Impossible");
        }
    }
}
