public static class FloatExt
{
    /// <summary>
    /// Difference between 1 and the least value greater than 1 that is representable.
    /// </summary>
    public static float Epsilon = 1e-5f;
    public static float NegativeEpsilon = -Epsilon;

    public static bool IsZero(this float f)
    {
        return f < Epsilon && f > NegativeEpsilon;
    }
}
