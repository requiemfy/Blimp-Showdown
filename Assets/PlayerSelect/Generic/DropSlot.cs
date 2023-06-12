using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(GridLayoutGroup))]
public abstract class DropSlot : MonoBehaviour, IDropHandler
{
    protected abstract void OnItemDropped(DragableItem droppedItem);
    public void OnDrop(PointerEventData eventData)
    {
        ClearDropSlot();
        DragableItem droppedItem = eventData.pointerDrag.GetComponent<DragableItem>();
        droppedItem.parentAfterDrag = transform;
        OnItemDropped(droppedItem);
    }

    protected void ClearDropSlot()
    {
        if (transform.childCount != 0) Destroy(transform.GetChild(0).gameObject);
    }
}
