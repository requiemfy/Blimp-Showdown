using System.Collections;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] AudioGroup m_audioGrp;

    private IEnumerator Start()
    {
        var source = gameObject.AddComponent<AudioSource>();
        foreach(Sound sound in m_audioGrp.sounds)
        {
            source.clip = sound.clip;
            source.loop = sound.loop;
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.Play();
            yield return new WaitWhile(()=>source.isPlaying);
        }
    }
}
