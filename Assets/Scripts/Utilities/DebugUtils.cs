#define DEBUG

using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Utilities
{
    public class DebugUtils
    {
        [Conditional("DEBUG")]
        public static void Assert(bool condition)
        {
            if (!condition)
            {
                throw new Exception();
            }
        }
    }
}
