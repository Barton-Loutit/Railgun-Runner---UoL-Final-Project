using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManagerInstance { get; private set; }
    public Sound[] sounds;
    //public AudioSources[]

    private void Awake()
    {
        if (audioManagerInstance == null)
        {
            audioManagerInstance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Play("BackgroundMusic");
    }

    
    public void Play(string audioName)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == audioName);
        if (s == null)
        {
            Debug.LogWarning("Sound " + audioName + "not found");
            return;
        }
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public void Stop(string audioName)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == audioName);
        if (s == null)
        {
            Debug.LogWarning("Sound " + audioName + "not found");
            return;
        }
        s.source.Stop();
    }

    public void setLoopState(string audioName, bool desiredLoopState)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == audioName);
        if (s == null)
        {
            Debug.LogWarning("Sound " + audioName + "not found");
            return;
        }
        s.source.loop = desiredLoopState;
    }
}
