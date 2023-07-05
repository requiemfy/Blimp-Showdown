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

    //Palette
    public static Color32 GetColorPalette (this Team team)
    {
        switch (team)
        {
            case Team.Red:
                return new Color32(255, 0, 0,255);
            case Team.Blue:
                return new Color32(75, 154,255,255);
            case Team.Yellow:
                return new Color32(255,226,75,255);
            case Team.Green:
                return new Color32(41, 233, 105,255);
            default:
                break;
        }
        throw new Exception("Team has no color in palette");
    }
}
