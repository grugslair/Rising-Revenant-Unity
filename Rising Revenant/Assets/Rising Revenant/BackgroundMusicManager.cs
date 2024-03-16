using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioSource backgroundMusicSource; 

    public void StartBackgroundMusic()
    {
        if (!backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Play();
        }
    }

    public void TogglePauseBackgroundMusic(bool pause)
    {

        if (pause)
        {
            backgroundMusicSource.Pause();
        }
        else
        {
            backgroundMusicSource.UnPause();
        }
    }

    public void SetVolume(int volume)
    {
        backgroundMusicSource.volume = volume / 100f;
    }
}
