using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VidPlayer : MonoBehaviour
{

    [SerializeField]
    private string videoName;

    private void Start()
    {
        PlayVideo();
    }

    public void PlayVideo()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer)
        {
            string videoPath =  System.IO.Path.Combine(Application.streamingAssetsPath, videoName);
            Debug.Log(videoPath);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }

}
