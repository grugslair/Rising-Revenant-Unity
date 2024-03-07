using System;
using System.Collections;
using System.Collections.Generic;
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
    private string tradeId;

    public void Initialize(string price, string count, string seller, string tradeId)
    {
        sellerWholeHex = seller;

        sellerText.text = $"Seller: {seller.Substring(0, 8)}";
        reinforcementCountText.text = $"Reinforcements: {count}";
        priceText.text = $"Price: {RisingRevenantUtils.GeneralHexToInt(price)} $LORDS";

        this.tradeId = tradeId;

        TMP_Text buttonText = interactButton.GetComponentInChildren<TMP_Text>();

        Debug.Log($"Seller: {seller} - Current Account: {DojoEntitiesDataManager.currentAccount.Address.Hex()}");

        if (seller == DojoEntitiesDataManager.currentAccount.Address.Hex())
        {
            owner = true;
            interactButton.onClick.AddListener(RevokeTrade);
            buttonText.text = "Revoke";
        }
        else
        {
            owner = false;
            interactButton.onClick.AddListener(PurchaseTrade);
            buttonText.text = "Purchase";
        }
    }


    public async void RevokeTrade()
    {
        try
        {
            if (owner)
            {
                var revokeStruct = new DojoCallsManager.RevokeTradeReinforcementStruct { gameId = DojoEntitiesDataManager.currentGameId, tradeId = new Dojo.Starknet.FieldElement(tradeId) };
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

    public async void ChangePrice()
    {
        try
        {
            if (owner)
            {
                var changeTradePriceStruct = new DojoCallsManager.ModifyTradeReinforcementStruct { gameId = DojoEntitiesDataManager.currentGameId, price = new Dojo.Starknet.FieldElement(20), tradeId = new Dojo.Starknet.FieldElement(tradeId) };
                var endpoint = new DojoCallsManager.EndpointDojoCallStruct { account = DojoEntitiesDataManager.currentAccount, addressOfSystem = DojoCallsManager.tradeReinforcementActionsAddress, functionName = "revoke" };

                await DojoCallsManager.ModifyTradeReinforcementDojoCall(changeTradePriceStruct, endpoint);


                // need to change the price here
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to change price: {ex.Message}");
        }
    }


    public async void PurchaseTrade()
    {
        try
        {
            if (!owner)
            {
                var purchaseStruct = new DojoCallsManager.PurchaseTradeReinforcementStruct { gameId = DojoEntitiesDataManager.currentGameId, tradeId = new Dojo.Starknet.FieldElement(tradeId) };
                var endpoint = new DojoCallsManager.EndpointDojoCallStruct { account = DojoEntitiesDataManager.currentAccount, addressOfSystem = DojoCallsManager.tradeReinforcementActionsAddress, functionName = "purchase" };

                await DojoCallsManager.PurchaseTradeReinforcementDojoCall(purchaseStruct, endpoint);
                Destroy(gameObject); // Assuming 'gameObject' is the context you want to destroy
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to purchase trade: {ex.Message}");
        }
    }
}
