using UnityEngine;

public class TriggerMusicOnStart : MonoBehaviour
{
    [SerializeField] string[] m_musicGroups;
    void Start()
    {
        AudioManager.Instance.StopAllButPlay(m_musicGroups);
    }
}
