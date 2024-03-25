using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellOutpostPageBehaviour : Menu
{
    [SerializeField]
    private GameObject shieldContainer;
    [SerializeField]
    private RawImage outpostPPImage;

    [SerializeField]
    private TMP_Text outpostDataText;
    [SerializeField]
    private TMP_Text outpostPositionText;

    [SerializeField]
    private TMP_InputField costInputField;

    [SerializeField]
    private Outpost currentlySelectedOutpost;
    
    private int currentOutpostIndex;
    private List<RisingRevenantUtils.Vec2> listOfSellableOutposts = new List<RisingRevenantUtils.Vec2>();

    [SerializeField]
    private RectTransform mapView;
    [SerializeField]
    private RectTransform outpostMarker;

    public bool loaded = false;

    private void OnEnable()
    {
        if (DojoEntitiesDataManager.ownOutpostIndex.Count == 0)
        {
            currentOutpostIndex = -1;
            return;
        }

       LoadSellableOutposts();
    }

    /// <summary>
    /// here we check if the outposts are on sale and if they are not we add them to the list of sellable outposts
    /// </summary>
    /// <returns></returns>
    private async void LoadSellableOutposts()
    {
        listOfSellableOutposts.Clear();

        var listOfOutpostsOnSale = await RisingRevenantUtils.GetAllOutpostSelling(RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString(), 1);

        loaded = false;

        foreach (var item in DojoEntitiesDataManager.ownOutpostIndex)
        {
            var outpost = DojoEntitiesDataManager.outpostDictInstance[item];

            if (outpost.life > 0)
            {
                if (!listOfOutpostsOnSale.Contains(outpost.position)) 
                {
                    listOfSellableOutposts.Add(outpost.position);
                }
            }
        }

        loaded = true;
        SetTextsAndData(0);
        currentOutpostIndex = 0;
    }

    /// <summary>
    /// called from the UI to cycle through the outposts
    /// </summary>
    /// <param name="dir"></param>
    public void CycleThroughOutposts(int dir)
    {
        var newIdx = currentOutpostIndex + dir;



        if (newIdx < 0 || newIdx >= listOfSellableOutposts.Count) { return; }

        SetTextsAndData(newIdx);
    }

    /// <summary>
    /// sets the texts and data for the currently selected outpost, takes in the idx of the array where all the outposts are saved 
    /// </summary>
    /// <param name="idx"></param>
    private void SetTextsAndData(int idx)
    {
        currentOutpostIndex = idx;
        currentlySelectedOutpost = DojoEntitiesDataManager.outpostDictInstance[listOfSellableOutposts[idx]];

        outpostDataText.text = $"Outpost Id: {RisingRevenantUtils.CantonPair((int)currentlySelectedOutpost.position.x, (int)currentlySelectedOutpost.position.y)} \n" +
                             $"Lifes: {currentlySelectedOutpost.life} \n" +
                             $"Reinforcement type: {currentlySelectedOutpost.reinforcementType.ToCustomString()}\n" +
                             $"Reinforcement Left: {currentlySelectedOutpost.reinforcesRemaining} \n" +
                             $"Revenant name: {RisingRevenantUtils.GetFullRevenantName(currentlySelectedOutpost.position)}";

        outpostPositionText.text = $"Position:\nX: {currentlySelectedOutpost.position.x} Y: {currentlySelectedOutpost.position.y}";

        int i = 0;
        foreach (Transform child in shieldContainer.transform)
        {
            if (i >= RisingRevenantUtils.CalculateShields(currentlySelectedOutpost.life))
            {
                RawImage image = child.GetComponent<RawImage>();
                image.color = new Color(1, 1, 1, 0);
            }

            i++;
        }

        SetUpOutpostPic(new Vector2(currentlySelectedOutpost.position.x, currentlySelectedOutpost.position.y));
    }
    
    private void SetUpOutpostPic(Vector2 pos)
    {
        outpostMarker.gameObject.SetActive(true);
        var compHeight = mapView.rect.height;
        var compWidth = mapView.rect.width;

        float scaledX = (currentlySelectedOutpost.position.x / RisingRevenantUtils.MAP_WIDHT) * compWidth;
        float scaledY = (currentlySelectedOutpost.position.y / RisingRevenantUtils.MAP_HEIGHT) * compHeight;

        outpostMarker.anchoredPosition = new Vector2(scaledX, scaledY);
    }
    
    /// <summary>
    /// function to call on sell of the outpost
    /// </summary>
    public async void SellOutpost()
    {
        if (costInputField.text == "") { return; }

        try
        {
            DojoCallsManager.CreateTradeRevenantStruct structToSellReinf = new DojoCallsManager.CreateTradeRevenantStruct
            {
                revenantId = currentlySelectedOutpost.position,
                gameId = DojoEntitiesDataManager.currentGameId,
                priceRevenant = new Dojo.Starknet.FieldElement(int.Parse(costInputField.text).ToString("X"))
            };
            DojoCallsManager.EndpointDojoCallStruct endPoint = new DojoCallsManager.EndpointDojoCallStruct
            {
                account = DojoEntitiesDataManager.currentAccount,
                addressOfSystem = DojoCallsManager.tradeOutpostActionsAddress,
                functionName = "create",
            };

            await DojoCallsManager.CreateTradeRevenantDojoCall(structToSellReinf, endPoint);
        }
        catch (Exception ex) // You can catch more specific exceptions if needed
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
