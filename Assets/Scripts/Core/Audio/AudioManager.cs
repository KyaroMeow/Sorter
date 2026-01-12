using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private Sound[] sounds;
    
    void Awake()
    {
        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play(string soundName)
    {
        var sound = Array.Find(sounds, sound => sound.name == soundName);
        sound.source.Play();
    }

    public void Stop(string soundName)
    {
        var sound = Array.Find(sounds, sound => sound.name == soundName);
        sound.source.Stop();
    }
}
