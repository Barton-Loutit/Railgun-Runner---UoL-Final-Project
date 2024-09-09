using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

//This script is effectively being used as a container for AudioSource
//variables to ease the management of audio clips.

//I was originally going to include all the variable instantiation
//in the AudioManager, but once I referenced a audio tutorial on YouTube
// by Brackeys that uses this class container methodology, I feel it's
// a better and easier implementation.

//Note that this implementation of the Sound class was inspired by
//a tutorial from Brackeys here: https://www.youtube.com/watch?v=6OT43pvUyfY&t


[System.Serializable]
public class Sound
{

    public string audioName;
    public AudioClip clip;
    
    [Range(0f, 1f)]
    public float volume;

    [Range(0f, 5f)]
    public float pitch;

    public bool loop;
    public bool playOnAwake;

    [HideInInspector]
    public AudioSource source;
}
