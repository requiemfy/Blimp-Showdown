using System;
using System.Linq;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private AudioGroup[] audioGroups;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        foreach (AudioGroup audioGroup in audioGroups)
        {
            audioGroup.source = gameObject.AddComponent<AudioSource>();
            audioGroup.source.loop = audioGroup.loop;
        }
    }

    public void PlayAudioGroup(string name, bool restartIfPlaying = true)
    {
        AudioGroup group = Array.Find(audioGroups, group => group.name == name);
        if (group == null)
        {
            Debug.LogWarning($"group {name} not found");
            return;
        }
        if (restartIfPlaying && group.source.isPlaying) return;

        Sound targetSound = group.sounds[UnityEngine.Random.Range(0, group.sounds.Length)];
        group.source.clip = targetSound.clip;
        group.source.volume = targetSound.volume;
        group.source.Play();
    }

    public void StopAllButPlay(string[] names)
    {
        AudioGroup[] stopGroups = audioGroups;
        foreach (string name in names)
        {
            stopGroups = stopGroups.Where(group => group.name != name).ToArray();
            PlayAudioGroup(name, restartIfPlaying: false);
        }
        foreach (AudioGroup group in stopGroups)
        {
            group.source.Stop();
        }
    }
}

[Serializable]
class AudioGroup
{
    public string name;
    public bool loop;
    [HideInInspector] public AudioSource source;

    public Sound[] sounds;
}

[Serializable]
class Sound
{
    public AudioClip clip;
    [Range(0f, 1f)] public float volume;
}


