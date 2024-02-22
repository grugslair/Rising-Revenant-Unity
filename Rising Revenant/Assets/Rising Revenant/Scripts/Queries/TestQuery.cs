using Dojo;
using dojo_bindings;
using SimpleGraphQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class TestQuery : MonoBehaviour
{

    public GraphQLConfig Config;

    public string graphqlEndpoint = "http://127.0.0.1:8080/graphql";
    public string graphqlQuery = @"
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

    //public WorldManager worldManager;
    public bool run = true;


    private void Update()
    {
        if (run)
        {
            run = false;
            //TestDojoQuery();
        }
    } 

    public async void FormQuery()
    {
        Debug.Log(graphqlQuery);

        var client = new GraphQLClient(graphqlEndpoint);
        var request = new Request
        {
            Query = graphqlQuery,
        };

        var responseType = new
        {
            TradeModels = new
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

        for (int i = 0; i < response.Data.TradeModels.edges.Length; i++)
        {
            var edge = response.Data.TradeModels.edges[i];

            for (int x = 0; x < edge.node.entity.keys.Length; x++)
            {
                Debug.Log("key: " + edge.node.entity.keys[x]);
            }

            for (int x = 0; x < edge.node.entity.models.Length; x++)
            {
                if (edge.node.entity.models[x].__typename == "Trade")
                {
                    Debug.Log("buyer: " + edge.node.entity.models[x].buyer);
                    Debug.Log("seller: " + edge.node.entity.models[x].seller);
                    Debug.Log("price: " + edge.node.entity.models[x].price);
                    Debug.Log("count: " + edge.node.entity.models[x].count);
                    Debug.Log("game_id: " + edge.node.entity.models[x].game_id);
                }
            }
        }
    }

}
