using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CamDragable : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private float multiplier;
    private CameraManager cam;
    private void Start()
    {
        cam = CameraManager.Instance;
    }
    private Vector2 firstTouchPos;
    private Vector2 firstOffset;
    public void OnBeginDrag(PointerEventData touch)
    {
        firstTouchPos = touch.position;
        firstOffset = cam.Offset;
    }
    public void OnDrag(PointerEventData touch)
    {
        if (Input.touchCount != 1) return;
        Vector2 dragVec = touch.position - firstTouchPos;
        cam.Offset = firstOffset - dragVec * multiplier;
    }
}
