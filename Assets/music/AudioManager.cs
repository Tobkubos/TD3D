using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public sounds[] sounds;

    public static AudioManager instance;

    private void Start()
    {
        Play("Theme");
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.playOnAwake = s.playonawake;
            s.source.loop = s.loop;
        }
    }


    public void ChangeVolume(float change)
    {
        foreach (sounds s in sounds)
        {
            s.source.volume = change;
        }
    }

    public void Play(string name, float PitchOffset = 0)
    {
        sounds s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " wasn't found!");
            return;
        }
        s.source.pitch = 1 + PitchOffset;
        s.source.Play();
    }
}


[System.Serializable]
public class sounds
{
    public string name;

    public AudioClip clip;

    public bool playonawake;

    [Range(0f, 1f)]
    //[HideInInspector]
    public float volume;
    public float pitch;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

