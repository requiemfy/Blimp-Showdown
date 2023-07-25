using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringObject : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField] int delta;
    private void Awake()
    {
        rectTransform = transform as RectTransform;
        rectTransform.DOAnchorPos(rectTransform.anchoredPosition + new Vector2(0,delta), duration: 1)
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
