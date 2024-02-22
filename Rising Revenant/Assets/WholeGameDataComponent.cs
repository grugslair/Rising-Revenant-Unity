using SimpleGraphQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class WholeGameDataComponent : MonoBehaviour
{
    public TMP_Text outpostText;
    public TMP_Text tradesText;
    public TMP_Text ReinforcmentsText;

    private Coroutine repeatingFunctionCoroutine;

    private string graphqlEndpoint = "http://127.0.0.1:8080/graphql";


    private string deadOutpostsGraphqlQueryStructure = @"
       query {
    outpostModels(
      where: { game_id: 1, lifes: 0 }
    ) {
      edges {
        node {
          entity {
            keys
            models {
              __typename
              ... on Outpost {
                lifes
              }
            }
          }
        }
      }
    	totalCount
    }
  }";






    //massive issue apparently you are not allowed to do stuiff to @ strings so make a function

    private string tradesGraphqlQueryStructure = "query { tradeReinforcementModels(where: { game_id: 1, status: STATUS }) { edges { node { entity { keys models { __typename ... on TradeReinforcement { status } } } } } totalCount } }";


    private void OnEnable()
    {
        // Start the repeating function when the GameObject is enabled
        repeatingFunctionCoroutine = StartCoroutine(RepeatingFunction());

    }

    private void OnDisable()
    {
        // Stop the repeating function when the GameObject is disabled
        if (repeatingFunctionCoroutine != null)
        {
            StopCoroutine(repeatingFunctionCoroutine);
            repeatingFunctionCoroutine = null;
        }
    }

    private IEnumerator RepeatingFunction()
    {
        // Repeat this loop until the GameObject is disabled
        while (true)
        {
            PopulateData();
            // Wait for 10 seconds before continuing the loop
            yield return new WaitForSeconds(10f);
        }
    }


    private async void PopulateData()
    {
        var testVar = 1;

        var deadOutpostsNum = await GetAmountOfDeadOutpostsQuery();
        var outpostDataStr = $"Total Outpost: {DojoEntitiesDataManager.gameEntityCounterInstance.outpostCount}\n Dead: {deadOutpostsNum}\nAlive: {DojoEntitiesDataManager.gameEntityCounterInstance.outpostCount - deadOutpostsNum}\n\n Volume Trades: -1";
        outpostText.text = outpostDataStr;


        var arrOfDataOfTrades = await GetAmountOfTradesQuery();
        var tradesDataStr = $"Total Trades: {DojoEntitiesDataManager.gameEntityCounterInstance.tradeCount}\nSold: {arrOfDataOfTrades[1]}\nRevoked: {arrOfDataOfTrades[2]}\nActive: {arrOfDataOfTrades[0]}";
        tradesText.text = tradesDataStr;


        var reinfDataStr = $"In Wallets: {DojoEntitiesDataManager.gameEntityCounterInstance.reinforcementCount}\nIn Outposts: {DojoEntitiesDataManager.gameEntityCounterInstance.remainLifeCount}\n\nVolume Trades: {-1}";
        ReinforcmentsText.text = reinfDataStr;



        //var deadOutpostsNum = await GetAmountOfDeadOutpostsQuery();
        //var outpostDataStr = $"Total Outpost: {testVar}\nDead: {deadOutpostsNum}\nAlive: {testVar - deadOutpostsNum}\n\nVolume Trades: -1";
        //outpostText.text = outpostDataStr;


        //var arrOfDataOfTrades = await GetAmountOfTradesQuery();
        //var tradesDataStr = $"Total Trades: {testVar}\nSold: {arrOfDataOfTrades[1]}\nRevoked: {arrOfDataOfTrades[2]}\nActive: {arrOfDataOfTrades[0]}";
        //tradesText.text = tradesDataStr;


        //var reinfDataStr = $"In Wallets: {testVar}\nIn Outposts: {testVar}";
        //ReinforcmentsText.text = reinfDataStr;
    }


    public async Task<int> GetAmountOfDeadOutpostsQuery()
    {
        var client = new GraphQLClient(graphqlEndpoint);
        var request = new Request
        {
            Query = deadOutpostsGraphqlQueryStructure,
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
                                        lifes = "", 
                                    }
                                }
                            }
                        }
                    }
                },
                totalCount = ""
            }
        };

        var response = await client.Send(() => responseType, request);
        Debug.Log(response.Data.outpostModels.totalCount);

        if (response.Data != null && response.Data.outpostModels != null && !string.IsNullOrEmpty(response.Data.outpostModels.totalCount))
        {
            // Parse totalCount to an int
            if (int.TryParse(response.Data.outpostModels.totalCount, out int totalCount))
            {
                return totalCount;
            }
            else
            {
                Debug.LogError("no data");
                return -1;
            }
        }
        else
        {
            Debug.LogError("invalid data");
            return -1;
        }
    }


    public async Task<int[]> GetAmountOfTradesQuery()
    {
        var arrOfInts = new int[3];

        var strQuery = tradesGraphqlQueryStructure;
        var client = new GraphQLClient(graphqlEndpoint);

        strQuery = strQuery.Replace("STATUS", ((int)RisingRevenantUtils.TradesStatus.SELLING).ToString());

        var tradesRequest = new Request
        {
            Query = strQuery,
        };

        Debug.Log(strQuery);

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
                                        status = "",
                                    }
                                }
                            }
                        }
                    }
                },
                totalCount = ""
            }
        };

        var response = await client.Send(() => responseType, tradesRequest);

        if (response.Data != null && response.Data.tradeReinforcementModels != null && !string.IsNullOrEmpty(response.Data.tradeReinforcementModels.totalCount))
        {
            // Parse totalCount to an int
            if (int.TryParse(response.Data.tradeReinforcementModels.totalCount, out int totalCount))
            {
                arrOfInts[0] = totalCount;
            }
            else
            {
                Debug.LogError("no data");
                arrOfInts[0] = -1;
            }
        }
        else
        {
            Debug.LogError("invalid data");
            arrOfInts[0] = -1;
        }







        strQuery = tradesGraphqlQueryStructure;

        strQuery = strQuery.Replace("STATUS", ((int)RisingRevenantUtils.TradesStatus.SOLD).ToString());

        tradesRequest = new Request
        {
            Query = strQuery,
        };

         response = await client.Send(() => responseType, tradesRequest);

        if (response.Data != null && response.Data.tradeReinforcementModels != null && !string.IsNullOrEmpty(response.Data.tradeReinforcementModels.totalCount))
        {
            // Parse totalCount to an int
            if (int.TryParse(response.Data.tradeReinforcementModels.totalCount, out int totalCount))
            {
                arrOfInts[1] = totalCount;
            }
            else
            {
                Debug.LogError("no data");
                arrOfInts[1] = -1;
            }
        }
        else
        {
            Debug.LogError("invalid data");
            arrOfInts[1] = -1;
        }





        strQuery = tradesGraphqlQueryStructure;

        strQuery = strQuery.Replace("STATUS", ((int)RisingRevenantUtils.TradesStatus.REVOKED).ToString());

        tradesRequest = new Request
        {
            Query = strQuery,
        };

        response = await client.Send(() => responseType, tradesRequest);

        if (response.Data != null && response.Data.tradeReinforcementModels != null && !string.IsNullOrEmpty(response.Data.tradeReinforcementModels.totalCount))
        {
            // Parse totalCount to an int
            if (int.TryParse(response.Data.tradeReinforcementModels.totalCount, out int totalCount))
            {
                arrOfInts[2] = totalCount;
            }
            else
            {
                Debug.LogError("no data");
                arrOfInts[2] = -1;
            }
        }
        else
        {
            Debug.LogError("invalid data");
            arrOfInts[2] = -1;
        }


        return arrOfInts;
    }



}
