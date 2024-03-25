using System.Linq;
using UnityEngine;
using Types;
using Utils;
using StarkSharp.Rpc.Utils;
using System.Globalization;
using System.Numerics;
using System;

public class GamePlay : MonoBehaviour
{
    private string gameObjectName = "UnityMainThreadDispatcher";
    private string worldAddress = "0x04718f5a0fc34cc1af16a1cdee98ffb20c31f5cd61d6ab07201858f4287c938d";

    public void Entity(string component, Query query, int offset = 0, int length = 0)
    {
        string[] calldata = new string[]{
            StarknetOps.CalculateFunctionSelector(component),
            query.AddressDomain,
            string.Join(",", query.Keys.Select(k => k.ToString())),
            offset.ToString(),
            length.ToString()
        };
        string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = calldata });
        JSInteropManager.CallContract(worldAddress, WorldEntryPoints.Get, calldataString, gameObjectName, "Callback");
    }

    public void Entities(string component, int length)
    {
        string[] calldata = new string[]{
            component,
            StarknetOps.CalculateFunctionSelector(length.ToString())
        };
        string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = calldata });
        JSInteropManager.CallContract(worldAddress, WorldEntryPoints.Entities, calldataString, gameObjectName, "Callback");
    }

    public void Component(string name)
    {
        string[] calldata = new string[]{
            StarknetOps.CalculateFunctionSelector(name)
        };
        string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = calldata });
        JSInteropManager.CallContract(worldAddress, WorldEntryPoints.Component, calldataString, gameObjectName, "Callback");
    }

    public void Execute(string[] callData)
    {
        string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = callData });
        JSInteropManager.SendTransactionBraavos(worldAddress, "transfer", calldataString, gameObjectName, "Callback");
    }


    public void Transfer()
    {
        string contractAddress = "0x04718f5a0fc34cc1af16a1cdee98ffb20c31f5cd61d6ab07201858f4287c938d";

        var arr = new string[3] { "0x00d8f5468626409c9af775c7994f4b4d1e7f17edbecd82ad548d3632b8773538", "10000000000", "0" };

        string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = arr });
        JSInteropManager.SendTransactionArgentX(contractAddress, "transfer", calldataString, gameObjectName, "Callback");
    }

    public void BalanceOf()
    {
        try
        {
            string userAddress = "0x04eD97D7549E11745300b30036c327f14d70B5cDDE7085A8554327478f09c8Bc";
            string contractAddress = "0x04718f5a0fc34cc1af16a1cdee98ffb20c31f5cd61d6ab07201858f4287c938d";

            string[] calldata = new string[1];
            calldata[0] = userAddress;
            string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = calldata });
            JSInteropManager.CallContract(contractAddress, "balanceOf", calldataString, gameObjectName, "Erc721Callback");
        }
        catch (Exception ex)
        {
            Debug.LogError($"error on the main function: {ex.Message}");
        }

    }

    public void Erc721Callback(string response)
    {
        try
        {
            JsonResponse jsonResponse = JsonUtility.FromJson<JsonResponse>(response);
            BigInteger balance = BigInteger.Parse(jsonResponse.result[0].Substring(2), NumberStyles.HexNumber);
            Debug.Log(balance);
        }
        catch (Exception ex)
        {
            Debug.LogError($"error on the callback {ex.Message}");
        }
    }

    public void Callback(string response)
    {
        JsonResponse jsonResponse = JsonUtility.FromJson<JsonResponse>(response);
        Debug.Log(jsonResponse.result);
    }
}
