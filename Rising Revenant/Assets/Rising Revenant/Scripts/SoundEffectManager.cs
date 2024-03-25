using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance { get; private set; }
    public AudioSource soundEffectSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySoundEffect(AudioClip clip, bool priority)
    {
        if (priority || !soundEffectSource.isPlaying)
        {
            soundEffectSource.Stop(); 
            soundEffectSource.clip = clip; 
            soundEffectSource.Play(); 
        }
    }
}
