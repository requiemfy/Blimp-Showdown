using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] // for raycast usable area
public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
    [SerializeField] bool fixedOrigin;
    [SerializeField] RectTransform knob;
    [SerializeField] RectTransform origin;

    public Action OnDragStarted;
    public Action<Vector2> WhileDraging;
    public Action OnDragStopped;

    private Vector2 firstTouchPos;






    //-------------------------------FUNCTIONS-------------------------------//
    public void OnBeginDrag(PointerEventData touch)
    {
        firstTouchPos = touch.position;
        OnDragStarted();
    }

    private const int RADIUS = 200;
    public void OnDrag(PointerEventData touch)
    {
        Vector2 _anchor = fixedOrigin ? (Vector2)origin.position : firstTouchPos;
        Vector2 _dragVec = touch.position - _anchor;
        //knob.position = (Vector2)origin.position + _dragVec.normalized;
        if (_dragVec.magnitude > RADIUS) knob.position = (Vector2)origin.position + RADIUS * _dragVec.normalized;
        else knob.position = (Vector2)origin.position + _dragVec;
        WhileDraging?.Invoke(_dragVec);
    }

    public void OnEndDrag(PointerEventData touch)
    {
        knob.position = origin.position;
    }
}
