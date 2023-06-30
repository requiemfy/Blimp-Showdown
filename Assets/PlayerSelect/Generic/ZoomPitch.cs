using UnityEngine;

public class ZoomPitch : MonoBehaviour
{
    private Camera _cam;
    private void Start()
    {
        _cam = Camera.main;
    }
    private void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPos = touchZero.position;
            Vector2 touchOnePos = touchOne.position;
            float distance = (touchZeroPos - touchOnePos).magnitude;

            Vector2 touchZeroPrevPos = touchZeroPos - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOnePos - touchOne.deltaPosition;
            float preDistance = (touchZeroPrevPos - touchOnePrevPos).magnitude;

            float offset = distance - preDistance;
            Zoom(offset * 0.01f);
        }
    }

    private void Zoom(float amount)
    {
        _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize - amount, 5f, 15f);
    }
}
