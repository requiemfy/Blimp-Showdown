using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CanvasGroup))]
public class ReadyCard : MonoBehaviour, IPointerDownHandler
{
    public static int NotReadyCount = 0;

    [SerializeField] Team targetTeam;
    [SerializeField] TextMeshProUGUI stateTMP;
    [SerializeField] GameObject notReadyDisplay;
    [SerializeField] GameObject readyDisplay;
    [SerializeField] Button closeBtn;

    private CanvasGroup _parentCanvasGrp;
    private CanvasGroup _thisCanvasGrp;
    private Image _image;
    private TeamState _state = TeamState.Closed;
    enum TeamState 
    { 
        Closed,
        NotReady,
        Ready
    }

    private void Awake()
    {
        notReadyDisplay.transform.Find("ColorDynamic").GetComponent<Image>().color = targetTeam.GetTeamColor();
        readyDisplay.transform.Find("ColorDynamic").GetComponent<Image>().color = targetTeam.GetTeamColor();
        _parentCanvasGrp = transform.parent.GetComponent<CanvasGroup>();
        _thisCanvasGrp = GetComponent<CanvasGroup>();
        _thisCanvasGrp.alpha = 0;
        _image = GetComponent<Image>();
        _image.color = Color.white;
        closeBtn.onClick.AddListener(CloseSlot);
    }
    private void OnEnable()
    {
        FadeIn();
        if (DataPersistence.Get(targetTeam) == null) return; //first time
        if (DataPersistence.Get(targetTeam).Weapons.HasNullElement()) return;
        if (_state == TeamState.Ready) return;
        _state = TeamState.Ready;
        NotReadyCount--;
        stateTMP.text = "ready";
        TurnOnDisplay(TeamState.Ready);
    }
    private void OnDisable()
    {
        _thisCanvasGrp.alpha = 0;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        switch (_state)
        {
            case TeamState.Closed:
                _image.DOColor(targetTeam.GetTeamColor(), duration: 0.5f);
                _state = TeamState.NotReady;
                stateTMP.text = "not ready";
                closeBtn.gameObject.SetActive(true);
                NotReadyCount++;
                TurnOnDisplay(TeamState.NotReady);
                break;

            case TeamState.NotReady:
                EnterEdit();
                break;

            case TeamState.Ready:
                EnterEdit();
                break;

            default:
                break;
        }
    }

    private void TurnOnDisplay(TeamState state)
    {
        switch (state)
        {
            case TeamState.Closed:
                notReadyDisplay.SetActive(false);
                readyDisplay.SetActive(false);
                break;

            case TeamState.NotReady:
                notReadyDisplay.SetActive(true);
                readyDisplay.SetActive(false);
                break;

            case TeamState.Ready:
                notReadyDisplay.SetActive(false);
                readyDisplay.SetActive(true);
                break;

            default:
                break;
        }
    }
    private void EnterEdit()
    {
        SelectManager.Instance.StagedTeam = targetTeam;
        FadeOut();
    }
    private void CloseSlot()
    {
        if (_state == TeamState.NotReady) NotReadyCount--;
        _image.DOColor(Color.white, duration: 0.5f);
        _state = TeamState.Closed;
        stateTMP.text = "add player";
        closeBtn.gameObject.SetActive(false);
        DataPersistence.Push(targetTeam, data: null);
        TurnOnDisplay(TeamState.Closed);
    }
    private void FadeIn()
    {
        _thisCanvasGrp.DOKill();
        _parentCanvasGrp.DOFade(1, 0.2f)
            .onComplete = () => {
                SelectManager.Instance.weaponSelectScreen.SetActive(false);
                _thisCanvasGrp.DOFade(1, 0.5f)
                    .SetDelay((int)targetTeam * 0.2f);
            };
        
    }
    private void FadeOut()
    {
        _parentCanvasGrp.DOKill();
        SelectManager.Instance.weaponSelectScreen.SetActive(true);
        _parentCanvasGrp.DOFade(0, 0.2f)
            .onComplete = () =>
            {
                _parentCanvasGrp.gameObject.SetActive(false);
            };
    }
}
