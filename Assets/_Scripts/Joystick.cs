using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(Image))] // for raycast usable area
public class Joystick : MonoBehaviour
{
    public Vector2 dragVec;

    [SerializeField] RectTransform knob;
    [SerializeField] RectTransform origin;

    public Action<Vector2> WhileDraging;






    //-------------------------------FUNCTIONS-------------------------------//

    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData touch = baseEventData as PointerEventData;
        Vector2 touchToOrigin = touch.position - (Vector2)origin.position;
        dragVec = touchToOrigin;
        //knob.position = (Vector2)origin.position + DragVec.normalized;
        WhileDraging?.Invoke(dragVec);
    }
}
