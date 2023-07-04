using System;
using UnityEngine;

public static class Extension
{
    public static Vector2 ChangeX(this Vector2 vec, float value)
    {
        return new Vector2(value, vec.y);
    }
    public static Vector2 ChangeY(this Vector2 vec, float value)
    {
        return new Vector2(vec.x, value);
    }
    public static Vector3 ChangeZ(this Vector3 vec, float value)
    {
        return new Vector3(vec.x, vec.y, value);
    }

    public static bool InRange(this float num, float min, float max)
    {
        return min < num && num < max;    
    }

    public static T ToEnum<T>(this string strg)
    {
        return (T)Enum.Parse(typeof(T), strg);
    }
}
