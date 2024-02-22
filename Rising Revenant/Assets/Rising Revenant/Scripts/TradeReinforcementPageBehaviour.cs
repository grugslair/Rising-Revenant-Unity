using SimpleGraphQL;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeReinforcementPageBehaviour : MonoBehaviour
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
    [Header("For Address Sorting ")]
    public TMP_InputField addressInputField;

    [Space(50)]
    public TMP_Dropdown dropdown;
    private string graphqlEndpoint = "http://127.0.0.1:8080/graphql";



    // this should change the lastsprting method and then call the function above
    
    private string[] graphqlQueryStructure = new string[3]
    {
    @"query {
        tradeReinforcementModels(
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
                            ... on TradeReinforcement {
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
    }",

    @"query {
        tradeReinforcementModels(
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
                            ... on TradeReinforcement {
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
    }",

    @"query {
        tradeReinforcementModels(
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
                            ... on TradeReinforcement {
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

    // might need some testing because we dont know if the string can just work without the whole query thing 
    //add a timer every 5 seconds to refresh



    private void OnEnable()
    {
        //InvokeRepeating("NewLoad", 0f, 5f);
        // this needs to get the value of the bools and set the correct image
    }

    private void OnDisable()
    {
       // CancelInvoke("NewLoad");
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

        switch (currentSortingMethod)
        {
            case 0: //latest
                queryStruct = graphqlQueryStructure[2];

                queryStruct.Replace("GAME_ID", "1");
                queryStruct.Replace("NUM_DATA", "25");

                break;

            case 1: //count

                queryStruct = graphqlQueryStructure[0];

                queryStruct.Replace("GAME_ID", "1");
                queryStruct.Replace("NUM_DATA", "25");

                queryStruct.Replace("MAX_VAL", sliderForCountSorting.currentMaxValue.ToString());
                queryStruct.Replace("MIN_VAL", sliderForCountSorting.currentMinValue.ToString());

                queryStruct.Replace("LTE_VAR", "countLTE");
                queryStruct.Replace("GTE_VAR", "countGTE");

                queryStruct.Replace("FIELD_NAME", "COUNT");

                break;

            case 2: //price

                queryStruct = graphqlQueryStructure[0];

                queryStruct.Replace("GAME_ID", "1");
                queryStruct.Replace("NUM_DATA", "25");


                if (priceSortingMaxInput.text == "" && priceSortingMinInput.text == "") { return; }  // if both are empty then return

                int number;
                bool isNumeric;


                if (priceSortingMaxInput.text != "")  // if price is somehting then check what it is
                {
                    isNumeric = int.TryParse(priceSortingMaxInput.text, out number);
                    if (!isNumeric) { return; }  // if it is not a valide number 
                    if (number < 0) { return; }
                    queryStruct.Replace("MAX_VAL", number.ToString());
                    queryStruct.Replace("LTE_VAR", "priceLTE");
                }
                else
                {
                    queryStruct.Replace("LTE_VAR: MAX_VAL,", "");
                }

                if (priceSortingMinInput.text != "")
                {
                    isNumeric = int.TryParse(priceSortingMinInput.text, out number);
                    if (!isNumeric) { return; }
                    if (number < 0) { return; }
                    queryStruct.Replace("MIN_VAL", number.ToString());
                    queryStruct.Replace("GTE_VAR", "priceGTE");
                }
                else
                {
                    queryStruct.Replace("GTE_VAR: MIN_VAL", "");
                }



                queryStruct.Replace("FIELD_NAME", "PRICE");

                break;

            case 3: // address

                //see if the value is correct


                queryStruct = graphqlQueryStructure[1];


                queryStruct.Replace("GAME_ID", "1");
                queryStruct.Replace("NUM_DATA", "25");

                queryStruct.Replace("SELLER", addressInputField.text);

                break;
        }


        if (queryStruct.Length > 0)
        {

        }


    }

    public async void NewLoad()
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

        for (int i = 0; i < response.Data.tradeReinforcementModels.edges.Length; i++)
        {
            var edge = response.Data.tradeReinforcementModels.edges[i];

            for (int x = 0; x < edge.node.entity.models.Length; x++)
            {
                if (edge.node.entity.models[x].__typename == "TradeReinforcement")
                {
                    GameObject instance = Instantiate(reinfTradePrefab, transform.position, Quaternion.identity);

                    instance.transform.parent = tradesParent.transform;
                    instance.GetComponent<TradeReinforcementsUiElement>().Initialize(edge.node.entity.models[x].price, edge.node.entity.models[x].count, edge.node.entity.models[x].seller.ToString(), edge.node.entity.models[x].entity_id);
                }
            }
        }
    }




    public void CallForSortMenuSelection()
    {

        int selectedSort = dropdown.value;

        Debug.Log(selectedSort);

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
