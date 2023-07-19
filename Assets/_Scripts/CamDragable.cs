using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CamDragable : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private float multiplier;
    private CinemachineCameraOffset _transposer;
    private void Start()
    {
        _transposer = CinemachineManager.Instance.Offset;
    }
    private Vector2 firstTouchPos;
    private Vector2 firstOffset;
    public void OnBeginDrag(PointerEventData touch)
    {
        firstTouchPos = touch.position;
        firstOffset = _transposer.m_Offset;
    }
    public void OnDrag(PointerEventData touch)
    {
        if (Input.touchCount != 1 && !Input.GetMouseButton(0)) return;
        Vector2 dragVec = touch.position - firstTouchPos;
        _transposer.m_Offset = firstOffset - dragVec * multiplier;
    }
}
