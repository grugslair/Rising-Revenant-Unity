using Dojo.Starknet;
using dojo_bindings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestActions : MonoBehaviour
{
    public InitializeDojoEntities contractAddress;

    [Space(30)]
    public int revenantId = 1;
    public int outpostId = 1;
    public UInt32 revenantsCount = 1;

    [Space(30)]
    public UInt32 gameId = 1;
    public UInt32 reinforcemetCount = 1;

    [Space(30)]
    public UInt32 tradeId = 1;

    [Space(30)]
    public int worldEventId = 1;

    public async void DeployBurner()
    {
        var burner = await contractAddress.burnerManager.DeployBurner();
    }

    private void Start()
    {
        Debug.Log(RisingRevenantUtils.GetConsistentRandomNumber(123, 444, 0, 21));
        Debug.Log(RisingRevenantUtils.GetConsistentRandomNumber(123, 444, 0, 21));

        Debug.Log(RisingRevenantUtils.GetConsistentRandomNumber(123, 22, 0, 25));
    }

    public async void StartGame()
    {
        if (contractAddress.burnerManager.CurrentBurner == null) { return; }

        var createGameProps = new DojoCallsManager.CreateGameStruct
        {
            preparationPhaseInterval = 10,
            eventInterval = 5,
            ercAddress = contractAddress.burnerManager.CurrentBurner.Address,
            rewardPoolAddress = contractAddress.burnerManager.CurrentBurner.Address,
            revenantInitPrice = new Dojo.Starknet.FieldElement(10),
            maxAmountOfRevenants = 10,
            transactionFeePercentage = 85,
            championPrizePercentage = 5,
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "create",
            addressOfSystem=  contractAddress.gameActionsAddress, 
            account = contractAddress.burnerManager.CurrentBurner,
        };

        var transaction = await DojoCallsManager.CreateGameDojoCall(createGameProps, endpoint);
    }

    public async void ReinforceOutpost()
    {
        if (contractAddress.burnerManager.CurrentBurner == null) { return; }

        var reinforceOutpostProps = new DojoCallsManager.ReinforceOutpostStruct
        {
            gameId = gameId,
            count = reinforcemetCount,
            outpostId = new FieldElement(outpostId)
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "reinforce_outpost",
            addressOfSystem = contractAddress.revenantActionsAddress,
            account = contractAddress.burnerManager.CurrentBurner,
        };

        var transaction = await DojoCallsManager.ReinforceOutpostDojoCall(reinforceOutpostProps, endpoint);
    }

    public async void CreateRevenant()
    {
        if (contractAddress.burnerManager.CurrentBurner == null) { return; }

        var createRevenantsProps = new DojoCallsManager.CreateRevenantsStruct
        {
            gameId = gameId,
            count = revenantsCount,
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "create",
            addressOfSystem = contractAddress.revenantActionsAddress,
            account = contractAddress.burnerManager.CurrentBurner,
        };

        var transaction = await DojoCallsManager.CreateRevenantsDojoMultiCall(createRevenantsProps, endpoint);
    }

    public async void PurchaseReinforcements()
    {
        if (contractAddress.burnerManager.CurrentBurner == null) { return; }

        var createRevenantsProps = new DojoCallsManager.PurchaseReinforcementsStruct
        {
            gameId = gameId,
            count = reinforcemetCount,
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "purchase_reinforcement",
            addressOfSystem = contractAddress.revenantActionsAddress,
            account = contractAddress.burnerManager.CurrentBurner,
        };

        var transaction = await DojoCallsManager.PurchaseReinforcementsDojoCall(createRevenantsProps, endpoint);
    }

    public async void CreateEvent()
    {
        if (contractAddress.burnerManager.CurrentBurner == null) { return; }

        var createRevenantsProps = new DojoCallsManager.CreateEventStruct
        {
            gameId = gameId
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "create",
            addressOfSystem = contractAddress.eventActionsAddress,
            account = contractAddress.burnerManager.CurrentBurner,
        };

        var transaction = await DojoCallsManager.CreateEventDojoCall(createRevenantsProps, endpoint);
    }

    public async void DamageOutpost()
    {
        if (contractAddress.burnerManager.CurrentBurner == null) { return; }

        var createRevenantsProps = new DojoCallsManager.DamageOutpostStruct
        {
            gameId = gameId,
            eventId = new FieldElement(worldEventId),
            outpostId = new FieldElement(outpostId),
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "destroy_outpost",
            addressOfSystem = contractAddress.eventActionsAddress,
            account = contractAddress.burnerManager.CurrentBurner,
        };

        var transaction = await DojoCallsManager.DamageOutpostDojoCall(createRevenantsProps, endpoint);
    }

    public async void CreateTradeReinforcement()
    {
        if (contractAddress.burnerManager.CurrentBurner == null) { return; }

        var createReinforcementTradeProps = new DojoCallsManager.CreateTradeReinforcementStruct
        {
            gameId = gameId,
            count = 1,
            price = new FieldElement(2),
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "create",
            addressOfSystem = contractAddress.tradeReinfActionsAddress,
            account = contractAddress.burnerManager.CurrentBurner,
        };

        var transaction = await DojoCallsManager.CreateTradeReinforcementDojoCall(createReinforcementTradeProps, endpoint);
    }


    public async void RevokeTradeReinforcement()
    {
        if (contractAddress.burnerManager.CurrentBurner == null) { return; }

        var revokeReinforcementTradeProps = new DojoCallsManager.RevokeTradeReinforcementStruct
        {
            gameId = gameId,
            tradeId = tradeId,
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "revoke",
            addressOfSystem = contractAddress.tradeReinfActionsAddress,
            account = contractAddress.burnerManager.CurrentBurner,
        };

        var transaction = await DojoCallsManager.RevokeTradeReinforcementDojoCall(revokeReinforcementTradeProps, endpoint);
    }

    public async void PurchaseTradeReinforcement()
    {
        if (contractAddress.burnerManager.CurrentBurner == null) { return; }

        var purchaseTradeReinforcementStruct = new DojoCallsManager.PurchaseTradeReinforcementStruct
        {
            gameId = gameId,
            tradeId = tradeId,
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "purchase",
            addressOfSystem = contractAddress.tradeReinfActionsAddress,
            account = contractAddress.burnerManager.CurrentBurner,
        };

        var transaction = await DojoCallsManager.PurchaseTradeReinforcementDojoCall(purchaseTradeReinforcementStruct, endpoint);
    }



    public async void testCall()
    {
        if (contractAddress.burnerManager.CurrentBurner == null) { return; }

        
        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "get_current_block",
            addressOfSystem = contractAddress.gameActionsAddress,
            account = contractAddress.burnerManager.CurrentBurner,
        };

        var trans = await endpoint.account.ExecuteRaw(new dojo.Call[]
        {
            new dojo.Call
            {
            calldata = new dojo.FieldElement[]
            {
            },
               selector = endpoint.functionName,
                to = endpoint.addressOfSystem,
            }
        });

        trans.OnBeforeSerialize();

        Debug.Log(trans.ToString());
        Debug.Log(trans.Inner());

        Debug.Log(trans.Hex());


    }

    public void Hello()
    { }
}

