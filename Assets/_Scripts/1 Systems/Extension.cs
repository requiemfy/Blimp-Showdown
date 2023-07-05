using System;
using Unity.VisualScripting;
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
public static class CustomColors
{
    public static Color32 Red { get; } = new Color32(255, 0, 0, 255);
    public static Color32 Blue { get; } = new Color32(75, 154, 255, 255);
    public static Color32 Yellow { get; } = new Color32(255, 226, 75, 255);
    public static Color32 Green { get; } = new Color32(41, 233, 105, 255);
    public static Color32 Purple { get; } = new Color32(128, 0, 255, 255);
}
