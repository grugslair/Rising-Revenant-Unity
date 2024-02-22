using Dojo;
using UnityEngine;
using UnityEngine.Video;

public class DojoToriiLoader : MonoBehaviour
{
    [SerializeField]
    private SynchronizationMaster synchManager;
    [SerializeField]
    private VideoPlayer videoPlayer;
    [SerializeField]
    private UIStateManager uiStateManager;

    private bool videoHasCompletedOnce = false; 

    private bool LoadingHasCompleted = false; 
    public bool actuallyLoad = true;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoLoopPointReached;
    }

    private void OnVideoLoopPointReached(VideoPlayer source)
    {
        videoHasCompletedOnce = true;
        videoPlayer.loopPointReached -= OnVideoLoopPointReached;
    }

    private void Update()
    {
        if (videoHasCompletedOnce && LoadingHasCompleted)
        {
            uiStateManager.SetUiState(2);
        }
    }

    // this should get the last game id
    /*
     * then load everything about that specific game 
     * so query go like this 
     * 
     * 
     * 
     * game counter
     * 
     * 
     * game data
     * 
     * 
     * revs
     * 
     * 
     * events
     * 
     * 
     * remember if none then it should create one and wait 
     * 
     */

    private void OnDisable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoLoopPointReached;
        }
    }

    private void OnEnable()
    {
        LoadingHasCompleted = true;
        if (actuallyLoad)
        {
            bool outcome = synchManager.CallFromLoadingPage(DojoEntitiesDataManager.currentAccount);  //theoretically should return bool and recursion the thing if false
            Debug.Log(outcome);

            foreach (var outpost in DojoEntitiesDataManager.outpostDictInstance)
            {
                outpost.Value.SetOutpostTexture();
            }

            LoadingHasCompleted = true;
        }
        else
        {
            LoadingHasCompleted = true ;
        }
    }
}
