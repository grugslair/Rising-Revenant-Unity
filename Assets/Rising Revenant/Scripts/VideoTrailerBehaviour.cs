using System.Collections;
using UnityEngine;
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

    public BackgroundMusicManager backgroundMusicManager;

    private void OnVideoStarted(VideoPlayer source)
    {
        backgroundMusicManager.TogglePauseBackgroundMusic(true);
        StartCoroutine(StartCountdown(seconds));
    }

    private IEnumerator StartCountdown(float duration)
    {
        yield return new WaitForSeconds(duration);
        buttonToContinue.SetActive(true);
    }

    private void OnVideoEnded(VideoPlayer source)
    {
        SkipVideo();
    }

    public void SkipVideo()
    {
        backgroundMusicManager.TogglePauseBackgroundMusic(false);
        if (DojoEntitiesDataManager.currentWorldEvent != null)
        {
            uIStateManager.SetUiState(4);
        }
        else
        {
            uIStateManager.SetUiState(3);
        }
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
