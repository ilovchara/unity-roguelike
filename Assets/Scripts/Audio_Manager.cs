using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    private static Audio_Manager _instance;

    public static Audio_Manager Instance
    {
        get
        {
            return _instance;
        }
    }



    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    public AudioSource efxSorce;
    public AudioSource bgSorce;

    public AudioSource efxSource;

    private void Awake()
    {
        _instance = this;
    }


    public void RandomPlay(params AudioClip[] dips)
    {
        float pitch = Random.Range(minPitch, maxPitch);
        int index = Random.Range(0, dips.Length);
        AudioClip clip = dips[index];
        efxSource.clip = clip;
        efxSource.pitch = pitch;
        efxSource.Play();
    }

    public void StopBgMusic()
    {
        bgSorce.Stop();
    }
}
