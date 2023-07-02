using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] Image fillImg;
    [SerializeField] TextMeshProUGUI TMP;

    public void SetFill(float val, float maxVal)
    {
        if (TMP) TMP.text = val.ToString();
        fillImg.DOFillAmount((float)val / maxVal, duration: 0.3f);
    }
}
