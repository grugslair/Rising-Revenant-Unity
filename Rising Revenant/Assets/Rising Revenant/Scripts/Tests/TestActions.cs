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

   

    public async void CreateEvent()
    {
        if (contractAddress.burnerManager.CurrentBurner == null) { return; }

        var createRevenantsProps = new DojoCallsManager.CreateEventStruct
        {
            gameId = DojoEntitiesDataManager.currentGameId
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "create",
            addressOfSystem = DojoCallsManager.eventActionsAddress,
            account = DojoEntitiesDataManager.currentAccount,
        };

        var transaction = await DojoCallsManager.CreateEventDojoCall(createRevenantsProps, endpoint);
    }

   
}

