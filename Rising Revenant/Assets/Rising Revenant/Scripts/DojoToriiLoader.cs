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

    string queryForLastActiveGame = @"
     query {
        gameStateModels (first: 1)
        {
          edges {
            node {
              entity {
                keys
                models {
                  ... on GameState {
                    __typename
               	    game_id
                  }
                }
              }
            }
          }
        }
      }";

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

            var gameId = await latestGameId();  // this shuold also set the dojo currentgmae id
            Debug.Log(gameId);                  // no it shoudlnt what yu on abut
            //SubscribeToGameData(gameId);

            LoadingHasCompleted = true;
        }
        else
        {
            LoadingHasCompleted = true ;
        }
    }

    private async Task<int> latestGameId()
    {
        var client = new GraphQLClient(DojoCallsManager.graphlQLEndpoint);
        var tradesRequest = new Request
        {
            Query = queryForLastActiveGame,
        };

        var responseType = new
        {
            gameStateModels = new
            {
                edges = new[]
                {
                    new
                    {
                        node = new
                        {
                            entity = new
                            {
                                keys = new[]
                                {
                                    ""
                                },
                                models = new[]
                                {
                                    new
                                    {
                                        __typename = "",
                                        game_id = ""
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        try
        {
            var response = await client.Send(() => responseType, tradesRequest);

            if (response.Data.gameStateModels.edges.Length == 0)
            {
                Debug.Log("no game was ever created");
                return -1;
            }

            if (response.Data != null && response.Data.gameStateModels != null)
            {
                for (int i = 0; i < response.Data.gameStateModels.edges.Length; i++)
                {
                    var edge = response.Data.gameStateModels.edges[i];

                    for (int x = 0; x < edge.node.entity.keys.Length; x++)
                    {
                        Debug.Log("key: " + edge.node.entity.keys[x]);
                    }

                    for (int x = 0; x < edge.node.entity.models.Length; x++)
                    {
                        if (edge.node.entity.models[x].__typename == "GameState")
                        {
                            return RisingRevenantUtils.GeneralHexToInt(edge.node.entity.models[x].game_id);
                        }
                    }
                }
            }
            Debug.LogError($"Failed to parse data for status");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Query failed for status: {ex.Message}");
        }

        return 0;
    }

    private async void LoadGameDataModelsIn(int gameId)
    {

    }

    private async void LoadGameEntitiesDataModelsIn(int gameId)
    {

    }

    //will need webgl if statement
    //also therre is something wrong with the game id
    private void SubscribeToGameData(int gameId)
    {
        Debug.Log("subscribing to game data, " + gameId);

        var gameStateModel = new dojo.KeysClause[]
        { new() { model_ = CString.FromString("GameState"), keys = new[] { gameId.ToString() } } };

        var outpostMarketModel = new dojo.KeysClause[]
        { new() { model_ = CString.FromString("OutpostMarket"), keys = new[] { gameId.ToString() } } };

        var gamePotModel = new dojo.KeysClause[]
        { new() { model_ = CString.FromString("GamePot"), keys = new[] { gameId.ToString() } } };

        worldManager.toriiClient.AddModelsToSync(gameStateModel);
        worldManager.toriiClient.AddModelsToSync(outpostMarketModel);
        worldManager.toriiClient.AddModelsToSync(gamePotModel);

        if (DojoEntitiesDataManager.currentAccount != null)
        {
            var devWalletModel = new dojo.KeysClause[]
            { new() { model_ = CString.FromString("DevWallet"), keys = new[] { gameId.ToString(), DojoEntitiesDataManager.currentAccount.Address.Hex().ToLower() } } };

            var playerContribModel = new dojo.KeysClause[]
            { new() { model_ = CString.FromString("PlayerContribution"), keys = new[] { gameId.ToString(), DojoEntitiesDataManager.currentAccount.Address.Hex().ToLower() } } };

            //here i need to do a query for the existance
            Debug.Log("something");
            worldManager.toriiClient.AddModelsToSync(devWalletModel);
            worldManager.toriiClient.AddModelsToSync(playerContribModel);
        }
    }
}
