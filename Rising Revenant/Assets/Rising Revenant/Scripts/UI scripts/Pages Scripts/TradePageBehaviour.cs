using SimpleGraphQL;
using System;
using TMPro;
using UnityEngine;

public class TradePageBehaviour : Menu
{
    // trades reinf
    [SerializeField]
    private GameObject tradesUiParent;
    [SerializeField]
    private GameObject tradesPrefab;


    // sell reinf
    [Space(40)]
    [SerializeField]
    private CounterUiElement reinfSellCounter;
    [SerializeField]
    private TMP_InputField reinfSellInputField;


    private string graphqlEndpoint = "http://127.0.0.1:8080/graphql";
    private string graphqlQuery = @"
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
    // no the trade page behaviour should only deal witht he changing of the screens inside the page parent



    // trades outpost

    // sell outpost


    public async void SellReinforcements()
    {
        if (reinfSellCounter.currentValue == 0 || reinfSellInputField.text == "") { return; }

        DojoCallsManager.CreateTradeReinforcementStruct structToSellReinf = new DojoCallsManager.CreateTradeReinforcementStruct
        {
            count = (UInt32)reinfSellCounter.currentValue,
            gameId = 1,
            price = new Dojo.Starknet.FieldElement(int.Parse(reinfSellInputField.text))
        };

        DojoCallsManager.EndpointDojoCallStruct endPoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            account = DojoEntitiesDataManager.currentAccount,
            addressOfSystem = DojoEntitiesDataManager.dojoEntInitializer.tradeReinfActionsAddress,
            functionName = "create",
        };

        await DojoCallsManager.CreateTradeReinforcementDojoCall(structToSellReinf, endPoint);
    }

    public async void RefreshTrades()
    {

        var client = new GraphQLClient(graphqlEndpoint);
        var request = new Request
        {
            Query = graphqlQuery,
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

        for (int i = 0; i < response.Data.tradeReinforcementModels.edges.Length; i++)
        {
            var edge = response.Data.tradeReinforcementModels.edges[i];

            for (int x = 0; x < edge.node.entity.keys.Length; x++)
            {
                Debug.Log("key: " + edge.node.entity.keys[x]);
            }

            for (int x = 0; x < edge.node.entity.models.Length; x++)
            {

                if (edge.node.entity.models[x].__typename == "TradeReinforcement")
                {
                    GameObject instance = Instantiate(tradesPrefab, transform.position, Quaternion.identity);

                    instance.transform.parent = tradesUiParent.transform;
                    instance.GetComponent<TradeReinforcementsUiElement>().Initialize(edge.node.entity.models[x].price, edge.node.entity.models[x].count, edge.node.entity.models[x].seller.ToString(), edge.node.entity.models[x].entity_id);

                    //Debug.Log("buyer: " + edge.node.entity.models[x].buyer);
                    //Debug.Log("seller: " + edge.node.entity.models[x].seller);
                    //Debug.Log("Price: " + edge.node.entity.models[x].price);
                    //Debug.Log("count: " + edge.node.entity.models[x].count);
                    //Debug.Log("game_id: " + edge.node.entity.models[x].game_id);
                }
            }
        }

    }


}
