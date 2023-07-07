using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ReadyCard : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Team targetTeam;

    private GameObject _parent;
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
        _parent = transform.parent.gameObject;
        _image = GetComponent<Image>();
        var faithColor = targetTeam.GetTeamColor();
        faithColor.a = 100;
        _image.color = faithColor;
    }
    private void OnEnable()
    {
        if (DataPersistence.Get(targetTeam) == null) return; //first time
        if (DataPersistence.Get(targetTeam).Weapons.HasNullElement()) return;
        _state = TeamState.Ready;
        Debug.Log("ready");
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        switch (_state)
        {
            case TeamState.Closed:
                _image.color = targetTeam.GetTeamColor();
                _state = TeamState.NotReady;
                break;

            case TeamState.NotReady:
                SelectManager.Instance.StagedTeam = targetTeam;
                _parent.SetActive(false);
                break;

            default:
                break;
        }
    }
}
