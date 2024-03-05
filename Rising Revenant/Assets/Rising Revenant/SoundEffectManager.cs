using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public AudioSource soundEffectSource;

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
