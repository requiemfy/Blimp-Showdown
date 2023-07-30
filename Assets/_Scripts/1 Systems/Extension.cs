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
    public static bool HasNullElement(this Array array)
    {
        foreach(var element in array)
        {
            if (element == null) return true;
        }
        return false;
    }
    public static Color32 GetTeamColor(this Team team)
    {
        switch (team)
        {
            case Team.Red:
                return CustomColors.Red;
            case Team.Blue:
                return CustomColors.Blue;
            case Team.Yellow:
                return CustomColors.Yellow;
            case Team.Green:
                return CustomColors.Green;
            default:
                break;
        }
        throw new Exception("Team has no color in palette");
    }
    public static Color32 ChangeAlpha(this Color32 color, byte alphaVal)
    {
        var targetColor = color; targetColor.a = alphaVal;
        return targetColor;
    }
    public static Color ChangeAlpha(this Color color, float alphaVal)
    {
        var targetColor = color; targetColor.a = alphaVal;
        return targetColor;
    }
}
public static class CustomColors
{
    public static Color32 Red { get; } = new Color32(242, 111, 103, 255);
    public static Color32 Blue { get; } = new Color32(75, 154, 255, 255);
    public static Color32 Yellow { get; } = new Color32(253, 191, 92, 255);
    public static Color32 Green { get; } = new Color32(54, 217, 140, 255);
    public static Color32 Energy { get; } = new Color32(255, 182, 0, 255);
    public static Color32 Black { get; } = new Color32(75 , 81, 103, 255);
    public static Color32 White { get; } = new Color32(235, 244, 255, 255);
}