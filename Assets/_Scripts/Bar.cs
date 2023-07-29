using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] Image fillImg;
    [SerializeField] Image delayImg;
    [SerializeField] TextMeshProUGUI TMP;

    public void SetFill(float val, float maxVal)
    {
        if (TMP) TMP.text = val.ToString();
        float targetVal = (float)val / maxVal;
        fillImg.fillAmount = targetVal;
        if (delayImg)
        {
            delayImg.DOKill();
            delayImg.DOFillAmount(targetVal, duration: 0.5f)
                .SetEase(Ease.InOutCubic)
                .SetDelay(1);
        }
    }
}
