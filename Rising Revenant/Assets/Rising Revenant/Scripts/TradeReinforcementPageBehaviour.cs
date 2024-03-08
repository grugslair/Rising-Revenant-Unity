using SimpleGraphQL;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeReinforcementPageBehaviour : Menu
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
    [Header("For Count Sorting")]
    public DoubleHandSlider sliderForCountSorting;

    [Space(30)]
    [Header("For Cost Sorting")]
    public TMP_InputField priceSortingMinInput;
    public TMP_InputField priceSortingMaxInput;

    [Space(30)]
    [Header("For Address Sorting")]
    public TMP_InputField addressInputField;

    [Space(50)]
    public TMP_Dropdown dropdown;

    // this should change the lastsprting method and then call the function above
    
    private string[] graphqlQueryStructure = new string[3]
    {
    @"query {
        reinforcementTradeModels(
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
                            ... on ReinforcementTrade {
                               __typename
                                game_id
                                trade_id
                                price
                                seller
                                offer
                                status
                            }
                        }
                    }
                }
            }
        }
    }",

    @"query {
        reinforcementTradeModels(
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
                            ... on ReinforcementTrade {
                        __typename
                                game_id
                                trade_id
                                price
                                seller
                                offer
                                status
                            }
                        }
                    }
                }
            }
        }
    }",

    @"query {
        reinforcementTradeModels(
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
                            ... on ReinforcementTrade {
                        __typename
                                game_id
                                trade_id
                                price
                                seller
                                offer
                                status
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
             reinforcementTradeModels(where: {status : 1, game_id: ""GAME_ID""}) {
              edges {
                node {
                  entity {
                    keys
                    models {
                      __typename
                      ... on ReinforcementTrade {
                        __typename
                        game_id
                        trade_id
                        price
                        seller
                        offer
                        status
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
        lastSavedGraphqlQueryStructure = RisingRevenantUtils.ReplaceWords(lastSavedGraphqlQueryStructure, new Dictionary<string, string> { { "GAME_ID", RisingRevenantUtils.FieldElementToInt( DojoEntitiesDataManager.currentGameId).ToString() } });
        StartCoroutine(CallRefreshTradesPeriodically()); 
    }

    private void OnDisable()
    {
        StopCoroutine(CallRefreshTradesPeriodically());
    }









    public void HideYourTrades()
    {
        //invert bool set image

        hidingYourTrades = !hidingYourTrades;

        RunCheckBoxSorting();
    }
    public void HideOthersTrades()
    {
        //invert bool set image
        hidingOthersTrades = !hidingOthersTrades;

        RunCheckBoxSorting();
    }

    // this is fine
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

        dictOfWordChanges.Add("GAME_ID", '"' + RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString() + '"' );
        dictOfWordChanges.Add("NUM_DATA", "25");  //this needs to be a variable

        switch (currentSortingMethod)
        {
            case 0: //latest
                queryStruct = graphqlQueryStructure[2];
                break;

            case 1: //count

                queryStruct = graphqlQueryStructure[0];

                dictOfWordChanges.Add("MAX_VAL", sliderForCountSorting.currentMaxValue.ToString());
                dictOfWordChanges.Add("MIN_VAL", sliderForCountSorting.currentMinValue.ToString());

                dictOfWordChanges.Add("LTE_VAR", "offerLTE");
                dictOfWordChanges.Add("GTE_VAR", "offerGTE");

                dictOfWordChanges.Add("FIELD_NAME", "OFFER");

                break;

            case 2: //price

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

            case 3: // address

                //see if the value is correct

                queryStruct = graphqlQueryStructure[1];

                dictOfWordChanges.Add("SELLER", '"' + addressInputField.text + '"');
                break;
        }

        queryStruct = RisingRevenantUtils.ReplaceWords(queryStruct, dictOfWordChanges);
        Debug.Log(queryStruct);

        if (queryStruct.Length > 0)
        {
            Debug.Log("this is getting called");
            lastSavedGraphqlQueryStructure = queryStruct;

            Debug.Log(queryStruct);
            RefreshTrades();
        }
    }

    public async void RefreshTrades()
    {
        foreach (Transform child in tradesParent.transform)
        {
            Destroy(child.gameObject);
        }
           
        Debug.Log(lastSavedGraphqlQueryStructure);

        var client = new GraphQLClient(DojoCallsManager.graphlQLEndpoint);
        var tradesRequest = new Request
        {
            Query = lastSavedGraphqlQueryStructure,
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
                                        __typename = "",
                                        game_id = "",
                                        trade_id = "",
                                        seller = "",
                                        price = "",
                                        status = "",
                                        offer = ""
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        var response = await client.Send(() => responseType, tradesRequest);

        Debug.Log(response.Data.reinforcementTradeModels.edges.Length);


        for (int i = 0; i < response.Data.reinforcementTradeModels.edges.Length; i++)
        {

            var edge = response.Data.reinforcementTradeModels.edges[i];

            for (int x = 0; x < edge.node.entity.models.Length; x++)
            {
                if (edge.node.entity.models[x].__typename == "ReinforcementTrade")
                {
                    GameObject instance = Instantiate(reinfTradePrefab, transform.position, Quaternion.identity);

                    instance.transform.parent = tradesParent.transform;
                    instance.GetComponent<TradeReinforcementsUiElement>().Initialize(
                        edge.node.entity.models[x].price,
                        edge.node.entity.models[x].offer,
                        edge.node.entity.models[x].seller.ToString(),
                        edge.node.entity.models[x].trade_id);
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
