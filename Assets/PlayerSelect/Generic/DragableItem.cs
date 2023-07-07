using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public abstract class DragableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] 
    public Transform parentAfterDrag;
    protected abstract void OnBeginScroll();
    protected abstract void WhileScrolling(Vector2 scrollVec);
    protected abstract void OnTap();

    private Transform originalParent;
    private Image image;
    private Vector2 potentialDrag;
    private bool isScrolling;
    private int _siblingIndex;

    protected virtual void Awake()
    {
        image = GetComponent<Image>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isTap = true;
        potentialDrag = eventData.position;
        _siblingIndex = transform.GetSiblingIndex();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        isTap = false;
        var delta = (eventData.position - potentialDrag);
        isScrolling = Mathf.Abs(delta.x) < Mathf.Abs(delta.y);
        if (isScrolling) {
            OnBeginScroll();
            return;
        }
        Duplicate();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isScrolling)
        {
            var delta = (eventData.position - potentialDrag);
            WhileScrolling(delta);
            return;
        }
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isScrolling) return;
        if (parentAfterDrag == originalParent)
        {
            Destroy(gameObject);
            return;
        }
        transform.SetParent(parentAfterDrag);
    }


    //tap
    private bool isTap = false;
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isTap) return;
        OnTap();
    }


    //private
    private void Duplicate()
    {
        originalParent = transform.parent;
        var duplicate = Instantiate(gameObject, originalParent);
        duplicate.transform.SetSiblingIndex(_siblingIndex);
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }
}
