using Cinemachine;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager Instance { get; private set; }

    public CinemachineVirtualCamera VCam;
    public CinemachineFramingTransposer Transposer { get; private set; }

    private void Awake()
    {
        Instance = this;
        Transposer = VCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void SetFollow(Transform target)
    {
        VCam.Follow = target;
        Transposer.m_TrackedObjectOffset = Vector2.zero;
    }
}
