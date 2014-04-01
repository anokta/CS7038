using System.Collections.Generic;
using HandyGestures;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    public static KeyboardController Instance { get; private set; }
    public IPan KeyboardEventHandler;

    private Direction? previousMovement;

    private readonly UniqueList<Direction> pressedKeys = new UniqueList<Direction>();

    private static readonly Dictionary<Direction, KeyCode[]> Keys = new Dictionary<Direction, KeyCode[]>();

    static KeyboardController()
    {
        Keys[Direction.Up] = new[] { KeyCode.W, KeyCode.UpArrow };
        Keys[Direction.Down] = new[] { KeyCode.S, KeyCode.DownArrow };
        Keys[Direction.Left] = new[] { KeyCode.A, KeyCode.LeftArrow };
        Keys[Direction.Right] = new[] { KeyCode.D, KeyCode.RightArrow };
    }

    public KeyboardController()
    {
        Instance = this;
    }

    public void Update()
    {
        if (KeyboardEventHandler == null) return;

        var keyUp = false;

        foreach (var direction in DirectionExt.Values)
        {
            var keys = Keys[direction];

            if (InputExt.GetAnyKeyDown(keys))
            {
                pressedKeys.Add(direction);
            }
            else if (InputExt.GetAnyKeyUp(keys))
            {
                pressedKeys.Remove(direction);
                keyUp = true;
            }
        }

        if (pressedKeys.Count > 0)
        {
            var movement = pressedKeys.Last;
            PanArgs args;

            if (movement == previousMovement)
            {
                args = new PanArgs(HandyDetector.Gesture.Press, PanArgs.State.Hold, Vector2.zero, Vector2.zero, Vector2.zero);
            }
            else
            {
                var delta = movement.Reverse().ToVector2();
                args = new PanArgs(HandyDetector.Gesture.Press, PanArgs.State.Move, Vector2.zero, Vector2.zero, delta);
            }

            KeyboardEventHandler.OnGesturePan(args);

            previousMovement = pressedKeys.Last;
        }
        else if (keyUp)
        {
            var args = new PanArgs(HandyDetector.Gesture.Press, PanArgs.State.Up, Vector2.zero, Vector2.zero, Vector2.zero);
            KeyboardEventHandler.OnGesturePan(args);

            previousMovement = null;
        }
    }
}
