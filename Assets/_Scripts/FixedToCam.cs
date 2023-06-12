using UnityEngine;

public class FixedToCam : MonoBehaviour
{
    private Transform cam;
    private void Awake()
    {
        cam = Camera.main.transform;
    }
    private void Update()
    {
        Vector3 currentPos = transform.position;
        transform.position = new(cam.position.x, currentPos.y, currentPos.z);
    }
}
