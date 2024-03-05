
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableOfEntTypeDataPageBehaviour : Menu
{
    public List<GameObject> headers;
    public TMP_Dropdown dropdownMenu;

    public GameObject parentPrefab;
    public List<GameObject> prefabs;

    public RawImage[] sortState = new RawImage[2];
    public List<List<GameObject>> headersOptionsList = new List<List<GameObject>>(); // this will be used when one of the headers will be called
    //so we can reset everything

    public enum SortedHeader { PLAYER_INFO, OUTPOST }
    public enum PlayerInfoSortData { CONTRIB_SCROE, REVENANTS_COUNT, REINF, TRADES }
    public enum OutpostSortData { LIFES  }


    [Space(30)]
    public GameObject playerInfoPrefabbElement;
    [Space(30)]
    public GameObject outpostPrefabElement;
    [Space(30)]
    public GameObject eventPrefabElement;

    private string[] graphqlQueryStructure = new string[2]
 {
    @"  query {
    outpostModels(
      where: { game_id: GAME_ID }
      first: NUM_DATA
      order: { direction: DIR , field: VAR_NAME }
    ) {
      edges {
        node {
          entity {
            keys
            models {
              __typename
              ... on Outpost {
                game_id
                entity_id
                owner
                name_outpost
                x
                y
                lifes
                shield
                reinforcement_count
                status
                last_affect_event_id
              }
            }
          }
        }
      }
    }
  }",

    @"query {
    playerinfoModels(
      where: { game_id: GAME_ID }
      first: NUM_DATA
      order: { direction: DIR , field: VAR_NAME }
    ) {
      edges {
        node {
          entity {
            keys
            models {
              __typename
              ... on PlayerInfo {
                game_id
                owner
                score
                score_claim_status
                earned_prize
                revenant_count
                outpost_count
                reinforcement_count
                inited
              }
            }
          }
        }
      }
    }
  }"
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

    public void onHeaderChange()
    {
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




    public void CallToSortSelection(int headerNum, int dataName)
    {

    }



    /*
    public async void RequestForPlayerInfoData()
    {
        var client = new GraphQLClient(graphqlEndpoint);
        var request = new Request
        {
            Query = lastSavedGraphqlQueryStructure,
        };

        var responseType = new
        {
            tradeReinforcementModels = new
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
                                    entity_id = "",
                                    buyer = "",
                                    seller = "",
                                    price = "",
                                    count = "",
                                    game_id = "",
                                }
                            }
                        }
                    }
                }
                }
            }
        };

        var response = await client.Send(() => responseType, request);

        //for (int i = 0; i < response.Data.tradeReinforcementModels.edges.Length; i++)
        //{
        //    var edge = response.Data.tradeReinforcementModels.edges[i];

        //    for (int x = 0; x < edge.node.entity.models.Length; x++)
        //    {
        //        if (edge.node.entity.models[x].__typename == "TradeReinforcement")
        //        {
        //            GameObject instance = Instantiate(reinfTradePrefab, transform.position, Quaternion.identity);

        //            instance.transform.parent = tradesParent.transform;
        //            instance.GetComponent<TradeReinforcementsUiElement>().Initialize(edge.node.entity.models[x].price, edge.node.entity.models[x].count, edge.node.entity.models[x].seller.ToString(), edge.node.entity.models[x].entity_id);
        //        }
        //    }
        //}
    }
    */
}
