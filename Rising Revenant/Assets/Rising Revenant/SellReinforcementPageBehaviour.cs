using System;
using TMPro;
using UnityEngine;

public class SellReinforcementPageBehaviour : Menu
{
    [SerializeField]
    private CounterUiElement reinfSellCounter;
    [SerializeField]
    private TMP_InputField reinfSellInputField;

    public async void SellReinforcements()
    {
        if (reinfSellCounter.currentValue == 0 || reinfSellInputField.text == "") { return; }

        DojoCallsManager.CreateTradeReinforcementStruct structToSellReinf = new DojoCallsManager.CreateTradeReinforcementStruct
        {
            count = (UInt32)reinfSellCounter.currentValue,
            gameId = DojoEntitiesDataManager.currentGameId,
            price = new Dojo.Starknet.FieldElement(int.Parse(reinfSellInputField.text))
        };

        DojoCallsManager.EndpointDojoCallStruct endPoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            account = DojoEntitiesDataManager.currentAccount,
            addressOfSystem = DojoCallsManager.tradeReinforcementActionsAddress,
            functionName = "create",
        };

        await DojoCallsManager.CreateTradeReinforcementDojoCall(structToSellReinf, endPoint);
    }
}
