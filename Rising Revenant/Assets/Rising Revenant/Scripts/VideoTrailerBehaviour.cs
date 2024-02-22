using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoTrailerBehaviour : MonoBehaviour
{
    [SerializeField]
    private UIStateManager uIStateManager;

    [SerializeField]
    private GameObject buttonToContinue;

    [SerializeField]
    private VideoPlayer videoPlayer;

    public float seconds = 5;


    private void OnVideoStarted(VideoPlayer source)
    {
        StartCoroutine(StartCountdown(seconds));
    }

    private IEnumerator StartCountdown(float duration)
    {
        yield return new WaitForSeconds(duration);
        buttonToContinue.SetActive(true);
    }

    private void OnVideoEnded(VideoPlayer source)
    {
        uIStateManager.SetUiState(3); 
    }

    private void OnEnable()
    {
        videoPlayer.started += OnVideoStarted;
        videoPlayer.loopPointReached += OnVideoEnded;

        buttonToContinue.SetActive(false);
    }

    private void OnDisable()
    {
        videoPlayer.started -= OnVideoStarted;
        videoPlayer.loopPointReached -= OnVideoEnded;
    }
}
