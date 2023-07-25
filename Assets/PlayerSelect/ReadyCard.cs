using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
                _image.color = targetTeam.GetTeamColor();
                _state = TeamState.NotReady;
                stateTMP.text = "not ready";
                closeBtn.gameObject.SetActive(true);
                NotReadyCount++;
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


    private void EnterEdit()
    {
        SelectManager.Instance.StagedTeam = targetTeam;
        FadeOut();
    }
    private void CloseSlot()
    {
        if (_state == TeamState.NotReady) NotReadyCount--;
        _image.color = Color.white;
        _state = TeamState.Closed;
        stateTMP.text = "add player";
        closeBtn.gameObject.SetActive(false);
        DataPersistence.Push(targetTeam, data: null);
    }
    private void FadeIn()
    {
        _thisCanvasGrp.DOKill();
        _parentCanvasGrp.DOFade(1, 0.2f)
            .onComplete = () => {
                _thisCanvasGrp.DOFade(1, 0.5f)
                    .SetDelay((int)targetTeam * 0.2f);
            };
        
    }
    private void FadeOut()
    {
        _parentCanvasGrp.DOKill();
        _parentCanvasGrp.DOFade(0, 0.2f)
            .onComplete = () =>
            {
                _parentCanvasGrp.gameObject.SetActive(false);
            };
    }
}
