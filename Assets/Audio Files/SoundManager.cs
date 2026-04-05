using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    //The sounds in the inspecter have to match this list. (Ambiance1 = Capstone environment Ambiance)
    Ambiance1,
    Amiance2,
    Orb1,
    Orb2,
    WallBreak1,
    WallBreak2,
    Teleporter,

}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    } 
}
