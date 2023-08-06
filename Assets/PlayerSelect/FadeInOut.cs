using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeInOut : MonoBehaviour
{
    [SerializeField]
    private float duration; 
    private CanvasGroup m_CanvasGroup;
    private void Awake()
    {
        m_CanvasGroup= GetComponent<CanvasGroup>();
    }
    public void FadeIn()
    {
        if (DOTween.IsTweening(m_CanvasGroup)) return;
        gameObject.SetActive(true);
        m_CanvasGroup.DOFade(1, duration);
    }
    public void FadeOut() 
    {
        if (DOTween.IsTweening(m_CanvasGroup)) return;
        m_CanvasGroup.DOFade(0, duration)
            .onComplete = () => gameObject.SetActive(false);
    }
}
