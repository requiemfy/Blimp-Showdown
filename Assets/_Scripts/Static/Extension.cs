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

    public static bool InRange(this float num, float min, float max)
    {
        return min < num && num < max;    
    }
}
