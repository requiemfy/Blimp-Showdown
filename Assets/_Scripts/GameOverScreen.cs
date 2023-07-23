using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Image crownIcon;
    [SerializeField] private TextMeshProUGUI winnerTMP;

    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        gameObject.SetActive(false);
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
    }

    public void Show()
    {
        foreach (Team team in DataPersistence.GetOpenedTeams())
        {
            if (DataPersistence.Get(team).isDestroyed) continue;
            crownIcon.color = team.GetTeamColor();
            winnerTMP.color = team.GetTeamColor();
            winnerTMP.text = team.ToString();
        }
        gameObject.SetActive(true);
        DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, endValue: 1, duration: 1);
    }
}
