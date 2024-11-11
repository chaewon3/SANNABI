using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audiosource;
    public AudioClip[] arrAudio;

    void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }



}
