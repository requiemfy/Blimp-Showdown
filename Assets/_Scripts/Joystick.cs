using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] // for raycast usable area
public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
    public Action OnDragStarted;
    public Action<Vector2> WhileDraging;
    public Action OnDragStopped;

    [SerializeField] bool fixedOrigin;
    [SerializeField] RectTransform knob;
    [SerializeField] RectTransform origin;

    private Vector2 _firstTouchPos;






    //-------------------------------FUNCTIONS-------------------------------//
    public void OnBeginDrag(PointerEventData touch)
    {
        _firstTouchPos = touch.position;
        OnDragStarted();
    }

    private const int RADIUS = 100;
    public void OnDrag(PointerEventData touch)
    {
        Vector2 anchor = fixedOrigin ? (Vector2)origin.position : _firstTouchPos;
        Vector2 dragVec = touch.position - anchor;
        //knob.position = (Vector2)origin.position + _dragVec.normalized;
        if (dragVec.magnitude < RADIUS) knob.position = (Vector2)origin.position + dragVec;
        else knob.position = (Vector2)origin.position + RADIUS * dragVec.normalized;
        WhileDraging?.Invoke(dragVec);
    }

    public void OnEndDrag(PointerEventData touch)
    {
        knob.position = origin.position;
    }
}
