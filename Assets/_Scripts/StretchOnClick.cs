using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class StretchOnClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private void OnValidate()
    {
        var rectTransform = transform as RectTransform;
        if (rectTransform.pivot.x != 0.5f)
        {
            Debug.LogWarning(name + ": stretchOnClick works best with pivot X set to 0.5");
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlayAudioGroup("ButtonClick");
        DOTween.Kill(transform);
        transform.localScale = new(1.2f, 0.7f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(new Vector2(1f, 1f), duration: 0.1f);
    }
}
