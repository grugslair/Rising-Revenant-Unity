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


        Debug.Log(seller);
        Debug.Log(sellerWholeHex);
        Debug.Log(DojoEntitiesDataManager.currentAccount.Address.Hex());
        Debug.Log(new Dojo.Starknet.FieldElement(sellerWholeHex).Hex());

        if (seller == DojoEntitiesDataManager.currentAccount.Address.Hex())
        {
            owner = true;
            interactButton.onClick.AddListener(RevokeTrade);
            if (buttonText != null) buttonText.text = "Revoke";
        }
        else
        {
            owner = false;
            interactButton.onClick.AddListener(PurchaseTrade);
            if (buttonText != null) buttonText.text = "Purchase";
        }
    }


    public async void RevokeTrade() {
        if (owner)
        {
            var revokeStruct = new DojoCallsManager.RevokeTradeReinforcementStruct { gameId = DojoEntitiesDataManager.gameDataInstance.gameId, tradeId = (uint)int.Parse(tradeId) };
            var endpoint = new DojoCallsManager.EndpointDojoCallStruct { account = DojoEntitiesDataManager.currentAccount, addressOfSystem = DojoCallsManager.tradeReinfActionsAddress, functionName = "revoke" };

            await DojoCallsManager.RevokeTradeReinforcementDojoCall(revokeStruct,endpoint);
        }
    }

    public async void ChangePrice()
    {
        if (owner)
        {
            var changeTradePriceStruct = new DojoCallsManager.ModifyTradeReinforcementStruct { gameId = 1, price = new Dojo.Starknet.FieldElement(20), tradeId = (uint)int.Parse(tradeId) };
            var endpoint = new DojoCallsManager.EndpointDojoCallStruct { account = DojoEntitiesDataManager.currentAccount, addressOfSystem = DojoCallsManager.tradeReinfActionsAddress, functionName = "revoke" };

            await DojoCallsManager.ModifyTradeReinforcementDojoCall(changeTradePriceStruct, endpoint);
        }
    }


    public async void PurchaseTrade() { 
        if (!owner)
        {
            var purchaseStruct = new DojoCallsManager.PurchaseTradeReinforcementStruct { gameId = DojoEntitiesDataManager.gameDataInstance.gameId, tradeId = (uint)int.Parse(tradeId) };
            var endpoint = new DojoCallsManager.EndpointDojoCallStruct { account = DojoEntitiesDataManager.currentAccount, addressOfSystem = DojoCallsManager.tradeReinfActionsAddress, functionName = "purchase" };

            await DojoCallsManager.PurchaseTradeReinforcementDojoCall(purchaseStruct, endpoint);
        }
    }
}
