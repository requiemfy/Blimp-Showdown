using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(Image))]
public class CamDragable : MonoBehaviour
{
    [SerializeField] private float multiplier;
    private CameraManager cam;
    private void Start()
    {
        cam = CameraManager.Instance;
    }


    private Vector2 firstTouchPos;
    private Vector2 firstOffset;
    public void DragBegin(BaseEventData baseEventData)
    {
        PointerEventData touch = baseEventData as PointerEventData;
        firstTouchPos = touch.position;
        firstOffset = cam.offset;
    }

    public void DragCamera(BaseEventData baseEventData)
    {
        PointerEventData touch = baseEventData as PointerEventData;
        Vector2 dragVec = touch.position - firstTouchPos;
        cam.offset = firstOffset - dragVec * multiplier;
    }

}
