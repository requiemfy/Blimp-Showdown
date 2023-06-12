using UnityEngine;

public static class Extension
{
    public static Vector2 ToV2(this Vector3 v3)
    {
        return new(v3.x, v3.y);
    }

    public static Vector2 ChangeX(this Vector2 vec, float value)
    {
        return new Vector2(value, vec.y);
    }
    public static Vector2 ChangeY(this Vector2 vec, float value)
    {
        return new Vector2(vec.x, value);
    }
}
