using UnityEngine;

public class ScaleWithZoom : MonoBehaviour
{
    private void Start()
    {
        CinemachineManager.Instance.WhileZoomChanging += Scale;
    }
    private void OnDestroy()
    {
        CinemachineManager.Instance.WhileZoomChanging -= Scale;
    }

    private void Scale(float orthographicSize)
    {
        float size = (float)orthographicSize / 5;
        transform.localScale = new Vector2(size, size);
    }
}
