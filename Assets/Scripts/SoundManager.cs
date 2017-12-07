using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    [Header("Game Audio")]
    public AudioSource as_game;
    public AudioClip ac_fanfare;

    [Header("Objects Audio")]
    public AudioSource as_objects;
    public AudioClip ac_objectfound;

    private float as_playerPitch;

    private float lowPitchRange = .95f;
    private float highPitchRange = 1.05f;


    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void Fanfare()
    {
        as_game.PlayOneShot(ac_fanfare);
    }

    public void ObjectFound()
    {
        as_objects.PlayOneShot(ac_objectfound);
    }

}