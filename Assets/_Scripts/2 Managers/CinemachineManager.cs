using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager Instance { get; private set; }

    public CinemachineVirtualCamera VCam;
    public CinemachineCameraOffset Offset { get; private set; }

    private CinemachineBasicMultiChannelPerlin _basicMultiChannelPerlin;

    private void Awake()
    {
        Instance = this;
        Offset = VCam.GetComponent<CinemachineCameraOffset>();
        _basicMultiChannelPerlin = VCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void SetFollow(Transform target)
    {
        VCam.Follow = target;
        //Offset.m_Offset = Vector2.zero;
        DOTween.To(() => (Vector2)Offset.m_Offset, x => Offset.m_Offset = x, Vector2.zero, 1.5f);
    }

    /*
    public void PlayCamShake(int amplitute, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCountDown());
        IEnumerator ShakeCountDown()
        {
            _basicMultiChannelPerlin.m_AmplitudeGain = amplitute;
            yield return new WaitForSeconds(duration);
            _basicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
    }*/

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
}
