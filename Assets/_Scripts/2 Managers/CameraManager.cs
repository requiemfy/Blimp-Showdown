using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    private Camera _cam;
    private void Awake()
    {
        Instance= this;
        _cam = Camera.main;
    }
    private void Update()
    {
        Update_Follow();
        Update_PitchZoom();
    }

    #region FOLLOW
    public Vector2 Offset;

    [SerializeField] 
    private float speed;
    private Transform _follow;
    private Vector3 _targetPos;
    private bool _onlyX = false;
    private void Update_Follow()
    {
        Vector3 camPos = _cam.transform.position;
        if (_follow)
        {
            float X = _follow.position.x + Offset.x;
            float Y = _onlyX? camPos.y : _follow.position.y + Offset.y;
            _targetPos = new Vector3(X, Y, camPos.z);
            StopAllCoroutines();
        }
        else
        {
            if (!_triggered) StartCoroutine(BackToCurrentPlayerCO());
        }
        _cam.transform.position = Vector3.Lerp(camPos, _targetPos, Time.deltaTime * speed);
    }
    public void SetFollow(Transform transform, bool onlyX = false, float camSpeed = 1)
    {
        _follow = transform;
        this._onlyX = onlyX;
        speed = camSpeed;
        Offset = Vector2.zero;
        _triggered = false;
    }

    private Transform _currentPlayer;
    public void SetCurrentPlayer(Transform transform)
    {
        _currentPlayer = transform;
        SetFollow(_currentPlayer);
    }

    private bool _triggered = false;
    private IEnumerator BackToCurrentPlayerCO()
    {
        _triggered = true;
        yield return new WaitForSeconds(1.5f);
        SetFollow(_currentPlayer);
    }
    #endregion

    #region PITCHZOOM
    public Action<float> WhileZoomChanging;

    [SerializeField]
    private CinemachineVirtualCamera virtualCam;
    private void Update_PitchZoom()
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

            float pitchAmount = distance - preDistance;
            Zoom(pitchAmount * 0.01f);
            return;
        }
        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void Zoom(float amount)
    {
        virtualCam.m_Lens.OrthographicSize = Mathf.Clamp(virtualCam.m_Lens.OrthographicSize - amount, 5f, 20f);
        WhileZoomChanging(virtualCam.m_Lens.OrthographicSize);
    }
    #endregion
}

