using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class GameOverScreen : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        gameObject.SetActive(false);
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, endValue: 1, duration: 1);
        foreach (Team team in DataPersistence.GetOpenedTeams())
        {
            if (DataPersistence.Get(team).isDestroyed) continue;
            Debug.LogWarning("Winner: " + team);
        }
    }
}
