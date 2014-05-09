using System;
using UnityEngine;

public class KeyCodeExt
{
    /// <summary>
    /// Converts the string representation of the name of keys to an equivalent enum array.
    /// </summary>
    /// <param name="keyStr">
    /// The string representation of the name of keys.
    /// Examples: "W A S D Up Down Left Right"
    /// </param>
    /// <returns></returns>
    public static KeyCode[] Parse(string keyStr)
    {
        var keyArray = keyStr.Split(' ');
        var keyCodes = new KeyCode[keyArray.Length];

        for (var i = 0; i < keyArray.Length; i++)
        {
            var keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), keyArray[i]);
            keyCodes[i] = keyCode;
        }

        return keyCodes;
    }
}
