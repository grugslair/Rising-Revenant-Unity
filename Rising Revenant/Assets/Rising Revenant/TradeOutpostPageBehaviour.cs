
using SimpleGraphQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TradeOutpostPageBehaviour : Menu
{
    public GameObject[] sortingAlgos;
    public GameObject tradesParent;
    public GameObject reinfTradePrefab;
    private int currentSortingMethod;

    [Space(30)]
    private bool hidingYourTrades;
    public RawImage hidingYourTradesImg;

    private bool hidingOthersTrades;
    public RawImage hidingOthersTradesImg;


    private string lastSavedGraphqlQueryStructure = @"
      query {
        outpostTradeModels(where: {status : 1, game_id: ""GAME_ID""}) {
          edges {
            node {
              entity {
                keys
                models {
                  __typename
                  ... on OutpostTrade {
                    game_id
                    trade_id
                    trade_type
                    buyer
                    price
                    seller
                    status
                    offer {
                      x
                      y
                    }
                  }
                }
              }
            }
          }
        }
      }";


    private void OnEnable()
    {
        RefreshTrades();
    }

    public void HideYourTrades()
    {
        hidingYourTrades = !hidingYourTrades;
        RunCheckBoxSorting();
    }
    public void HideOthersTrades()
    {
        hidingOthersTrades = !hidingOthersTrades;
        RunCheckBoxSorting();
    }

    private void RunCheckBoxSorting()
    {
        var account = DojoEntitiesDataManager.currentAccount.Address.Hex();

        foreach (Transform child in tradesParent.transform)
        {
            bool owned = child.GetComponent<TradeReinforcementsUiElement>().sellerWholeHex == account;

            if (!owned && hidingOthersTrades)
            {
                child.gameObject.SetActive(false);
            }
            else if (!owned && !hidingOthersTrades)
            {
                child.gameObject.SetActive(true);
            }

            if (owned && hidingYourTrades)
            {
                child.gameObject.SetActive(false);
            }
            else if (owned && !hidingYourTrades)
            {
                child.gameObject.SetActive(true);
            }
        }
    }


    public async void RefreshTrades()
    {
        foreach (Transform child in tradesParent.transform)
        {
            Destroy(child.gameObject);
        }

        var dict = new Dictionary<string, string>
        {
            { "GAME_ID", "0" }
        };

        var query = RisingRevenantUtils.ReplaceWords(lastSavedGraphqlQueryStructure, dict);

        var client = new GraphQLClient(DojoCallsManager.graphlQLEndpoint);
        var tradesRequest = new Request
        {
            Query = query,
        };

        var responseType = new
        {
            outpostTradeModels = new
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
                                    trade_id = "",
                                    trade_type = "",
                                    buyer = "",
                                    price = "",
                                    seller = "",
                                    status = "",
                                    offer = new
                                    {
                                        x= "",
                                        y = "",
                                    },
                                }
                                }
                            }
                        }
                    }
                }
            }
        };

        var response = await client.Send(() => responseType, tradesRequest);

        for (int i = 0; i < response.Data.outpostTradeModels.edges.Length; i++)
        {
            var edge = response.Data.outpostTradeModels.edges[i];

            for (int x = 0; x < edge.node.entity.models.Length; x++)
            {
                if (edge.node.entity.models[x].__typename == "OutpostTrade")
                {
                    GameObject instance = Instantiate(reinfTradePrefab, transform.position, Quaternion.identity);

                    instance.transform.SetParent(tradesParent.transform);

                    var id = new RisingRevenantUtils.Vec2
                    {
                        x = (UInt32)int.Parse(edge.node.entity.models[x].offer.x),
                        y = (UInt32)int.Parse(edge.node.entity.models[x].offer.y)
                    };

                    instance.GetComponent<OutpostSellingContainer>().Initilize(
                        id,
                        edge.node.entity.models[x].price,
                        edge.node.entity.models[x].trade_id
                    );
                }
            }
        }
    }

}
