using TMPro;
using UnityEngine;

public class PowerAnglePad : MonoBehaviour
{
    [SerializeField] TextMeshPro powerTMP;
    [SerializeField] TextMeshPro angleTMP;

    public void UpdateValues(float power, int angle)
    {
        powerTMP.text = (power * 100).ToString("0");
        angleTMP.text = angle.ToString();
    }
}
