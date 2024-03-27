using SimpleGraphQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class WholeGameDataPageBehaviour : Menu
{
    /* FOR OUTPOST DATA
        how many total
        how many alive

        how many in trades
    
    */


    /* FOR TRADES DATA
        //total amoutn of trades

        //split in the middle 
        // amount of trades for the reinf
        // amount of trades for the outposts
        
        //for each active
        //sold 
        //revoked
    */


    /* FOR REINF DATA
        // how many in total
        // how many placed 
        // how many in wallets


    */


    public TMP_Text outpostText;
    public TMP_Text ReinforcmentsText;

    public TMP_Text tradesAmountText;
    public TMP_Text tradesReinfAmountText;
    public TMP_Text tradesOutpostAmountText;

    private Coroutine repeatingFunctionCoroutine;


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
        var outpostDataStr = $"Total Outpost: {DojoEntitiesDataManager.gameEntCounter.outpostCreatedCount}\nDead: {DojoEntitiesDataManager.gameEntCounter.outpostCreatedCount - DojoEntitiesDataManager.gameEntCounter.outpostRemainingCount}\nAlive: {DojoEntitiesDataManager.gameEntCounter.outpostRemainingCount}";
        outpostText.text = outpostDataStr;

        // Sell Sold Rev
        var arrOfDataOfTradesReinf = await GetAmountOfTradesQueryAsync(false);
        var arrOfDataOfTradesOutpost = await GetAmountOfTradesQueryAsync(true);

        var totTrades = arrOfDataOfTradesReinf[0] + arrOfDataOfTradesReinf[1] + arrOfDataOfTradesReinf[2] + arrOfDataOfTradesOutpost[0] + arrOfDataOfTradesOutpost[1] + arrOfDataOfTradesOutpost[2];

        tradesAmountText.text = $"Total Trades: {totTrades}";
        tradesReinfAmountText.text = $"Reinf\n\nActive: {arrOfDataOfTradesReinf[0]}\nRevoke: {arrOfDataOfTradesReinf[1]}\nSold: {arrOfDataOfTradesReinf[2]}";
        tradesOutpostAmountText.text = $"Outposts\n\nActive: {arrOfDataOfTradesOutpost[0]}\nRevoke: {arrOfDataOfTradesOutpost[1]}\nSold: {arrOfDataOfTradesOutpost[2]}";

        var reinfDataStr = $"In Wallets: {DojoEntitiesDataManager.gameEntCounter.reinforcementCount}\nIn Outposts: {DojoEntitiesDataManager.gameEntCounter.remainLifeCount}";
        ReinforcmentsText.text = reinfDataStr;
    }
    


    #region Amount Of Trades Per Status Query
    public async Task<int[]> GetAmountOfTradesQueryAsync(bool outposts)
    {
        var statuses = new[]
        {
            RisingRevenantUtils.TradesStatus.SELLING,
            RisingRevenantUtils.TradesStatus.SOLD,
            RisingRevenantUtils.TradesStatus.REVOKED
        };

        var results = new List<int>();

        if (outposts)
        {
            foreach (var status in statuses)
            {
                var totalCount = await QueryTradeCountOutpostAsync(status);
                results.Add(totalCount);
            }
        }
        else
        {
            foreach (var status in statuses)
            {
                var totalCount = await QueryTradeCountReinforcementAsync(status);
                results.Add(totalCount);
            }
        }

        return results.ToArray();
    }

    private async Task<int> QueryTradeCountReinforcementAsync(RisingRevenantUtils.TradesStatus status)
    {
        string tradesGraphqlQueryStructure = @"
            query {
              reinforcementTradeModels(where: { game_id: ""GAME_ID"" , status: STATUS }) {
                edges {
                  node {
                    entity {
                      keys
                      models {
                        __typename
                        ... on ReinforcementTrade {
                          status
                        }
                      }
                    }
                  }
                }
                totalCount
              }
            }";

        var dict = new Dictionary<string, string>();

        dict.Add("GAME_ID", RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString());
        dict.Add("STATUS", ((int)status).ToString());

        var query = RisingRevenantUtils.ReplaceWords(tradesGraphqlQueryStructure, dict);

        var client = new GraphQLClient(DojoEntitiesDataManager.worldManager.chainConfig.toriiUrl);
        var tradesRequest = new Request
        {
            Query = query,
        };

        var responseType = new
        {
            reinforcementTradeModels = new
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

        try
        {
            var response = await client.Send(() => responseType, tradesRequest);


            if (response.Data.reinforcementTradeModels.edges.Length == 0)
            {
                return 0;
            }

            if (response.Data != null && response.Data.reinforcementTradeModels != null && int.TryParse(response.Data.reinforcementTradeModels.totalCount, out int totalCount))
            {
                return totalCount;
            }
            Debug.LogError($"Failed to parse data for status {status}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Query failed for status {status}: {ex.Message}");
        }

        return -1;
    }

    private async Task<int> QueryTradeCountOutpostAsync(RisingRevenantUtils.TradesStatus status)
    {
        string tradesGraphqlQueryStructure = @"
            query {
              outpostTradeModels(where: { game_id: ""GAME_ID"", status: STATUS }) {
                edges {
                  node {
                    entity {
                      keys
                      models {
                        __typename
                        ... on OutpostTrade {
                          status
                        }
                      }
                    }
                  }
                }
                totalCount
              }
            }";

        var dict = new Dictionary<string, string>();

        dict.Add("GAME_ID",  RisingRevenantUtils.FieldElementToInt( DojoEntitiesDataManager.currentGameId).ToString()  );
        dict.Add("STATUS", ((int)status).ToString());

        var query = RisingRevenantUtils.ReplaceWords(tradesGraphqlQueryStructure, dict);

        Debug.Log(query);
        var client = new GraphQLClient(DojoEntitiesDataManager.worldManager.chainConfig.toriiUrl);
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

        try
        {
            var response = await client.Send(() => responseType, tradesRequest);

            if (response.Data.outpostTradeModels.edges.Length == 0)
            {
                return 0;
            }

            if (response.Data != null && response.Data.outpostTradeModels != null && int.TryParse(response.Data.outpostTradeModels.totalCount, out int totalCount))
            {
                return totalCount;
            }
            Debug.LogError($"Failed to parse data for status {status}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Query failed for status {status}: {ex.Message}");
        }

        return 0;
    }
    #endregion
}
