using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] // for raycast usable area
public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public Action OnDragStarted;
    public Action<Vector2> WhileDraging;
    public Action UponPointerUp;

    [SerializeField] bool fixedOrigin;
    [SerializeField] RectTransform knob;
    [SerializeField] RectTransform origin;

    private Vector2 _firstTouchPos;
    private void Awake()
    {
        _radius = GetComponent<RectTransform>().sizeDelta.x / 2;
    }






    //-------------------------------FUNCTIONS-------------------------------//
    public void OnBeginDrag(PointerEventData touch)
    {
        _firstTouchPos = touch.position;
        OnDragStarted?.Invoke();
    }

    private float _radius;
    public void OnDrag(PointerEventData touch)
    {
        Vector2 anchor = fixedOrigin ? (Vector2)origin.position : _firstTouchPos;
        Vector2 dragVec = touch.position - anchor;
        if (dragVec.magnitude > _radius * Screen.width / 1920) knob.position = (Vector2)origin.position + _radius * Screen.width/1920 * dragVec.normalized; 
        else knob.position = (Vector2)origin.position + dragVec;
        WhileDraging?.Invoke(dragVec);
    }

    public void OnPointerUp(BaseEventData eventData)
    {
        knob.position = origin.position;
        UponPointerUp?.Invoke();
    }
}
