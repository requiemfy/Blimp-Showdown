using TMPro;
using UnityEngine;

public class PowerAnglePad : MonoBehaviour
{
    [SerializeField] TextMeshPro powerTMP;
    [SerializeField] TextMeshPro angleTMP;

    public void UpdateValues(float power, int angle)
    {
        powerTMP.text = $"{(int)(power * 100)}%";
        angleTMP.text = $"{angle}°";
    }
}
