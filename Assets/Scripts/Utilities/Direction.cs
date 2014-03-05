using System;
using UnityEngine;

public enum Direction { Up, Down, Left, Right }

public static class DirectionExtensions
{
    public static readonly Vector2 Up = new Vector2(0, 1);
    public static readonly Vector2 Down = new Vector2(0, -1);
    public static readonly Vector2 Left = new Vector2(-1, 0);
    public static readonly Vector2 Right = new Vector2(1, 0);

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
        return ToVector2(d);
    }

    public static Vector2 ToVector2(this Direction d)
    {
        switch (d)
        {
            case Direction.Up:
                return Up;
            case Direction.Down:
                return Down;
            case Direction.Left:
                return Left;
            case Direction.Right:
                return Right;
            default:
                throw new Exception("Impossible");
        }
    }

    public static Direction ToDirection(this Vector2 d)
    {
        if (d == Up) return Direction.Up;
        if (d == Down) return Direction.Down;
        if (d == Left) return Direction.Left;
        if (d == Right) return Direction.Right;
        throw new Exception(string.Format("{0} is not a valid direction vector", d));
    }
}
