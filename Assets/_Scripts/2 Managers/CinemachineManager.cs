using Cinemachine;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager Instance { get; private set; }

    [SerializeField]
    private CinemachineVirtualCamera cinemachine;

    private void Awake()
    {
        Instance = this;
    }

    public void SetFollow(Transform target)
    {
        cinemachine.Follow = target;
    }
}
