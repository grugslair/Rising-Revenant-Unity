using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepPhaseUIManager : MonoBehaviour
{
    [SerializeField]
    private CounterUiElement summonRevCounter;
    [SerializeField]
    private CounterUiElement buyReinfCounter;

    public InitializeDojoEntities contractAddress;

   

    public async void BuyReinfCall()
    {
        var createRevenantsProps = new DojoCallsManager.PurchaseReinforcementsStruct
        {
            gameId = 1,
            count = (uint)buyReinfCounter.currentValue,
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "purchase_reinforcement",
            addressOfSystem = contractAddress.revenantActionsAddress,
            account = contractAddress.burnerManager.CurrentBurner,
        };

        var transaction = await DojoCallsManager.PurchaseReinforcementsDojoCall(createRevenantsProps, endpoint);
    }

}
