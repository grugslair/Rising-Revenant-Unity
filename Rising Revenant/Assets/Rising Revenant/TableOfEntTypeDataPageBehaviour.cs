
using SimpleGraphQL;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TableOfEntTypeDataPageBehaviour : Menu
{
    public List<GameObject> headers;
    public TMP_Dropdown dropdownMenu;

    public GameObject listParent;
    public List<GameObject> prefabs;

    public Texture2D[] sortState = new Texture2D[2];
    public List<List<GameObject>> headersOptionsList = new List<List<GameObject>>(); // this will be used when one of the headers will be called
    //so we can reset everything

    [SerializeField]
    private bool ascOrder = true;
    [SerializeField]
    private int currentHeader = 0;
    [SerializeField]
    private string dataPointName = "LIFE";


    private string[] graphqlQueryStructure = new string[2]
    {

         @"
        query {
            playerInfoModels (
                where: { 
                    game_id: ""GAMEID"",
                }
                order: { direction: DIR, field: DATAPOINT }
                ) {
                  edges {
                    node {
                      entity {
                        keys
                        models {
                          __typename
                          ... on PlayerInfo {
                            game_id
                            reinforcements_available_count
                            outpost_count
                            player_id

                          }
                        }
                      }
                    }
                  }
                }
              }
        ",

        @"
        query {
            outpostModels (
            where: { 
                game_id: ""GAMEID"",
            }
            order: { direction: DIR, field: DATAPOINT }
            ) {
              edges {
                node {
                  entity {
                    keys
                    models {
                      __typename
                      ... on Outpost {
                        game_id
                        life
                        owner
                        position{
                          x
                          y
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        "
       
    };




    private string lastSavedGraphqlQueryStructure = @"
           query {
      tradeReinforcementModels(where: {status : 1, game_id: 1}) {
        edges{
          node{
            entity{
              keys
              models{
                ... on TradeReinforcement{
                __typename
                entity_id
                buyer
                seller
                price
                count
                game_id
                }
              }
            }
          }
        }
      }
    }";

    private void Start()
    {
        lastSavedGraphqlQueryStructure = RisingRevenantUtils.ReplaceWords(lastSavedGraphqlQueryStructure, new Dictionary<string, string> 
        { 
            { "DIR", "ASC" },
            { "DATAPOINT", "LIFE" },
            { "GAMEID", RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString() },
        });
    }

    public void onHeaderChange()
    {
        foreach (Transform child in listParent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < headers.Count; i++)
        {
            if (dropdownMenu.value == i)
            {
                headers[i].gameObject.SetActive(true);
            }
            else
            {
                headers[i].gameObject.SetActive(false);
            }
        }
    }


    public void CallToSetDatapointName(string dataPointName)
    {
        if (dataPointName == this.dataPointName)
        {
            ascOrder = !ascOrder; //switch the direction
        }
        else
        {
            ascOrder = true;
        }

        this.dataPointName = dataPointName;
    }

    public void CallToSortSelection(int headerNum)
    {
        lastSavedGraphqlQueryStructure = RisingRevenantUtils.ReplaceWords(graphqlQueryStructure[headerNum], new Dictionary<string, string>
        {
            { "DIR", ascOrder == true ? "ASC" : "DESC" },
            { "DATAPOINT", this.dataPointName },
            { "GAMEID", RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString() },
        });

        switch (headerNum)
        {
            case 0:
                PlayerInfoDataQuery();
                break;
            case 1:
                OutpostDataQuery();
                break;
        }
        //call for update
    }








    public async void PlayerInfoDataQuery()
    {
        foreach (Transform child in listParent.transform)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("this is for the playerinfo");
        Debug.Log(lastSavedGraphqlQueryStructure);

        var client = new GraphQLClient(DojoCallsManager.graphlQLEndpoint);
        var tradesRequest = new Request
        {
            Query = lastSavedGraphqlQueryStructure,
        };

        var responseType = new
        {
            playerInfoModels = new
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
                                        game_id = "",
                                        reinforcements_available_count= "",
                                        outpost_count= "",
                                        player_id = ""
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        var response = await client.Send(() => responseType, tradesRequest);

        Debug.Log(response.Data.playerInfoModels.edges.Length);

        for (int i = 0; i < response.Data.playerInfoModels.edges.Length; i++)
        {
            var edge = response.Data.playerInfoModels.edges[i];

            for (int x = 0; x < edge.node.entity.models.Length; x++)
            {
                if (edge.node.entity.models[x].__typename == "PlayerInfo")
                {
                    GameObject instance = Instantiate(prefabs[0], transform.position, Quaternion.identity);

                    instance.transform.parent = listParent.transform;
                    var contributions = await SubPlayerInfoDataQueryContribution(edge.node.entity.models[x].player_id); 
                    TMP_Text[] allTexts = instance.GetComponentsInChildren<TMP_Text>();

                    //address
                    allTexts[0].text = edge.node.entity.models[x].player_id.Substring(0,8);
                    // contributions
                    allTexts[1].text = contributions;
                    // outpost count
                    allTexts[2].text = edge.node.entity.models[x].outpost_count;
                    //reinf
                    allTexts[3].text = edge.node.entity.models[x].reinforcements_available_count;
                    //trades
                    allTexts[4].text = "0";
                }
            }
        }
    }

    public async Task<string> SubPlayerInfoDataQueryContribution(string playerId)
    {

        string query = @"
            query {
                playerContributionModels ( where: { game_id: ""GAMEID"" , player_id : ""PLAYERID""} ) {
                  edges {
                    node {
                      entity {
                        keys
                        models {
                          __typename
                          ... on PlayerContribution {
                            game_id
                            player_id
                            score
                          }
                        }
                      }
                    }
                  }
                }
              } ";

        query = RisingRevenantUtils.ReplaceWords(query, new Dictionary<string, string>
        {
            { "GAMEID", RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString() },
            { "PLAYERID", playerId },
        });

        var client = new GraphQLClient(DojoCallsManager.graphlQLEndpoint);
        var tradesRequest = new Request
        {
            Query = query,
        };

        var responseType = new
        {
            playerContributionModels = new
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
                                        game_id= "",
                                        player_id= "",
                                        score= "",
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        var response = await client.Send(() => responseType, tradesRequest);

        if (response.Data.playerContributionModels.edges.Length > 0)
        {
            for (int i = 0; i < response.Data.playerContributionModels.edges.Length; i++)
            {
                var edge = response.Data.playerContributionModels.edges[i];

                for (int x = 0; x < edge.node.entity.models.Length; x++)
                {
                    if (edge.node.entity.models[x].__typename == "PlayerContribution")
                    {
                        return response.Data.playerContributionModels.edges[i].node.entity.models[x].score;
                    }
                }
            }
        }
        else
        {
            return "0";
        }

        return "0";
    }




    public async void OutpostDataQuery()
    {
        foreach (Transform child in listParent.transform)
        {
            Destroy(child.gameObject);
        }
        
        var client = new GraphQLClient(DojoCallsManager.graphlQLEndpoint);
        var tradesRequest = new Request
        {
            Query = lastSavedGraphqlQueryStructure,
        };

        var responseType = new
        {
            outpostModels = new
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
                                        game_id = "",
                                        life= "",
                                        owner= "",
                                        position = new {
                                          x= "",
                                          y= "",
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        var response = await client.Send(() => responseType, tradesRequest);

        Debug.Log(response.Data.outpostModels.edges.Length);


        for (int i = 0; i < response.Data.outpostModels.edges.Length; i++)
        {

            var edge = response.Data.outpostModels.edges[i];

            for (int x = 0; x < edge.node.entity.models.Length; x++)
            {
                if (edge.node.entity.models[x].__typename == "Outpost")
                {

                    Debug.Log("this is for the outpost");
                    GameObject instance = Instantiate(prefabs[1], transform.position, Quaternion.identity);

                    instance.transform.parent = listParent.transform;
                    TMP_Text[] allTexts = instance.GetComponentsInChildren<TMP_Text>();

                    //outpost position
                    int posX = int.Parse(edge.node.entity.models[x].position.x);
                    int posY = int.Parse(edge.node.entity.models[x].position.y);

                    allTexts[0].text = $"ID: {RisingRevenantUtils.CantonPair(posX, posY)}";

                    //life
                    allTexts[1].text = edge.node.entity.models[x].life;
                    //owne
                    allTexts[2].text = edge.node.entity.models[x].owner.Substring(0, 8);
                    //po
                    allTexts[3].text = $"(X:{edge.node.entity.models[x].position.x} || Y:{edge.node.entity.models[x].position.y})";
                    //bein sold
                    allTexts[4].text = "0";
                }
            }
        }
    }


}
