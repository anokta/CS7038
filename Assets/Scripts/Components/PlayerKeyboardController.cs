using System.Collections.Generic;
using HandyGestures;
using UnityEngine;

public class PlayerKeyboardController
{
    public PlayerController Player;

    private bool wasPressingKey;

    private readonly UniqueList<Direction> PressedKeys = new UniqueList<Direction>();

    private static readonly Dictionary<Direction, KeyCode[]> Keys = new Dictionary<Direction, KeyCode[]>();

    static PlayerKeyboardController()
    {
        Keys[Direction.Up] = new[] { KeyCode.W, KeyCode.UpArrow };
        Keys[Direction.Down] = new[] { KeyCode.S, KeyCode.DownArrow };
        Keys[Direction.Left] = new[] { KeyCode.A, KeyCode.LeftArrow };
        Keys[Direction.Right] = new[] { KeyCode.D, KeyCode.RightArrow };
    }

    public PlayerKeyboardController(PlayerController player)
    {
        Player = player;
    }

    public void Update()
    {
        if (Player == null) return;

        var removed = false;

        foreach (var direction in DirectionExt.Values)
        {
            var keys = Keys[direction];

            if (InputExt.GetAnyKeyDown(keys))
            {
                PressedKeys.Add(direction);
            }
            else if (InputExt.GetAnyKeyUp(keys))
            {
                PressedKeys.Remove(direction);
                removed = true;
            }
        }

        if (PressedKeys.Count > 0)
        {
            var movement = PressedKeys.Last;
            if (!Player.playerMoving || movement.ToVector2() != Player.movement)
            {
                var delta = movement.Reverse().ToVector2();
                var args = new PanArgs(HandyDetector.Gesture.Press, PanArgs.State.Move, Vector2.zero, Vector2.zero,
                    delta);
                Player.PlayerMoving(args);
            }
        }
        else if (removed)
        {
            var args = new PanArgs(HandyDetector.Gesture.Press, PanArgs.State.Up, Vector2.zero, Vector2.zero,
            Vector2.zero);
            Player.PlayerMoving(args);
        }
    }
}
