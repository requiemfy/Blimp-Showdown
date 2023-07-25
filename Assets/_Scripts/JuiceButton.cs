using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class JuiceButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onClick;
    [SerializeField] Color onClickCol;

    private Color originalCol;
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        originalCol = image.color;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlayAudioGroup("ButtonClick");
        DOTween.Kill(transform);
        transform.localScale = new(1.2f, 0.7f);
        image.color = onClickCol;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(new Vector2(1f, 1f), duration: 0.1f)
            .onComplete = () => onClick?.Invoke();
        image.color = originalCol;
    }
}
