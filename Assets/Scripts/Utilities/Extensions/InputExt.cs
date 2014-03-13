using System.Collections.Generic;
using UnityEngine;

public class InputExt
{
    public static bool GetAnyKey(params KeyCode[] keys)
    {
        foreach (var keyCode in keys)
        {
            if (Input.GetKey(keyCode)) return true;
        }

        return false;
    }

    public static bool GetAnyKeyDown(params KeyCode[] keys)
    {
        foreach (var keyCode in keys)
        {
            if (Input.GetKeyDown(keyCode)) return true;
        }

        return false;
    }

    public static bool GetAnyKeyUp(params KeyCode[] keys)
    {
        foreach (var keyCode in keys)
        {
            if (Input.GetKeyUp(keyCode)) return true;
        }

        return false;
    }

    public static List<KeyCode> GetPressedKeys()
    {
        var arrows = new[] { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };

        var result = new List<KeyCode>();
        var inputString = Input.inputString;

        foreach (var keychar in inputString)
        {
            var key = keychar - 'a' + KeyCode.A;
            result.Add(key);
        }

        foreach (var keyCode in arrows)
        {
            if (Input.GetKey(keyCode)) result.Add(keyCode);
        }

        return result;
    }
}
