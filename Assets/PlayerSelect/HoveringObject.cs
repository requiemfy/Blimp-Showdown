using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HoveringObject : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField] float delay = 0;
    [SerializeField] int delta;
    private void Start()
    {
        rectTransform = transform as RectTransform;
        rectTransform.DOAnchorPos(rectTransform.anchoredPosition + new Vector2(0, delta), duration: 1)
            .SetDelay(delay)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnEnable()
    {
        DOTween.Play(rectTransform);
    }
    private void OnDisable()
    {
        DOTween.Pause(rectTransform);
    }
}
