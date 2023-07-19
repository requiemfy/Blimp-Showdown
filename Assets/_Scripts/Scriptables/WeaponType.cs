using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Weapon")]
public class WeaponType : ScriptableObject
{
    public Vector2 firePointPos;
    public Sprite barrel;
    public Sprite frame;
    public Sprite bulletSprite;
    public int energyCost;
    public int health;
    public int damage;
    public int range;

    //projectile
    public float explodeRadius;
    public bool isGravityAffected;
    public int bulletCount;
    public float bulletOffset;
    public float bulletShootTime;

    //isSpearable
    //max and min angles

    //raycast
    [Header("Type")]
    public bool isRaycast;
    public bool isRayMultiple;

    private void OnValidate()
    {
        bulletCount = Mathf.Max(bulletCount, 1);
        bulletOffset = Mathf.Min((float)1 / bulletCount, bulletOffset);
        bulletOffset = Mathf.Max(bulletOffset, 0);
        bulletShootTime = Mathf.Max(bulletShootTime, 0.3f); //recoil time
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(WeaponType))]
public class WeaponEditor : Editor
{
    private SerializedProperty firePointPos;
    private SerializedProperty barrel;
    private SerializedProperty frame;
    private SerializedProperty bulletSprite;
    private SerializedProperty energyCost;
    private SerializedProperty health;
    private SerializedProperty damage;
    private SerializedProperty range;

    private SerializedProperty detectRadius;
    private SerializedProperty explodeRadius;
    private SerializedProperty isGravityAffected;
    private SerializedProperty bulletCount;
    private SerializedProperty bulletOffset;
    private SerializedProperty bulletShootTime;

    private SerializedProperty isRaycast;
    private SerializedProperty isRayMultiple;

    private void OnEnable()
    {
        firePointPos = serializedObject.FindProperty("firePointPos");
        barrel = serializedObject.FindProperty("barrel");
        frame = serializedObject.FindProperty("frame");
        bulletSprite = serializedObject.FindProperty("bulletSprite");
        energyCost = serializedObject.FindProperty("energyCost");
        health = serializedObject.FindProperty("health");
        damage = serializedObject.FindProperty("damage");
        range = serializedObject.FindProperty("range");

        explodeRadius = serializedObject.FindProperty("explodeRadius");
        isGravityAffected = serializedObject.FindProperty("isGravityAffected");
        bulletCount = serializedObject.FindProperty("bulletCount");
        bulletOffset = serializedObject.FindProperty("bulletOffset");
        bulletShootTime = serializedObject.FindProperty("bulletShootTime");

        isRaycast = serializedObject.FindProperty("isRaycast");
        isRayMultiple = serializedObject.FindProperty("isRayMultiple");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(firePointPos);
        EditorGUILayout.PropertyField(barrel);
        EditorGUILayout.PropertyField(frame);
        EditorGUILayout.PropertyField(bulletSprite);
        EditorGUILayout.PropertyField(energyCost);
        EditorGUILayout.PropertyField(health);
        EditorGUILayout.PropertyField(damage);
        EditorGUILayout.PropertyField(range);
        EditorGUILayout.PropertyField(isRaycast);

        if (isRaycast.boolValue)
        {
            EditorGUILayout.PropertyField(isRayMultiple);
        }
        else
        {
            EditorGUILayout.PropertyField(explodeRadius);
            EditorGUILayout.PropertyField(isGravityAffected);
            EditorGUILayout.PropertyField(bulletCount);
            if (bulletCount.intValue > 1)
            {
                EditorGUILayout.PropertyField(bulletOffset);
                EditorGUILayout.PropertyField(bulletShootTime);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif