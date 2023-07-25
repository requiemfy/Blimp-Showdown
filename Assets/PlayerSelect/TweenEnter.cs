using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TweenEnter : MonoBehaviour
{
    [SerializeField] Direction direction;
    [SerializeField] float duration;


    [System.Serializable]
    enum Direction {BottomUp, TopDown, LeftToRight, RightToLeft}
    private void OnEnable()
    {
        var rectTransform = transform as RectTransform;

        var originalPos = rectTransform.anchoredPosition;

        switch (direction)
        {
            case Direction.BottomUp:
                rectTransform.anchoredPosition = rectTransform.anchoredPosition.ChangeY(-(rectTransform.rect.height + 1080 * rectTransform.anchorMax.y));
                break;
            case Direction.TopDown:
                rectTransform.anchoredPosition = rectTransform.anchoredPosition.ChangeY(rectTransform.rect.height + 1080 * (1-rectTransform.anchorMin.y));
                break;
            case Direction.LeftToRight:
                rectTransform.anchoredPosition = rectTransform.anchoredPosition.ChangeX(-(rectTransform.rect.width + 1920 * rectTransform.anchorMin.x));
                break;
            case Direction.RightToLeft:
                rectTransform.anchoredPosition = rectTransform.anchoredPosition.ChangeX(rectTransform.rect.width + 1920 * (1-rectTransform.anchorMax.x));
                break;
            default:
                break;
        }

        rectTransform.DOAnchorPos(originalPos, duration).SetEase(Ease.OutCubic);
    }
}
