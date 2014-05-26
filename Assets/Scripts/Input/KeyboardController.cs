using System;
using System.Collections.Generic;
using HandyGestures;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    public static KeyboardController Instance { get; private set; }

    public IPan KeyboardEventHandler;
    private Direction? previousMovement;
    private readonly UniqueList<Direction> pressedKeys = new UniqueList<Direction>();
    private readonly Dictionary<Direction, KeyCode[]> directionKeys = new Dictionary<Direction, KeyCode[]>();

    private readonly List<KeySequenceDetector> keySequenceDetectors;

    public KeyboardController()
    {
        Instance = this;
        keySequenceDetectors = new List<KeySequenceDetector>();

        directionKeys[Direction.Up] = new[] { KeyCode.W, KeyCode.UpArrow };
        directionKeys[Direction.Down] = new[] { KeyCode.S, KeyCode.DownArrow };
        directionKeys[Direction.Left] = new[] { KeyCode.A, KeyCode.LeftArrow };
        directionKeys[Direction.Right] = new[] { KeyCode.D, KeyCode.RightArrow };
    }

    public void Update()
    {
        UpdateDirectionKeys();

        for (var i = keySequenceDetectors.Count - 1; i >= 0; i--)
        {
            var detector = keySequenceDetectors[i];
            detector.Update();
        }
    }

    private void UpdateDirectionKeys()
    {
        if (KeyboardEventHandler == null) return;

        var keyUp = false;

        foreach (var direction in DirectionExt.Values)
        {
            var keys = directionKeys[direction];

            if (InputExt.IsAnyKeyDown(keys))
            {
                pressedKeys.Add(direction);
            }
            else if (InputExt.IsAnyKeyUp(keys))
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
                args = new PanArgs(HandyDetector.Gesture.Press, PanArgs.State.Hold, Vector2.zero, Vector2.zero,
                    Vector2.zero);
            }
            else
            {
                var delta = movement.Reverse().ToVector2();
                args = new PanArgs(HandyDetector.Gesture.Press, PanArgs.State.Move, Vector2.zero, Vector2.zero,
                    delta * 10);
            }

            KeyboardEventHandler.OnGesturePan(args);

            previousMovement = pressedKeys.Last;
        }
        else if (keyUp)
        {
            var args = new PanArgs(HandyDetector.Gesture.Press, PanArgs.State.Up, Vector2.zero, Vector2.zero,
                Vector2.zero);
            KeyboardEventHandler.OnGesturePan(args);

            previousMovement = null;
        }
    }

    public void Add(KeySequenceDetector detector)
    {
        keySequenceDetectors.Add(detector);
    }

    public void Remove(KeySequenceDetector detector)
    {
        keySequenceDetectors.Remove(detector);
    }
}
