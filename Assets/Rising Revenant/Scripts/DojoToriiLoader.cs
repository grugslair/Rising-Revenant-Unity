using bottlenoselabs.C2CS.Runtime;
using Dojo;
using dojo_bindings;
using System.Numerics;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;
using SimpleGraphQL;

public class DojoToriiLoader : MonoBehaviour
{
    [SerializeField]
    private SynchronizationMaster synchManager;
    [SerializeField]
    private VideoPlayer videoPlayer;
    [SerializeField]
    private UIStateManager uiStateManager;
    [SerializeField]
    private WorldManager worldManager;


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
     
        // this needs to query for the latest game via a custom query get the game id and base everything from that

        // then load in game data structs
        // outpost
        // events
        // player if there 
        // rememebr that this will crash if they are empty....

     */

    private void OnDisable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoLoopPointReached;
        }
    }

    private async void OnEnable()
    {
        LoadingHasCompleted = true;
        if (actuallyLoad)
        {
            foreach (var outpost in DojoEntitiesDataManager.outpostDictInstance)
            {
                outpost.Value.SetOutpostTexture();
            }

            var gameId = await RisingRevenantUtils.FetchLatestGameId();  
            //Debug.Log($"btw this is the latest game id {gameId}");                 
            //SubscribeToGameData(gameId);
            DojoEntitiesDataManager.currentGameId = new Dojo.Starknet.FieldElement(gameId);
            Debug.Log($"btw this is the latest game id {DojoEntitiesDataManager.currentGameId.Hex()}");


            worldManager.LoadData();

            LoadingHasCompleted = true;
        }
        else
        {
            LoadingHasCompleted = true ;
        }
    }

 
    private async void SubscribeToGameData(int gameId)
    {
        Debug.Log("subscribing to game data");
        await synchManager.CustomSynchronizeEntities("GameState", new string[] { gameId.ToString() });
        Debug.Log("subscribing to outpost data");
        try
        {
            await synchManager.CustomSynchronizeEntities("Outpost", new string[] { gameId.ToString() });
        }
        catch (Exception ex)
        {
            Debug.LogError($"The query was empty there are no outposts right now");
        }

        if (DojoEntitiesDataManager.currentAccount.Address.Hex() != "")
        {
            try
            {
                if (DojoEntitiesDataManager.currentAccount.Address.Hex() != "")
                {
                    Debug.Log("subscribing to player data");
                    await synchManager.CustomSynchronizeEntities("DevWallet", new string[] { DojoEntitiesDataManager.currentAccount.Address.Hex(), gameId.ToString() });
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"The query was empty the user has no data available");
            }
        }
    }
}
