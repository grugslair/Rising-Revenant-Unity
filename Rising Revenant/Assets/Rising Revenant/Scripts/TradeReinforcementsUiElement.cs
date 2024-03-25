using Dojo.Starknet;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeReinforcementsUiElement : MonoBehaviour
{
    public TMP_Text sellerText;
    public TMP_Text reinforcementCountText;
    public TMP_Text priceText;

    public Button interactButton;

    public string sellerWholeHex;

    private bool owner;
    private FieldElement tradeId;

    [SerializeField]
    private TooltipAsker tooltipAsker;

    public void Initialize(string price, string count, string seller, string tradeId)
    {
        sellerWholeHex = seller;

        sellerText.text = $"Seller: {seller.Substring(0, 8)}";
        reinforcementCountText.text = $"Reinforcements: {count}";
        priceText.text = $"Price: {RisingRevenantUtils.GeneralHexToInt(price)} $LORDS";

        this.tradeId = new FieldElement(tradeId);

        TMP_Text buttonText = interactButton.GetComponentInChildren<TMP_Text>();

        Debug.Log($"seller: {seller}");
        Debug.Log($"current account: {DojoEntitiesDataManager.currentAccount.Address.Hex().Substring(0, 2) + DojoEntitiesDataManager.currentAccount.Address.Hex().Substring(3)}");

        if (seller == DojoEntitiesDataManager.currentAccount.Address.Hex().Substring(0, 2) + DojoEntitiesDataManager.currentAccount.Address.Hex().Substring(3) )
        {
            owner = true;
            interactButton.onClick.AddListener(RevokeTrade);
            Debug.Log("is the owner of this trade");
            buttonText.text = "Revoke";

            tooltipAsker.OnTooltipShown += OnTooltipEnable;
        }
        else
        {
            owner = false;
            Debug.Log("is not the owner of this trade");
            interactButton.onClick.AddListener(PurchaseTrade);
            buttonText.text = "Purchase";
        }
    }

    private void OnTooltipEnable(GameObject tooltip)
    {
        tooltip.GetComponent<ChangePriceTooltip>().Initilize(2, tradeId);
    }

    public async void RevokeTrade()
    {
        try
        {
            if (owner)
            {
                var revokeStruct = new DojoCallsManager.RevokeTradeReinforcementStruct { gameId = DojoEntitiesDataManager.currentGameId, tradeId = tradeId };
                var endpoint = new DojoCallsManager.EndpointDojoCallStruct { account = DojoEntitiesDataManager.currentAccount, addressOfSystem = DojoCallsManager.tradeReinforcementActionsAddress, functionName = "revoke" };

                await DojoCallsManager.RevokeTradeReinforcementDojoCall(revokeStruct, endpoint);
                Destroy(gameObject); 
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to revoke trade: {ex.Message}");
        }
    }

    public async void PurchaseTrade()
    {
        try
        {
            if (!owner)
            {
                var purchaseStruct = new DojoCallsManager.PurchaseTradeReinforcementStruct { gameId = DojoEntitiesDataManager.currentGameId, tradeId = tradeId };
                var endpoint = new DojoCallsManager.EndpointDojoCallStruct { account = DojoEntitiesDataManager.currentAccount, addressOfSystem = DojoCallsManager.tradeReinforcementActionsAddress, functionName = "purchase" };

                await DojoCallsManager.PurchaseTradeReinforcementDojoCall(purchaseStruct, endpoint);
                Destroy(gameObject);

                UiEntitiesReferenceManager.notificationManager.CreateNotification("Trade purchased successfully",null, 3);
            }
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to purchase trade: ", null, 3);
            Debug.LogError($"Failed to purchase trade: {ex.Message}");
        }
    }
}
