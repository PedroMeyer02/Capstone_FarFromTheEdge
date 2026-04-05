using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource audioSource;
    public AudioClip teleportClip;
    public AudioClip wallClip;
    public AudioClip orbClip;
    public AudioClip audioClip;
    public AudioClip audioClip2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else { Destroy(gameObject); }
    }

    public void PlayTeleport()
    {
        audioSource.clip = teleportClip;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    public void PlayWall()
    {
        audioSource.clip = wallClip;
        audioSource.Play();
    }

    public void PlayOrb()
    {
        audioSource.clip = orbClip;
        audioSource.Play();
    }

}