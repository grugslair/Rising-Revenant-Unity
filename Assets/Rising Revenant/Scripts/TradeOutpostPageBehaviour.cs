
using SimpleGraphQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
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

    [Space(30)]
    [Header("For Cost Sorting")]
    public TMP_InputField priceSortingMinInput;
    public TMP_InputField priceSortingMaxInput;

    [Space(30)]
    [Header("For Address Sorting ")]
    public TMP_InputField addressInputField;

    [Space(50)]
    public TMP_Dropdown dropdown;

    private string[] graphqlQueryStructure = new string[3]
    {
        @"query {
            outpostTradeModels(
                where: { 
                    game_id: GAME_ID,
                    status: 1,
                    LTE_VAR: MAX_VAL,
                    GTE_VAR: MIN_VAL
                }
                last: NUM_DATA
                order: { direction: DESC, field: FIELD_NAME }
            ) {
                edges {
                    node {
                        entity {
                            keys
                            models {
                                ... on OutpostTrade {
                                    __typename
                                    game_id
                                    trade_id
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
        }",

        @"query {
            outpostTradeModels(
                where: { 
                    game_id: GAME_ID,
                    status: 1,
                    seller: SELLER
                }
            ) {
                edges {
                    node {
                        entity {
                            keys
                            models {
                                ... on OutpostTrade {
                                    __typename
                                    game_id
                                    trade_id
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
        }",

        @"query {
            outpostTradeModels(
                where: { 
                    game_id: GAME_ID, 
                    status: 1 
                } 
                last: NUM_DATA
            ) {
                edges {
                    node {
                        entity {
                            keys
                            models {
                                ... on OutpostTrade {
                                     __typename
                                    game_id
                                    trade_id
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
        }"
    };














    private string lastSavedGraphqlQueryStructure = @"
      query {
        outpostTradeModels(where: {status : 1, game_id: ""GAME_ID""}) {
          edges {
            node {
              entity {
                keys
                models {
                  ... on OutpostTrade { 
                    __typename
                    game_id
                    trade_id
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

    IEnumerator CallRefreshTradesPeriodically()
    {
        while (true)
        {
            RefreshTrades();
            yield return new WaitForSeconds(60f);
        }
    }

    private void OnEnable()
    {
        lastSavedGraphqlQueryStructure = RisingRevenantUtils.ReplaceWords(lastSavedGraphqlQueryStructure, new Dictionary<string, string> { { "GAME_ID", RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString() } });
        StartCoroutine(CallRefreshTradesPeriodically());
    }

    private void OnDisable()
    {
        StopCoroutine(CallRefreshTradesPeriodically());
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






    public void CallForSort()
    {
        var queryStruct = "";
        var dictOfWordChanges = new Dictionary<string, string>();

        dictOfWordChanges.Add("GAME_ID", '"' + RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString() + '"');
        dictOfWordChanges.Add("NUM_DATA", "25");  //this needs to be a variable

        switch (currentSortingMethod)
        {
            case 0: //latest
                queryStruct = graphqlQueryStructure[2];
                break;

            case 1: //price

                queryStruct = graphqlQueryStructure[0];

                if (priceSortingMaxInput.text == "" && priceSortingMinInput.text == "") { return; }  // if both are empty then return

                int number;
                bool isNumeric;

                if (priceSortingMaxInput.text != "")  // if price is somehting then check what it is
                {
                    isNumeric = int.TryParse(priceSortingMaxInput.text, out number);
                    if (!isNumeric) { return; }  // if it is not a valide number 
                    if (number < 0) { return; }
                    dictOfWordChanges.Add("MAX_VAL", '"' + number.ToString() + '"');
                    dictOfWordChanges.Add("LTE_VAR", "priceLTE");
                }
                else
                {
                    dictOfWordChanges.Add("LTE_VAR: MAX_VAL,", "");
                }

                if (priceSortingMinInput.text != "")
                {
                    isNumeric = int.TryParse(priceSortingMinInput.text, out number);
                    if (!isNumeric) { return; }
                    if (number < 0) { return; }
                    dictOfWordChanges.Add("MIN_VAL", '"' + number.ToString() + '"');
                    dictOfWordChanges.Add("GTE_VAR", "priceGTE");
                }
                else
                {
                    dictOfWordChanges.Add("GTE_VAR: MIN_VAL", "");
                }

                dictOfWordChanges.Add("FIELD_NAME", "PRICE");

                break;

            case 2: 

                queryStruct = graphqlQueryStructure[1];

                dictOfWordChanges.Add("SELLER", '"' + addressInputField.text + '"');
                break;
        }

        queryStruct = RisingRevenantUtils.ReplaceWords(queryStruct, dictOfWordChanges);
    
        if (queryStruct.Length > 0)
        {
            lastSavedGraphqlQueryStructure = queryStruct;

            RefreshTrades();
        }
    }

    public async void RefreshTrades()
    {
        foreach (Transform child in tradesParent.transform)
        {
            Destroy(child.gameObject);
        }


        var client = new GraphQLClient(DojoEntitiesDataManager.worldManager.chainConfig.toriiUrl);
        var tradesRequest = new Request
        {
            Query = lastSavedGraphqlQueryStructure,
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

    public void CallForSortMenuSelection()
    {
        int selectedSort = dropdown.value;

        for (int i = 0; i < sortingAlgos.Length; i++)
        {
            if (i == selectedSort)
            {
                sortingAlgos[i].SetActive(true);
            }
            else
            {
                sortingAlgos[i].SetActive(false);
            }
        }

        currentSortingMethod = selectedSort;
    }

}
