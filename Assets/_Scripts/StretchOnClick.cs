using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class StretchOnClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Tween currentTween;
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlayAudioGroup("ButtonClick");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        currentTween?.Kill(true);
        currentTween = transform.DOScale(new Vector2(1.2f, 0.7f), duration: 0.1f)
            .SetLoops(2, LoopType.Yoyo);
    }
}
