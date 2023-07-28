using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Random = UnityEngine.Random;

public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager Instance { get; private set; }

    public CinemachineVirtualCamera VCam;
    public CinemachineCameraOffset Offset { get; private set; }

    private Transform _currentPlayer;

    private void Awake()
    {
        Instance = this;
        Offset = VCam.GetComponent<CinemachineCameraOffset>();
    }
    private void Update()
    {
        Update_PitchZoom();
    }

    public void SetFollow(Transform target, bool isPlayer = false)
    {
        if (target == null) target = _currentPlayer;
        if (isPlayer) _currentPlayer = target;
        VCam.Follow = target;
        //Offset.m_Offset = Vector2.zero;
        DOTween.To(() => (Vector2)Offset.m_Offset, x => Offset.m_Offset = x, Vector2.zero, 1.5f);
    }

    public void PlayCamShake(float intensity, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCountDown());
        IEnumerator ShakeCountDown()
        {
            Vector2 original = Offset.m_Offset;

            float time = 0;
            float timeSinceLastUpdate = 0;
            while (time < duration)
            {
                yield return null;
                if (timeSinceLastUpdate > 0.02f)
                {
                    Offset.m_Offset = (Vector2)Offset.m_Offset + new Vector2(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));
                    timeSinceLastUpdate = 0;
                }
                time += Time.deltaTime;
                timeSinceLastUpdate += Time.deltaTime;
            }

            Offset.m_Offset = original;
        }
    }

    #region PITCHZOOM
    public Action<float> WhileZoomChanging;
    public float OrthographicSize
    {
        get
        {
            return VCam.m_Lens.OrthographicSize;
        }
        set
        {
            VCam.m_Lens.OrthographicSize = value;
            WhileZoomChanging?.Invoke(value);
        }
    }

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
        OrthographicSize = Mathf.Clamp(VCam.m_Lens.OrthographicSize - amount, min: 5f, max: 20f);
    }
    #endregion
}
