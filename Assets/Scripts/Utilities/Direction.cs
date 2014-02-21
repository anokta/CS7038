using System;
using UnityEngine;

public enum Direction { Up, Down, Left, Right }

public static class DirectionExtensions
{
    public static Direction Reverse(this Direction d)
    {
        switch (d)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            default:
                throw new Exception("Impossible");
        }
    }

    public static Vector3 ToVector3(this Direction d)
    {
        switch (d)
        {
            case Direction.Up:
                return new Vector3(0, 1, 0);
            case Direction.Down:
                return new Vector3(0, -1, 0);
            case Direction.Left:
                return new Vector3(-1, 0, 0);
            case Direction.Right:
                return new Vector3(1, 0, 0);
            default:
                throw new Exception("Impossible");
        }
    }

    public static Vector2 ToVector2(this Direction d)
    {
        switch (d)
        {
            case Direction.Up:
                return new Vector2(0, 1);
            case Direction.Down:
                return new Vector2(0, -1);
            case Direction.Left:
                return new Vector2(-1, 0);
            case Direction.Right:
                return new Vector2(1, 0);
            default:
                throw new Exception("Impossible");
        }
    }
}
