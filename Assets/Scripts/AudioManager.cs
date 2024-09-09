/*
 * The AudioManager class is used to implement a singleton AudioManager 
 * which will be responsible for playing and controlling all audio
 * aspects of the game.
 * 
 * This currently includes background music, Weapon Firing (laser/bomb) SFX,
 * Player Death SFX, enemy hit and death SFX, and pickup collection SFX.
 * 
 * The Audio Manager is persistent across all scenes, and will destroy
 * a new audio manager if present across a scene transition.
 * 
 * Note that this implementation of the audio manager was inspired by
 * a tutorial from Brackeys here: https://www.youtube.com/watch?v=6OT43pvUyfY&t
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    //This is the singleton instance which will be used to play sounds
    //in other scripts
    public static AudioManager audioManagerInstance { get; private set; }
    
    //This is the array of audio clips that we expect to play.
    public Sound[] sounds;

    //Setup the singleton - if there is no existing instance assigned,
    //then this instance should be used as the singleton, and persist
    //across scene transitions. Else, this should be destroyed.   
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

    //Background Music should be played from game start
    void Start()
    {
        Play("BackgroundMusic");
    }

    //This method allows us to play any Sound with a corresponding
    //audioname. If the audioName provided does not exist, throw a warning.
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

    //This method allows us to stop a sound as it is playing.
    //This method is not currently in use (since setLoopState is used for lasers)
    //but is here for usability in future iterations.
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

    //This method sets the loop state of a sound provided to the method.
    //Toggles the "Loop" switch of an audioclip.
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
